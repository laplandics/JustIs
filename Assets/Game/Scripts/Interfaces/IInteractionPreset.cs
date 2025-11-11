using UnityEngine;

public interface IInteractionPreset
{
    public Sprite TargetSprite { get; }
    public bool IsRelevant();
    public void PerformInteraction();
    public void UpdateUI(bool showUI);
    public void Reset();
}