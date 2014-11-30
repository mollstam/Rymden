using System.Collections.Generic;
using UnityEngine;

namespace Assets.Terminal
{
    class VentGreenhouseScreen : ScreenBehahvior
    {
        public ScreenInfo CurrentInfo
        {
            get
            {
                return new ScreenInfo(
                    @"Venting greenhouse atmosphere
-----------------------------

Are you sure? Venting the atmosphere will make
most living matter in the room perish due to
the instant temperature fall.", new List<ScreenAction> {
                    new ScreenAction("Yes", () =>
                    {
                        WorldState.SetHappened(WorldEvent.VentGreenHouseOutside);
                        WorldState.AddHappenSometimeBefore(Time.time + 20, () =>
                        {
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
                        WorldState.AddHappenOnEnterRoom(RoomType.Bridge, () =>
                        {
                            Debug.Log("We entered bridge?");
                        });
                        return null;
                    }),
                    new ScreenAction("No", () => null)});
            }
        }

        public bool ShowMessages
        {
            get { return false; }
        }
        
        public bool ShowMap
        {
            get { return false; }
        }
    }

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
                        "--------------------\n\n" +
                        "Warning, flames detected in greenhouse.\n",

                        new List<ScreenAction>
                        {
                            new ScreenAction("Vent greenhouse atmosphere", () => new VentGreenhouseScreen()),
                            new ScreenAction("Sign off", () => null)
                        });
                }


                return new ScreenInfo(
                    "Science Lab Computer\n" +
                    "--------------------",
                    
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
