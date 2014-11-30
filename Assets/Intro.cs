using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

    private float _startTime;
    private float _endTime = 0;
    private int _step = 0;
    private bool _running;

	// Use this for initialization
	void Start () {
        _running = true;
        IntercomHandler.IntroMode = true;
        _startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (_running)
        {
            if (_step < 2 && Time.time > _startTime + 8.0)
            {
                _step = 2;
                IntercomHandler.Broadcast("After many years of uncertainty the scientific operation at base EUROPA II on Europa has finally started to stabilise.", null, true);
                IntercomHandler.Broadcast("Being far, far away from home not even replication technology can cut the proverbial umbilical chord to planet Earth.", null, true);
                IntercomHandler.Broadcast("Concurrently running shipments of supplies to the new base are critical.", null, true);
                IntercomHandler.Broadcast("The trip is far. The transeuropan trajectory takes more than 9 years, and not everyone are cut out to be alone for that long.", null, true);
                IntercomHandler.Broadcast("Is anyone?", () => {
                    _endTime = Time.time + 4;
                }, true);
            }
            else if (_endTime != 0 && Time.time > _endTime)
            {
                End();
            }
        }
	}

    public void End()
    {
        IntercomHandler.IntroMode = false;
        IntercomHandler.Clear();
        WorldState.AddHappenAt(Time.time + 4, () => IntercomHandler.Instance.SwapToPostIntroBuffer());
        RoomController.Instance.CurrentRoom = GameObject.Find("LivingQuarters").transform;
        _endTime = 0;
        _running = false;
    }
}
