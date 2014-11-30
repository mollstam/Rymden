using System.Collections.Generic;
using UnityEngine;

namespace Assets.Terminal
{
    class EngineeringDiagnosticsScreen : ScreenBehahvior
    {
        public ScreenInfo CurrentInfo
        {
            get
            {
                return new ScreenInfo(
                    "Starting engine diagnostics...                     \n\n" +
                    "Fuel-injection [OK]                    \n" +
                    "Cooling rods [OK]                          \n" +
                    "Auxiliary thruster engagagers [OK]                             \n" +
                    "Plasma magnetizer breadsmearer [OK]        \n" +
                    "Lunix kernel     ... [Recompiling] ... [OK]                            \n" +
                    "Recalibrating navigation beacons [OK]             \n" +
                    "Thruster [OK]                  \n\n" +
                    "All systems are operational.\n\n",

                    new List<ScreenAction>
                    {
                        new ScreenAction("Exit", () =>
                        {
                            if (!WorldState.HasHappened(WorldEvent.DiagnosticRun))
                            {
                                var currentTime = Time.time;
                                WorldState.AddConditionedAction(new ConditionedAction(
                                    () => Time.time > currentTime + 20,
                                    () => WorldState.AddNewMessage("We REALLY need the supplies!",
                                        "Hi, we just want to make really really sure\n" +
                                        "that your supplies reach us!! You've probably\n" +
                                        "heard that the supply ship before you was\n" +
                                        "destroyed by the asteroid storm. We MUST have\n" +
                                        "your supplies or face starvation.\n\n" +
                                        "Regards,\nMilek Baviddori\nAdministrator of EUROPA II")));
                            }

                            WorldState.SetHappened(WorldEvent.DiagnosticRun);
                            return null;
                        })
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

    class EngineeringScreen : ScreenBehahvior
    {
        public ScreenInfo CurrentInfo
        {
            get
            {
                if (!WorldState.HasHappened(WorldEvent.DiagnosticRun))
                {
                    return new ScreenInfo(
                        "Engineering\n" +
                        "-----------\n\n" +
                        "Massive heat build-up detected near\nengine. Full diagnostic required.",

                        new List<ScreenAction>
                        {
                            new ScreenAction("Run engine diagnostics", () => new EngineeringDiagnosticsScreen()),
                            new ScreenAction("Sign off", () => null)
                        });
                }


                return new ScreenInfo(
                    "Engineering\n" +
                    "-----------",

                    new List<ScreenAction>
                        {
                            new ScreenAction("Run engine diagnostics", () => new EngineeringDiagnosticsScreen()),
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
