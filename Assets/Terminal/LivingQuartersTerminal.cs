using System.Collections.Generic;
using UnityEngine;

class LivingQuartersTerminal : ScreenBehahvior
{
    public ScreenInfo CurrentInfo
    {
        get
        {
            return new ScreenInfo("Bye",
    
            new List<ScreenAction>
            {
                new ScreenAction("Sign off", () => null)
            });
        }
    }

    public bool ShowMessages
    {
        get
        {
            return WorldState.HasHappened(WorldEvent.StartInfoRead);
        }
    }

    public bool ShowMap
    {
        get { return WorldState.HasHappened(WorldEvent.StartInfoRead); }
    }
}