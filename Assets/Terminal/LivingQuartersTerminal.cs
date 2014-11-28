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
                return new ScreenInfo("Sorry to wake you at this\nhour. Bladididblabla", new List<ScreenAction>
                {
                    new ScreenAction("Do a backflip", () =>
                    {
                        WorldState.SetHappened(WorldEvent.StartInfoRead);
                        Debug.Log("Doing a backflip");
                        return this;
                    }),
                    new ScreenAction("Blow up", () =>
                    {
                        WorldState.SetHappened(WorldEvent.StartInfoRead);
                        Debug.Log("Blew up, a lot");
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
}