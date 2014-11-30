// This class knows the progress the player has made and it is used by for example computers to control what to show.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum WorldEvent
{
    StartInfoAvailable,
    StartInfoRead,
    EngineOverheating,
    EngineBlownUp,
    VentGreenHouseInside,
    VentGreenHouseOutside,
    OxygenInfoAvailable,
    OxygenInfoRead,
    ShipmentFailureInfoAvailable,
    ShipmentFailureInfoRead,
    DiagnosticRun,
    ColonyInfoAvailable,
    ColonyInfoRead,
    PlottedForEarth,
    PlottedForEuropa,
    End
};

public enum RoomType
{
    LivingQuarters,
    Bridge,
    DiningRoom,
    Medbay,
    ScienceLab,
    Greenhouse,
    Engineering
};

public class ConditionedAction
{
    public ConditionedAction(Func<bool> predicate, Action action)
    {
        Predicate = predicate;
        Action = action;
    }

    public Func<bool> Predicate { get; private set; }
    public Action Action { get; private set; }
}

public class Message
{
    public Message(string subject, float time, string text)
    {
        Subject = subject;
        Text = text;
        Time = time;
        Read = false;
    }
    
    public void MarkRead()
    {
        Read = true;
    }

    public float Time { get; private set; }
    public string Subject { get; private set; }
    public string Text { get; private set; }
    public bool Read { get; private set; }
}

public static class WorldState
{
    static WorldState()
    {
        SetHappened(WorldEvent.StartInfoAvailable);
        AddNewMessage("Godspeed", TimeWithOffset(-126194123),
            @"Good luck on your journey! Always remember that
you are, by piloting these supply missions to
EUROPA II, a hero. The supplies your ship
carries will feed and maintain the EUROPA II
science colony for years to come.

The safety of these supply missions are always
of utmost concern and we can proudly state that
no EUROPA II supply mission has ever suffered
any major problems.

Regards,
Traval Blansson
Space-V Shipyard Overseer");
        Messages.ForEach(x => x.MarkRead());
    }

    private static readonly List<Message> Messages = new List<Message>();
    private static readonly HashSet<WorldEvent> PastEvents = new HashSet<WorldEvent>();
    private static readonly List<ConditionedAction> ConditionedActions = new List<ConditionedAction>();

    public static bool AnyNewMessages()
    {
        return Messages.Any(x => !x.Read);
    }

    public static void AddNewMessage(string subject, string text)
    {
        AddNewMessage(subject, TimeWithOffset(0), text);
    }

    private static float TimeWithOffset(float offset)
    {
        var epochStart = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);
        return (float)(DateTime.UtcNow - epochStart).TotalSeconds + 788432121 + offset;
    }

    public static void AddNewMessage(string subject, float time, string text)
    {
        Messages.Add(new Message(subject, time, text));
        Messages.Sort((m1, m2) => m1.Time.CompareTo(m2.Time));
    }

    public static List<Message> AllMessages()
    {
        return Messages;
    }

    public static bool HasHappened(WorldEvent evt)
    {
        return PastEvents.Contains(evt);
    }

    public static bool HasHappened(IEnumerable<WorldEvent> evt)
    {
        return evt.All(PastEvents.Contains);
    }

    public static void SetHappened(WorldEvent evt)
    {
        PastEvents.Add(evt);
    }

    public static void SetFutureEvent(float time, WorldEvent evt)
    {
        ConditionedActions.Add(new ConditionedAction(
            () => Time.time > time,
            () => SetHappened(evt)));
    }

    public static void AddHappenSometimeBefore(float time, Action action)
    {
        AddHappenSometimeBetween(Time.time, time, action);
    }

    public static void AddHappenSometimeBetween(float earliest, float latest, Action action)
    {
        float timeSpan = latest - earliest;
        float runAtTime = earliest + (timeSpan * UnityEngine.Random.value);

        ConditionedActions.Add(new ConditionedAction(
            () => Time.time > runAtTime,
            action));
    }

    public static void AddHappenOnEnterRoom(RoomType targetRoom, Action action)
    {
        bool hasEntered = false;
        RoomController.RoomChangedHandler handler;
        handler = (room) => {
            if (targetRoom == room.Type)
            {
                hasEntered = true;
                RoomController.Instance.OnRoomChanged -= handler;
            }
        };
        RoomController.Instance.OnRoomChanged += handler;

        ConditionedActions.Add(new ConditionedAction(
            () => hasEntered,
            action));
    }

    public static void AddConditionedAction(ConditionedAction action)
    {
        ConditionedActions.Add(action);
    }

    public static bool HasEndState()
    {
        return new[]
        {
            WorldEvent.EngineBlownUp,
            WorldEvent.VentGreenHouseInside,
            WorldEvent.End
        }.Any(PastEvents.Contains);
    }

    public static void Update()
    {
        foreach (var conditionedAction in ConditionedActions)
        {
            if (!conditionedAction.Predicate())
                continue;

            conditionedAction.Action();
        }

        ConditionedActions.RemoveAll(x => x.Predicate());

        if (HasEndState())
            Debug.Log("END");
    }
}
