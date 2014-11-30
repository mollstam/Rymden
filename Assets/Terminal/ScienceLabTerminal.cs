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
                                    WorldState.AddNewMessage("URGENT: Supply ship destroyed",
                                    @"We have some very bad news for you. The ship
before you was hit by the same asteroid storm
as you and was unfortunately destroyed.

This means that the success of your supply
mission is even more important. The colony will
quickly run out of supplies should your load
not reach them. Stay sharp out there!

Best regards,
Flamkik Vlabidodo
ANTESCO Food Supplies Manager");
                                });
                                WorldState.AddHappenOnEnterRoom(RoomType.Bridge, () => {
                                    Debug.Log("We entered bridge?");
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

        public bool ShowMap
        {
            get { return true; }
        }
    }
}
