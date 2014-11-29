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
    public Message(string subject, string text)
    {
        Subject = subject;
        Text = text;
        Read = false;
    }

    public void MarkRead()
    {
        Read = true;
    }
    
    public string Subject { get; private set; }
    public string Text { get; private set; }
    public bool Read { get; private set; }
}

public static class WorldState
{
    static WorldState()
    {
        SetHappened(WorldEvent.StartInfoAvailable);
        AddNewMessage("God Speed", "Good luck on your most important\njourney LOLS.");
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
        Messages.Add(new Message(subject, text));
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
