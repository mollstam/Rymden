using System;
using UnityEngine;
using Screen = UnityEngine.Screen;

public class RoomTransitionAnimator : MonoBehaviour
{
    private enum AnimationState
    {
        NotAnimating,
        MovingTowardsFirstStageTarget,
        MovingTowardsSecondStageTarget
    };

    private GameObject _animatedCamera;
    private SpriteRenderer _overlay;
    private Rect _firstStageRect;
    private Vector2 _firstStageTarget;
    private Rect _secondStageRect;
    private Vector2 _secondStageStart;
    private Vector2 _secondStageTarget;
    private float _distanceTravelled;
    private float _zoomedOutViewSize;
    private float _zoomedInViewSize;
    private AnimationState _animationState;

    public void Start()
    {
        _animationState = AnimationState.NotAnimating;
    }

    private Vector3 ClampCameraInside(Rect container)
    {
        var verticalSize = _animatedCamera.camera.orthographicSize * 2.0f;
        var horizontalSize = verticalSize * UnityEngine.Screen.width / UnityEngine.Screen.height;
        var cameraSize = new Vector2(horizontalSize, verticalSize);

        var cameraRect = new Rect(_animatedCamera.transform.position.x - cameraSize.x / 2, _animatedCamera.transform.position.y - cameraSize.y / 2, cameraSize.x, cameraSize.y);
        var cameraPos = _animatedCamera.transform.position;
        
        if (cameraRect.xMin < container.xMin)
            cameraPos.x = container.x + cameraRect.width / 2;

        if (cameraRect.xMax > container.xMax)
            cameraPos.x = container.xMax - cameraRect.width / 2;
            
        if (cameraRect.yMin < container.yMin)
            cameraPos.y = container.y + cameraRect.height / 2;

        if (cameraRect.yMax > container.yMax)
            cameraPos.y = container.yMax - cameraRect.height / 2;

        //newPos.y = Mathf.Clamp(position.y, rect.y + rect.height / 2, rect.y - rect.height / 2);
        return cameraPos;
    }

    public void Update()
    {
        if (!IsAnimating)
            return;

        switch (_animationState)
        {
            case AnimationState.MovingTowardsFirstStageTarget:
            {
                // Move camera
                Vector2 cameraPos2 = _animatedCamera.transform.position;
                var cameraPosToTarget = _firstStageTarget - cameraPos2;
                Vector3 movementDelta = cameraPosToTarget * Time.deltaTime;
                _animatedCamera.transform.position += movementDelta;

                // Make overlay more opaque
                var currentColor = _overlay.color;
                currentColor.a += Time.deltaTime;
                _overlay.color = currentColor;

                // Zoom in
                var currentZoomToTarget = _zoomedInViewSize - _animatedCamera.camera.orthographicSize;
                _animatedCamera.camera.orthographicSize += currentZoomToTarget * Time.deltaTime;
                _animatedCamera.transform.position = ClampCameraInside(_firstStageRect);

                if (_overlay.color.a > 0.95f)
                {
                    _distanceTravelled = 0;
                    currentColor.a = 1.0f;
                    _overlay.color = currentColor;
                    _animatedCamera.transform.position = new Vector3(_secondStageStart.x, _secondStageStart.y, _animatedCamera.transform.position.z);
                    _animationState = AnimationState.MovingTowardsSecondStageTarget;
                }
            } break;

            case AnimationState.MovingTowardsSecondStageTarget:
            {
                // Make overlay less opaque
                var currentColor = _overlay.color;
                currentColor.a -= Time.deltaTime;
                _overlay.color = currentColor;
                var currentBlend = 1.0f - currentColor.a;

                // Move camera
                Vector2 cameraPos2 = _animatedCamera.transform.position;
                var startToTarget = _secondStageTarget - _secondStageStart;
                Vector3 newPos = _secondStageStart + startToTarget*currentBlend;
                newPos.z = _animatedCamera.transform.position.z;
                _animatedCamera.transform.position = newPos;
                
                // Zoom out
                var currentZoomToTarget = _zoomedOutViewSize - _zoomedInViewSize;
                _animatedCamera.camera.orthographicSize = _zoomedInViewSize + currentZoomToTarget * currentBlend;
                _animatedCamera.transform.position = ClampCameraInside(_secondStageRect);

                if (currentBlend > 1.0f - float.Epsilon)
                {
                    _distanceTravelled = 0;
                    _animatedCamera.camera.orthographicSize = _zoomedOutViewSize;
                    _animatedCamera.transform.position = new Vector3(_secondStageTarget.x, _secondStageTarget.y, _animatedCamera.transform.position.z);
                    _animationState = AnimationState.NotAnimating;
                }
            } break;
        }
    }

    public void DoTransition(GameObject animatedCamera, Vector2 firstStageTarget, Vector2 secondStageStart, Vector2 secondStageTarget, Rect firstStageRect, Rect secondStageRect, float zoomedOutViewSize, float zoomedInViewSize)
    {
        if (IsAnimating)
            throw new Exception("Trying to do transition animation while other animation is in progress");

        _animatedCamera = animatedCamera;
        _overlay = animatedCamera.transform.FindChild("Overlay").GetComponent<SpriteRenderer>();
        _firstStageTarget = firstStageTarget;
        _secondStageStart = secondStageStart;
        _secondStageTarget = secondStageTarget;
        _zoomedOutViewSize = zoomedOutViewSize;
        _zoomedInViewSize = zoomedInViewSize;
        _firstStageRect = firstStageRect;
        _secondStageRect = secondStageRect;
        _distanceTravelled = 0;
        _animationState = AnimationState.MovingTowardsFirstStageTarget;
    }

    public bool IsAnimating
    {
        get { return _animationState != AnimationState.NotAnimating; }
    }
}
