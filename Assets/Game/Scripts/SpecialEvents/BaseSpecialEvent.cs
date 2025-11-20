using UnityEngine;

public abstract class BaseSpecialEvent : ScriptableObject
{
    public abstract void Invoke(ObjectConfig config);
}
