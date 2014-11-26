using System;
using UnityEngine;
using System.Collections;

public class CameraSway : MonoBehaviour
{
    private Vector3 _initialPosition;

    void Start ()
    {
        _initialPosition = transform.position;
    }
    
    void Update ()
    {
        transform.position = _initialPosition + new Vector3(Mathf.Cos(Time.time) * 2, Mathf.Sin(Time.time), 0) * 0.1f;
    }
}
