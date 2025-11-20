using UnityEngine;

public class InteractableComponent : InteractableObject
{
    [SerializeField] private InteractableObject parentObject;

    public override void Enable() { parentObject.Enable(); }
}
