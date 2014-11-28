// This class knows the progress the player has made and it is used by for example computers to control what to show.

using System.Collections.Generic;
using System.Linq;

public enum WorldEvent
{
    StartInfoAvailable,
    StartInfoRead,
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
    EndGoToEarth,
    EndGoToEuropa
};

public static class WorldState
{
    static WorldState()
    {
        SetHappened(WorldEvent.StartInfoAvailable);
    }

    public static HashSet<WorldEvent> PastEvents = new HashSet<WorldEvent>();

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
}
