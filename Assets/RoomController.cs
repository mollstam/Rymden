using UnityEngine;

public class RoomController : MonoBehaviour
{
    public Transform StartingRoom;
    private Transform _currentRoom;
    private Transform _currentTerminal;

    public Transform ActiveTransform
    {
        get
        {
            if (CurrentTerminal != null)
                return CurrentTerminal;

            return CurrentRoom;
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

    public Transform CurrentTerminal
    {
        get { return _currentTerminal; }
        set
        {
            InactivateAllRooms();
            _currentTerminal = value;

            if (value == null)
                _currentRoom.gameObject.SetActive(true);
            else
                _currentTerminal.gameObject.SetActive(true);

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
        if (_currentTerminal != null)
        {
            var computerPos = _currentTerminal.position;
            transform.position = new Vector3(computerPos.x, computerPos.y, transform.position.z);
            return;
        }

        var roomPos = _currentRoom.position;
        transform.position = new Vector3(roomPos.x, roomPos.y, transform.position.z);
    }

    public void Start()
    {
        CurrentRoom = StartingRoom;
    }

    public void Update()
    {
        if (Input.GetMouseButtonUp(0))
            HandleLeftMouseClick();

        if (CurrentTerminal != null && (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonUp(1)))
            CurrentTerminal = null;
    }

    private void HandleLeftMouseClick()
    {
        if (CurrentTerminal != null)
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

        var terminals = GameObject.FindGameObjectsWithTag("Terminal");

        foreach (var terminal in terminals)
        {
            if (terminal.collider2D.bounds.Contains(mousePosition))
            {
                CurrentTerminal = terminal.GetComponent<DoorWay>().Target.transform;
                return;
            }
        }
    }
}
