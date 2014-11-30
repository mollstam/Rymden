using UnityEngine;
using System.Collections;

public class EndMover : MonoBehaviour
{
    public float Speed = 0.1f;
	
	// Update is called once per frame
	void Update () {
    if (WorldState.HasHappened(WorldEvent.PlottedForEarth) || WorldState.HasHappened(WorldEvent.PlottedForEuropa))
        {
            transform.Translate(Time.deltaTime * Speed, Time.deltaTime * Speed * 0.7f, 0);
        }
	}
}
