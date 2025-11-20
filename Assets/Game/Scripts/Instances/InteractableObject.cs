using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : StageObject
{
    private readonly Dictionary<Type, IInteractionPreset> _presets = new();

    private IInteractionPreset CurrentPreset { get; set; }

    public virtual void Enable()
    {
        var presets = GetComponents<IInteractionPreset>();
        foreach (var preset in presets) { _presets.TryAdd(preset.GetType(), preset); }
        Launch();
    }

    protected virtual void Launch() {}

    public void Interact(bool isPressed)
    {
        if(isPressed) CurrentPreset?.PerformInteraction();
        else CurrentPreset?.CancelInteraction();
    }
    public void PrepareInteraction(Collider colliderInfo) => CurrentPreset = GetInteraction(colliderInfo);

    public void DistanceToObjectChanged(Collider colliderInfo)
    {
        if (CurrentPreset == null) {PrepareInteraction(colliderInfo); return;}
        if (CurrentPreset.IsRelevant(colliderInfo)) return;
        CurrentPreset.Reset();
        CurrentPreset = null;
    }

    public void InteractableChanged()
    {
        CurrentPreset?.Reset();
        CurrentPreset = null;
    }

    private IInteractionPreset GetInteraction(Collider colliderInfo)
    {
        IInteractionPreset interaction = null;
        foreach (var preset in _presets.Values)
        {
            if (!preset.IsRelevant(colliderInfo)) continue;
            interaction = preset;
        }
        return interaction;
    }

    private void OnDestroy()
    {
        foreach (var preset in _presets.Values) { preset.Reset(); }
        _presets.Clear();
    }
}