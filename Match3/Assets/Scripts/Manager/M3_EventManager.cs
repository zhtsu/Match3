using System;
using System.Collections.Generic;
using static Unity.Collections.AllocatorManager;

public class M3_EventManager : M3_IManager
{
    public string ManagerName
    {
        get { return "EventManager"; }
    }

    public delegate void EventHandler<T>(T Event);

    private static Dictionary<Type, Delegate> _EventHandlers = new Dictionary<Type, Delegate>();

    public void Initialize()
    {

    }

    public void Destroy()
    {

    }

    public static void SendEvent<T>(T Event = default)
        where T : M3_Event
    {
        Type EventType = typeof(T);

        if (_EventHandlers.TryGetValue(EventType, out Delegate Handlers))
        {
            (Handlers as EventHandler<T>)?.Invoke(Event);
        }
    }

    public static void Subscribe<T>(EventHandler<T> Handler)
    {
        Type EventType = typeof(T);

        if (_EventHandlers.TryGetValue(EventType, out Delegate ExistingDelegate))
        {
            _EventHandlers[EventType] = Delegate.Combine(ExistingDelegate, Handler);
        }
        else
        {
            _EventHandlers[EventType] = Handler;
        }
    }

    public static void Unsubscribe<T>(EventHandler<T> Handler)
    {
        Type EventType = typeof(T);

        if (_EventHandlers.TryGetValue(EventType, out Delegate ExistingDelegate))
        {
            Delegate NewDelegate = Delegate.Remove(ExistingDelegate, Handler);

            if (NewDelegate == null)
            {
                _EventHandlers.Remove(EventType);
            }
            else
            {
                _EventHandlers[EventType] = NewDelegate;
            }
        }
    }
}
