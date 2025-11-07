using System;
using System.Collections.Generic;
using UnityEngine;



public class UT_EventService : MonoBehaviour, UT_IEventService
{
    private UT_EventManager _EventManager;

    public void Initialize(UT_EventManager EventManager)
    {
        _EventManager = EventManager;
    }

    public void Dispatch<T>(T Event = default) where T : UT_Event, new()
    {
        if (_EventManager == null)
            return;

        T EventToDispatch = Event ?? new T();

        _EventManager.Dispatch(EventToDispatch);
    }

    public void Subscribe<T>(Action<T> Handler) where T : UT_Event
    {
        if (_EventManager == null)
            return;

        _EventManager.Subscribe(Handler);
    }

    public void Unsubscribe<T>(Action<T> Handler) where T : UT_Event
    {
        if (_EventManager == null)
            return;

        _EventManager.Unsubscribe(Handler);
    }
}
