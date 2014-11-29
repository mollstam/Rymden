using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    public float Speed = 3.0f;

    public void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * Speed);
    }
}
