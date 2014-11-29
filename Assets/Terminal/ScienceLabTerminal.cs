using System.Collections.Generic;
using UnityEngine;

namespace Assets.Terminal
{
    class ScienceLabTerminal : ScreenBehahvior
    {
        public ScreenInfo CurrentInfo
        {
            get
            {
                if (!(WorldState.HasHappened(WorldEvent.VentGreenHouseInside) || WorldState.HasHappened(WorldEvent.VentGreenHouseOutside)))
                {
                    return new ScreenInfo(
                        "Science Lab Computer\n" +
                        "--------------------------\n\n" +
                        "It is burning in the greenhouse\n" +
                        "",

                        new List<ScreenAction>
                        {
                            new ScreenAction("Flush oxygen in greenhouse", () =>
                            {
                                WorldState.SetHappened(WorldEvent.VentGreenHouseOutside);
                                WorldState.AddHappenSometimeBefore(Time.time + 20, () => {
                                    WorldState.AddNewMessage("URGENT: Other ship destroyed", "Hello there Astronaut! The other ship went boom,\nyou are now alone and it is more important\nthat you reach your goal.\n\nTake care, Earth.");
                                });
                                return null;
                            }),
                            new ScreenAction("Sign off", () => null)
                        });
                }


                return new ScreenInfo(
                    "Science Lab Computer\n" +
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
    }
}
