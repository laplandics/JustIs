using UnityEngine;

public class InteractableComponent : InteractableObject
{
    [SerializeField] private InteractableObject mainInteractable;

    public override void Enable() { mainInteractable.Enable(); }
}