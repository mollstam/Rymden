using System.Collections.Generic;
using UnityEngine;

namespace Assets.Terminal
{
    static class CommandBridgeTerminalConstants
    {
        public static string HeatWarning =
            "!!! CAUTION, HIGH HEAT IN ENGINEERING !!!\n" +
            "!!! ENGINES NEED DIAGNOSTICS !!!\n" +
            "!!! MAKING A BURN MIGHT CAUSE PERMANENT DAMAGE !!!\n";
    }

    class CommandBridgeTerminalPlotCourse : ScreenBehahvior
    {

        public ScreenInfo CurrentInfo
        {
            get
            {
                if (!WorldState.HasHappened(WorldEvent.DiagnosticRun))
                {
                    return new ScreenInfo(
                        "Plotting Course\n" +
                        "---------------\n\n" +
                        CommandBridgeTerminalConstants.HeatWarning +
                        "",

                        new List<ScreenAction>
                        {
                            new ScreenAction("To Earth", () =>
                            {
                                WorldState.SetFutureEvent(Time.time + 60, WorldEvent.EngineBlownUp);
                                WorldState.SetHappened(WorldEvent.EngineOverheating);
                                WorldState.SetHappened(WorldEvent.PlottedForEarth);
                                return null;
                            }),
                            new ScreenAction("To Europa", () =>
                            {
                                WorldState.SetFutureEvent(Time.time + 60, WorldEvent.EngineBlownUp);
                                WorldState.SetHappened(WorldEvent.EngineOverheating);
                                WorldState.SetHappened(WorldEvent.PlottedForEuropa);
                                return null;
                            }),
                            new ScreenAction("Exit", () => null)
                        });
                }

            return new ScreenInfo("Bye", new List<ScreenAction>
            {
                new ScreenAction("Sign off", () => null)
            });
            }
        }
    }

    class CommandBridgeTerminal : ScreenBehahvior
    {
        public ScreenInfo CurrentInfo
        {
            get
            {
                if (WorldState.HasHappened(WorldEvent.EngineOverheating))
                {
                    var destination = WorldState.HasHappened(WorldEvent.PlottedForEarth)
                        ? "EARTH"
                        : "EUROPA";

                    return new ScreenInfo(
                        "Ship Navigational Computer\n" +
                        "--------------------------\n\n" +
                        "!!! MAKING NAVIGATIONAL BURN TORWARDS " + destination + " !!!\n" +
                        "!!! ENGINES OVERHEATING !!!\n" +
                        "",

                        new List<ScreenAction>
                        {
                            new ScreenAction("Sign off", () => null)
                        });
                }

                if (!WorldState.HasHappened(WorldEvent.DiagnosticRun))
                {
                    return new ScreenInfo(

                        "Ship Navigational Computer\n" +
                        "--------------------------\n\n" +
                        CommandBridgeTerminalConstants.HeatWarning +
                        "",

                        new List<ScreenAction>
                        {
                            new ScreenAction("Plot course", () => new CommandBridgeTerminalPlotCourse()),
                            new ScreenAction("Sign off", () => null)
                        });
                }

                return new ScreenInfo(
                    "Ship Navigational Computer\n" +
                    "--------------------------\n\n",
                    
                    new List<ScreenAction>
                    {
                        new ScreenAction("Sign off", () => null)
                    });
            }
        }
    }
}
