using UnityEngine;
using System.Collections;

public class RoomController : MonoBehaviour
{
    public Transform[] Rooms;
    private Transform _currentRoom;

    public Transform CurrentRoom
    {
        get { return _currentRoom; }
        set
        {
            _currentRoom = value;
            var roomPos = _currentRoom.position;
            transform.position = new Vector3(roomPos.x, roomPos.y, transform.position.z);
        }
    }

    void Start ()
    {
        CurrentRoom = Rooms[0];
    }

    void Update ()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var doorways = GameObject.FindGameObjectsWithTag("DoorWay");
            var mousePosition = new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x, camera.ScreenToWorldPoint(Input.mousePosition).y);
            foreach (var doorway in doorways)
            {
                if (doorway.collider2D.bounds.Contains(mousePosition))
                {
                    CurrentRoom = doorway.GetComponent<DoorWay>().Target.transform;
                    break;
                }
            }
        }
    }
}
