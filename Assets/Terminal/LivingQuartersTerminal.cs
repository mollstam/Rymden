using System.Collections.Generic;
using UnityEngine;

class LivingQuartersTerminal : ScreenBehahvior
{
    public ScreenInfo CurrentInfo
    {
        get
        {
            if (WorldState.HasHappened(WorldEvent.StartInfoAvailable) &&
               !WorldState.HasHappened(WorldEvent.StartInfoRead))
            {
                return new ScreenInfo("!!! EMERGENCY !!!\n\n" +
                                      "- ASTEROID STORM DETECTED\n" +
                                      "- COURSE HAS BEEN DIVERTED \n" +
                                      "- ASTEROID HIT DETECTED\n" +
                                      "- FIRE BETWEEN ENGINEERING AND GREENHOUSE\n" +
                                      "- ENGINES OFFLINE, SHIP TUMBLING FREE\n\n", new List<ScreenAction>
                {
                    new ScreenAction("Dismiss warning", () =>
                    {
                        WorldState.SetHappened(WorldEvent.StartInfoRead);
                        return this;
                    })
                });
            }

            return new ScreenInfo("Bye", new List<ScreenAction>
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
}