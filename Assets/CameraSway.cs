using System;
using UnityEngine;
using System.Collections;

public class CameraSway : MonoBehaviour
{
    void Update ()
    {
        var currentRoomPos = GetComponent<RoomController>().ActiveTransform.position;
        var currentPosition = new Vector3(currentRoomPos.x, currentRoomPos.y, transform.position.z);
        var speed = 0.5f;
        transform.position = currentPosition + new Vector3(Mathf.Cos(Time.time * speed) * 2, Mathf.Sin(Time.time * speed), 0) * 0.1f;
    }
}
