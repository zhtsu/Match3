using System;
using System.Collections.Generic;

public class M3_EventManager : M3_Manager
{
    public override string ManagerName
    {
        get { return "Event Manager"; }
    }

    public delegate void EventHandler<T>(T Event);

    private Dictionary<Type, Delegate> _EventHandlers = new Dictionary<Type, Delegate>();

    public override void Initialize()
    {

    }

    public override void Destroy()
    {

    }

    public void SendEvent<T>(T Event = default)
        where T : M3_Event
    {
        Type EventType = typeof(T);

        if (_EventHandlers.TryGetValue(EventType, out Delegate Handlers))
        {
            (Handlers as EventHandler<T>)?.Invoke(Event);
        }
    }

    public void Subscribe<T>(EventHandler<T> Handler)
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

    public void Unsubscribe<T>(EventHandler<T> Handler)
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
