using UnityEngine;
using System.Collections;

public class EndCameraSway : MonoBehaviour {
    public void Update()
    {
        transform.position = new Vector3(Mathf.Cos(Time.time) * 2 * 0.4f, Mathf.Sin(Time.time) * 0.4f, -10) * 0.1f;
    }
}
