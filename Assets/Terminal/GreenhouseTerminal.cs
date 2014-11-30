using System.Collections.Generic;
using UnityEngine;

namespace Assets.Terminal
{
    class VentGreenhouseInsideScreen : ScreenBehahvior
    {
        public ScreenInfo CurrentInfo
        {
            get
            {
                return new ScreenInfo(
                    @"Venting atmosphere in this room
-----------------------------

Are you sure? Venting the atmosphere will make
most living matter in this room perish due to
the instant temperature fall.", new List<ScreenAction> {
                    new ScreenAction("Yes", () =>
                    {
                        WorldState.SetHappened(WorldEvent.VentGreenHouseInside);
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
                        "-------------------\n\n" +
                        @"Warning, flames detected in this room.
Recommended course of action is to vent the
atmosphere from the room, removing the oxygen
feeding the fire. This will also kill any
living entities in here.",

                        new List<ScreenAction>
                        {
                            new ScreenAction("Vent atmosphere in this room", () => new VentGreenhouseInsideScreen()),
                            new ScreenAction("Sign off", () => null)
                        });
                }

                return new ScreenInfo(
                    @"Greenhouse Computer
-------------------

Please note that the recent atmosphere venting
has killed all the plants in this room. This
comes with the consequence that the oxygen will
be insufficient for the one (1) living being
on-board for the most recently plotted course.",

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
