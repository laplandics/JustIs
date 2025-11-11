using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
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

    public void Interact() => CurrentPreset?.PerformInteraction();
    public void PrepareInteraction() => CurrentPreset = GetInteraction();
    public void ResetInteraction() { CurrentPreset?.Reset(); CurrentPreset = null; }

    private IInteractionPreset GetInteraction()
    {
        IInteractionPreset interaction = null;
        foreach (var preset in _presets.Values)
        {
            if (!preset.IsRelevant()) continue;
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