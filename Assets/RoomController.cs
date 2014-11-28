using UnityEngine;

public class RoomController : MonoBehaviour
{
    public Transform StartingRoom;
    private Transform _currentRoom;
    private Terminal _terminal;
    private bool _inTerminal;

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
            InactivateAllRooms();
            _currentRoom = value;
            _currentRoom.gameObject.SetActive(true);
            UpdateCameraPosition();
        }
    }

    public bool InTerminal
    {
        get { return _inTerminal; }
        set
        {
            if (_inTerminal == value)
                return;

            InactivateAllRooms();
            _inTerminal = value;
            _currentRoom.gameObject.SetActive(!_inTerminal);
            _terminal.gameObject.SetActive(_inTerminal);
            UpdateCameraPosition();
        }
    }

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
            return;
        }

        var roomPos = _currentRoom.position;
        transform.position = new Vector3(roomPos.x, roomPos.y, transform.position.z);
    }

    public void Start()
    {
        _terminal = GameObject.Find("TerminalRoom").GetComponent<Terminal>();
        _inTerminal = false;
        CurrentRoom = StartingRoom;
    }

    public void Update()
    {
        if (InTerminal && _terminal.HasQuit)
            InTerminal = false;

        if (Input.GetMouseButtonUp(0))
            HandleLeftMouseClick();

        if (_terminal && (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonUp(1)))
            InTerminal = false;
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
                CurrentRoom = doorway.GetComponent<DoorWay>().Target.transform;
                return;
            }
        }

        var terminals = GameObject.FindGameObjectsWithTag("TerminalWay");

        foreach (var terminal in terminals)
        {
            if (terminal.collider2D.bounds.Contains(mousePosition))
            {
                InTerminal = true;
                _terminal.GetComponent<Terminal>().Reset(terminal.GetComponent<TerminalWay>().StartScreenName);
                return;
            }
        }
    }
}
