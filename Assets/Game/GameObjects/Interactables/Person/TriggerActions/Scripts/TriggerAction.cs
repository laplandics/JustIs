using UnityEngine;

public abstract class TriggerAction : ScriptableObject
{
    public SpecialGameState state;
    [HideInInspector] public Person targetPerson;
    protected bool ReadyToPerform;
    
    public virtual void PrepareToPerform() { ReadyToPerform = false; }
    public void Reset() { ReadyToPerform = false; }
}
