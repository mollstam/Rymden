using System.Collections.Generic;
using UnityEngine;

namespace Assets.Terminal
{
    class GreenhouseTerminal : ScreenBehahvior
    {
        public ScreenInfo CurrentInfo
        {
            get
            {
                if (!(WorldState.HasHappened(WorldEvent.VentGreenHouseInside) || WorldState.HasHappened(WorldEvent.VentGreenHouseOutside)))
                {
                    return new ScreenInfo(
                        "Greenhouse Computer\n" +
                        "--------------------------\n\n" +
                        "It is burning\n" +
                        "",

                        new List<ScreenAction>
                        {
                            new ScreenAction("Flush oxygen", () =>
                            {
                                WorldState.SetHappened(WorldEvent.VentGreenHouseInside);
                                return null;
                            }),
                            new ScreenAction("Sign off", () => null)
                        });
                }


                return new ScreenInfo(
                    "Greenhouse Computer\n" +
                    "--------------------------\n\n",
                    
                    new List<ScreenAction>
                    {
                        new ScreenAction("Sign off", () => null)
                    });
            }
        }

        public bool ShowMessages
        {
            get { return true; }
        }

        public bool ShowMap
        {
            get { return true; }
        }
    }
}
