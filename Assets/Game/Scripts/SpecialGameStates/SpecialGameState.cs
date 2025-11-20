using UnityEngine;

public abstract class SpecialGameState : ScriptableObject { }

public abstract class SpecialGameState<T> : SpecialGameState
{
    [SerializeField] protected T value;
    
    public virtual void Set(T newValue) { value = newValue; }

    public virtual T Get() { return value; }
}