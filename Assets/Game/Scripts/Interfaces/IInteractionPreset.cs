using UnityEngine;

public interface IInteractionPreset
{
    public Sprite TargetSprite { get; }
    public Collider InteractionCollider { get; }
    public float Distance { get; }
    public bool IsRelevant(Collider colliderInfo);
    public void PerformInteraction();
    public void CancelInteraction();
    public void UpdateUI(bool showUI);
    public void Reset();
}