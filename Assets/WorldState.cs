// This class knows the progress the player has made and it is used by for example computers to control what to show.

using System.Collections.Generic;

public static class WorldState
{
    public enum WorldEvents
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

    public static List<WorldEvents> PastEvents = new List<WorldEvents>();
}
