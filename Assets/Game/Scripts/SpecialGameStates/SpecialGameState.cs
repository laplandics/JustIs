using System;
using UnityEngine;

public abstract class SpecialGameState : ScriptableObject { }

public abstract class SpecialGameState<T> : SpecialGameState
{
    [SerializeField] protected T value;
    
    public virtual void Set(T newValue) { EventService.Invoke(new StateEvents.OnSomeStateChangedEvent {State = this}); }

    public virtual T Get() { return value; }
}

public abstract class SpecialGameState<T1, T2> : SpecialGameState
{
    [SerializeField] protected T1 value1;
    [SerializeField] protected T2 value2;
    
    public virtual void Set(T1 newValue1, T2 newValue2 = default) { EventService.Invoke(new StateEvents.OnSomeStateChangedEvent {State = this}); }

    public virtual T Get<T>()
    {
        if (typeof(T) == typeof(T1)) return (T)(object)value1;
        if (typeof(T) == typeof(T2)) return (T)(object)value2;
        throw new Exception("Unsupported type");
    }
}