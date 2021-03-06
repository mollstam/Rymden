﻿using UnityEngine;

public class CameraSway : MonoBehaviour
{
    private RoomTransitionAnimator _roomTransitionAnimator;

    public void Start()
    {
        _roomTransitionAnimator = Camera.main.GetComponent<RoomTransitionAnimator>();
    }

    public void Update()
    {
        if (_roomTransitionAnimator.IsAnimating || (WorldState.HasHappened(WorldEvent.PlottedForEarth) || WorldState.HasHappened(WorldEvent.PlottedForEuropa)))
            return;

        var scale = 1.0f;

        if (GetComponent<RoomController>().InTerminal)
            return;

        var currentRoomPos = GetComponent<RoomController>().ActiveTransform.position;
        var currentPosition = new Vector3(currentRoomPos.x, currentRoomPos.y, transform.position.z);
        var speed = 0.5f;
        transform.position = currentPosition + new Vector3(Mathf.Cos(Time.time * speed) * 2 * scale, Mathf.Sin(Time.time * speed) * scale, 0) * 0.1f;
    }
}
