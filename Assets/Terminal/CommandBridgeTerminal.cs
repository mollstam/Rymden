﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Terminal
{
    static class CommandBridgeTerminalConstants
    {
        public static string HeatWarning =
@"Caution, high heat detected in Engineering.
Engines require diagnostics to function
properly, making a burn without doing so may
cause permanent damage or death.";
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
                        CommandBridgeTerminalConstants.HeatWarning,

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

                return new ScreenInfo(
                        "Plotting Course\n" +
                        "---------------\n\n" +
                        "Please select one of these pre-programmed\ndestinations.",

                        new List<ScreenAction>
                        {
                            new ScreenAction("To Earth", () =>
                            {
                                WorldState.SetFutureEvent(60, WorldEvent.PlottedForEarth);
                                WorldState.SetFutureEvent(60, WorldEvent.End);
                                return null;
                            }),
                            new ScreenAction("To Europa", () =>
                            {
                                WorldState.SetFutureEvent(60, WorldEvent.PlottedForEuropa);
                                WorldState.SetFutureEvent(60, WorldEvent.End);
                                return null;
                            }),
                            new ScreenAction("Exit", () => null)
                        });
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
                        CommandBridgeTerminalConstants.HeatWarning,

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
                        new ScreenAction("Plot course", () => new CommandBridgeTerminalPlotCourse()),
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
