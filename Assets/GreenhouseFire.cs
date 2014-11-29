using UnityEngine;
using System.Collections;

public class GreenhouseFire : MonoBehaviour {

    private bool _isBurning;

    public DoorWay EngineeringDoor;

    public bool IsBurning
    {
        get { return _isBurning; }
        set
        {
            _isBurning = value;
            active = _isBurning;
            EngineeringDoor.active = !_isBurning;
        }
    }

    public void Start()
    {
        IsBurning = true;
    }

    public void Update()
    {
        if (IsBurning && (WorldState.HasHappened(WorldEvent.VentGreenHouseInside) || WorldState.HasHappened(WorldEvent.VentGreenHouseOutside)))
        {
            IsBurning = false;
        }
    }

}
