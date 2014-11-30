using UnityEngine;
using System.Collections;

public class EnableWhenBlowingUp : MonoBehaviour {
    private SpriteRenderer _renderer;

    // Use this for initialization
	void Start ()
	{
	    _renderer = GetComponent<SpriteRenderer>();
	    _renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (WorldState.HasHappened(WorldEvent.EngineOverheating))
	    {
	        _renderer.enabled = true;
	    }
	}
}
