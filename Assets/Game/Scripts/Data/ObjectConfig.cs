using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectConfig
{
    public string name;
    public List<ObjectData> objectData = new();
    public List<ObjectTrait> traits = new();
    public List<BaseSpecialEvent> spawnEvents = new();
    public List<BaseSpecialEvent> despawnEvents = new();
    public List<StageNum> excludedStages = new();
    private readonly List<StageObject> _instances = new();
    private Type _mainType;

    public ObjectConfig GetConfigByType<T>() where T : MonoBehaviour { return typeof(T) == _mainType ? this : null; }

    public ObjectConfig GetConfigByTrait(ObjectTrait trait)
    {
        if (traits.Count == 0 || !traits.Contains(trait)) return null;
        return this;
    }
    
    public void Spawn()
    {
        if (Application.isPlaying)
        {
            var currentStage = G.GetService<SpecialGameStatesService>().GetState<CurrentGameStage>().Get();
            if (excludedStages.Contains(currentStage)) return;
        }
        foreach (var data in objectData)
        {
            var instance = SpawnManager.Spawn(data.prefab, data.position, data.rotation);
            if (instance.TryGetComponent<InteractableObject>(out var interactableObject)) interactableObject.Enable();
            instance.transform.localScale = data.scale;
            instance.name = data.prefab.name;
            _instances.Add(instance);
        }
        if (spawnEvents.Count > 0) return;
        foreach (var spawnEvent in spawnEvents) { spawnEvent.Invoke(this); }
    }

    public void Despawn()
    {
        if (Application.isPlaying)
        {
            foreach (var instance in _instances) { SpawnManager.Despawn(instance.gameObject); }
            foreach (var despawnEvent in despawnEvents) { despawnEvent.Invoke(this); }
        }
        else
        {
            foreach (var instance in _instances) { SpawnManager.DespawnImmediate(instance.gameObject); }
        }
        _instances.Clear();
    }

    public void SaveState()
    {
        for (var i = 0; i < _instances.Count; i++)
        {
            objectData[i].position = _instances[i].transform.localPosition;
            objectData[i].rotation = _instances[i].transform.localRotation;
            objectData[i].scale = _instances[i].transform.localScale;
        }
    }
    
    public List<StageObject> GetInstances() => _instances;
}

[Serializable]
public class ObjectData
{
    public StageObject prefab;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}

public enum ObjectTrait
{
    Unlockable
}