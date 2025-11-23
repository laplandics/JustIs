using UnityEngine;

public class InteractableComponent : InteractableObject
{
    [SerializeField] private InteractableObject parentObject;

    public override void Enable() { parentObject.Enable(); }
    public override void Disable() { parentObject.Disable(); }
}
