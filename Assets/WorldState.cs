// This class knows the progress the player has made and it is used by for example computers to control what to show.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum WorldEvent
{
    StartInfoAvailable,
    StartInfoRead,
    EngineOverheating,
    EngineBlownUp,
    VentGreenHouseInside,
    VentGreenHOuseOutside,
    OxygenInfoAvailable,
    OxygenInfoRead,
    ShipmentFailureInfoAvailable,
    ShipmentFailureInfoRead,
    DiagnosticRun,
    ColonyInfoAvailable,
    ColonyInfoRead,
    PlottedForEarth,
    PlottedForEuropa,
    End
};

public class TimedWorldEvent
{
    public TimedWorldEvent(float time, WorldEvent worldEvent)
    {
        Time = time;
        WorldEvent = worldEvent;
    }

    public float Time { get; private set; }
    public WorldEvent WorldEvent { get; private set; }
}

public static class WorldState
{
    static WorldState()
    {
        SetHappened(WorldEvent.StartInfoAvailable);
    }

    public static HashSet<WorldEvent> PastEvents = new HashSet<WorldEvent>();
    public static List<TimedWorldEvent> FutureEvents = new List<TimedWorldEvent>();

    public static bool HasHappened(WorldEvent evt)
    {
        return PastEvents.Contains(evt);
    }

    public static bool HasHappened(IEnumerable<WorldEvent> evt)
    {
        return evt.All(PastEvents.Contains);
    }

    public static void SetHappened(WorldEvent evt)
    {
        PastEvents.Add(evt);
    }

    public static void SetFutureEvent(float time, WorldEvent evt)
    {
        FutureEvents.Add(new TimedWorldEvent(time, evt));
    }

    public static bool HasEndState()
    {
        return new[]
        {
            WorldEvent.EngineBlownUp,
            WorldEvent.VentGreenHouseInside,
            WorldEvent.End
        }.Any(PastEvents.Contains);
    }

    public static void Update()
    {
        var time = Time.time;

        foreach (var timedEvent in FutureEvents)
        {
            if (time <= timedEvent.Time)
                continue;

            PastEvents.Add(timedEvent.WorldEvent);
        }

        FutureEvents.RemoveAll(x => time > x.Time);

        if (HasEndState())
            Debug.Log("END");
    }
}
