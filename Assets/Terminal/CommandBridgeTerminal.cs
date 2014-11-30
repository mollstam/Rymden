using System.Collections.Generic;
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
        private void PlayDramaMusic()
        {
            var music = GameObject.Find("Music");
            
            if (music == null)
                return;

            music.GetComponent<Music>().SetDrama();
        }

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
                                WorldState.SetFutureEvent(Time.time + 33, WorldEvent.EngineBlownUp);
                                WorldState.SetHappened(WorldEvent.EngineOverheating);
                                WorldState.SetHappened(WorldEvent.PlottedForEarth);
                                PlayDramaMusic();
                                return null;
                            }),
                            new ScreenAction("To Europa", () =>
                            {
                                WorldState.SetFutureEvent(Time.time + 33, WorldEvent.EngineBlownUp);
                                WorldState.SetHappened(WorldEvent.EngineOverheating);
                                WorldState.SetHappened(WorldEvent.PlottedForEuropa);
                                PlayDramaMusic();
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
                                WorldState.SetFutureEvent(Time.time, WorldEvent.PlottedForEarth);
                                WorldState.SetFutureEvent(Time.time + 32, WorldEvent.End);
                                PlayDramaMusic();
                                return null;
                            }),
                            new ScreenAction("To Europa", () =>
                            {
                                WorldState.SetFutureEvent(Time.time, WorldEvent.PlottedForEuropa);
                                WorldState.SetFutureEvent(Time.time + 32, WorldEvent.End);
                                PlayDramaMusic();
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
                        ? "Earth"
                        : "Europa";

                    return new ScreenInfo(
                        @"Ship Navigational Computer
--------------------------

Currently making navigation burn towards
"+ destination + @".

The engines are overheating, main cooling is
failing. Core integrity at high risk.",

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
