using UnityEngine;
using System.Collections;

public class EndShake : MonoBehaviour {
    private bool _shaking;
    private float _shakeAmount;
    private float _shakeHardness;
    private float _startedAt;

    // Use this for initialization
    void Start()
    {
     _shaking = false;
     _shakeAmount = 0.1f;
     _shakeHardness = 2.0f;
    }
	
    // Update is called once per frame
    void Update()
    {
        if (!_shaking && (WorldState.HasHappened(WorldEvent.PlottedForEarth) || WorldState.HasHappened(WorldEvent.PlottedForEuropa)))
        {
            _startedAt = Time.time;
            _shaking = true;
        }

        if (!_shaking)
            return;

        if (Time.time > _startedAt + 15)
        {
            _shakeHardness += Time.deltaTime;
            _shakeAmount += Time.deltaTime * 0.1f;
        }

        var controller = GetComponent<RoomController>();
        
        if (controller == null)
            return;

        var currentRoom = controller.ActiveTransform;

        if (currentRoom == null)
            return;

        var currentRoomPos = currentRoom.position;
        var currentPosition = new Vector3(currentRoomPos.x, currentRoomPos.y, transform.position.z);
        transform.position = currentPosition + new Vector3(Mathf.Cos(Time.time * _shakeHardness) * _shakeAmount, Mathf.Sin(Time.time * _shakeHardness) * _shakeAmount * 2.0f, 0) * 0.1f;

        var bg = currentRoom.FindChild("Background");

        if (bg != null)
        {
            var boundsCurrent = bg.GetComponent<SpriteRenderer>().bounds;
            var rectCurernt = new Rect(boundsCurrent.min.x, boundsCurrent.min.y, boundsCurrent.size.x, boundsCurrent.size.y);

            transform.position = ClampCameraInside(rectCurernt);
        }
    }

    private Vector3 ClampCameraInside(Rect container)
    {
        var verticalSize = camera.orthographicSize * 2.0f;
        var horizontalSize = verticalSize * UnityEngine.Screen.width / UnityEngine.Screen.height;
        var cameraSize = new Vector2(horizontalSize, verticalSize);

        var cameraRect = new Rect(camera.transform.position.x - cameraSize.x / 2, camera.transform.position.y - cameraSize.y / 2, cameraSize.x, cameraSize.y);
        var cameraPos = camera.transform.position;

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
}
