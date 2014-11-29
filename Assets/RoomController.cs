using UnityEngine;

public class RoomController : MonoBehaviour
{
    public Transform StartingRoom;
    private Transform _currentRoom;
    private Terminal _terminal;
    private bool _inTerminal;
    private float _roomCameraSize;

    private static RoomController instance;
    private RoomTransitionAnimator _roomTransitionAnimator;

    public static RoomController Instance
    {
        get
        {
            if (instance == null)
                instance = Camera.main.GetComponent<RoomController>();
            return instance;
        }
    }

    public Transform ActiveTransform
    {
        get
        {
            return _inTerminal
                ? _terminal.transform
                : CurrentRoom;
        }
    }

    public Transform CurrentRoom
    {
        get { return _currentRoom; }
        set
        {
            Transform previousRoom = _currentRoom;
            //InactivateAllRooms();
            _currentRoom = value;
            //_currentRoom.gameObject.SetActive(true);
            //UpdateCameraPosition();

            if (_currentRoom != previousRoom && OnRoomChanged != null)
                OnRoomChanged(_currentRoom.GetComponent<Room>());
        }
    }

    public bool InTerminal
    {
        get { return _inTerminal; }
        set
        {
            if (_inTerminal == value)
                return;

            //InactivateAllRooms();
            _inTerminal = value;
            //_currentRoom.gameObject.SetActive(!_inTerminal);
            //_terminal.gameObject.SetActive(_inTerminal);
            UpdateCameraPosition();
        }
    }

    public delegate void RoomChangedHandler (Room room);

    public event RoomChangedHandler OnRoomChanged;

    private void InactivateAllRooms()
    {
        var rooms = GameObject.FindGameObjectsWithTag("Room");

        foreach (var room in rooms)
            room.gameObject.SetActive(false);
    }

    private void UpdateCameraPosition()
    {
        if (InTerminal)
        {
            var computerPos = _terminal.transform.position;
            transform.position = new Vector3(computerPos.x, computerPos.y, transform.position.z);
            camera.orthographicSize = _terminal.CameraSize;
            return;
        }

        camera.orthographicSize = _roomCameraSize;
    }

    public void Start()
    {
        _roomCameraSize = camera.orthographicSize;
        _inTerminal = false;
        _roomTransitionAnimator = GetComponent<RoomTransitionAnimator>();
        CurrentRoom = StartingRoom;
    }

    public void Update()
    {
        if (_roomTransitionAnimator.IsAnimating)
            return;

        if (InTerminal && _terminal.HasQuit)
            InTerminal = false;

        if (Input.GetMouseButtonUp(0))
            HandleLeftMouseClick();

        if (_terminal && (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonUp(1)))
        {
            InTerminal = false;
            _terminal.StopUsing();
            _terminal = null;
        }
    }

    private void HandleLeftMouseClick()
    {
        if (InTerminal)
            return;

        var doorways = GameObject.FindGameObjectsWithTag("DoorWay");
        var mousePosition = new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x, camera.ScreenToWorldPoint(Input.mousePosition).y);

        foreach (var doorway in doorways)
        {
            if (doorway.collider2D.bounds.Contains(mousePosition))
            {
                var doorwayComp = doorway.GetComponent<DoorWay>();
                var startBounds = CurrentRoom.FindChild("Background").GetComponent<SpriteRenderer>().bounds;
                var startRect = new Rect(startBounds.min.x, startBounds.min.y, startBounds.size.x, startBounds.size.y);
                var targetBounds = doorwayComp.Target.transform.FindChild("Background").GetComponent<SpriteRenderer>().bounds;
                var targetRect = new Rect(targetBounds.min.x, targetBounds.min.y, targetBounds.size.x, targetBounds.size.y);

                Debug.Log(startRect);
                Debug.Log(targetRect);

                _roomTransitionAnimator.DoTransition(gameObject, doorway.transform.position, doorwayComp.ExitsTo.transform.position,
                    doorwayComp.Target.transform.position, startRect, targetRect, 5.2f, 2.0f);

                CurrentRoom = doorwayComp.Target.transform;
                return;
            }
        }

        var terminalWays = GameObject.FindGameObjectsWithTag("TerminalWay");

        foreach (var terminalWay in terminalWays)
        {
            if (terminalWay.collider2D.bounds.Contains(mousePosition))
            {
                _terminal = terminalWay.GetComponent<TerminalWay>().Terminal.GetComponent<Terminal>();
                _terminal.StartUsing();
                InTerminal = true;
                return;
            }
        }
    }
}
