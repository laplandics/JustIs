using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectConfig", menuName = "GameData/ObjectConfig")]
public class ObjectConfig : ScriptableObject
{
    [ReadOnly] public string configName;
    public StageObject configPrefab;
    [Space]
    public List<ObjectData> objectData = new();
    public List<ObjectTrait> traits = new();
    public List<BaseSpecialEvent> spawnEvents = new();
    public List<BaseSpecialEvent> despawnEvents = new();
    public List<StageNum> stages = new();
    public bool saveStateBetweenStages;
    [Space]
    public int spawnOrder;
    [SerializeField] private bool blockSpawn;
    
    [Header("InGame info")]
    [SerializeField] private List<StageObject> instances = new();

    [Button]
    public void Spawn()
    {
        if (blockSpawn) return;
        var playing = Application.isPlaying;
        Stage currentStage = null;
        if (playing) currentStage = G.GetService<SpecialGameStatesService>().GetState<CurrentGameStage>().Get();
        if (playing) { currentStage?.StageConfigs.Add(this); if(traits.Contains(ObjectTrait.Locked)) return; }
        foreach (var data in objectData)
        {
            var dataPos = data.position;
            var instance = SpawnManager.Spawn(configPrefab, dataPos, data.rotation);
            if (instance.TryGetComponent<InteractableObject>(out var interactableObject) && playing) interactableObject.Enable();
            instance.transform.localScale = data.scale;
            instance.name = configPrefab.name;
            instances.Add(instance);
        }
        if (spawnEvents.Count == 0 || !playing) return;
        foreach (var spawnEvent in spawnEvents) { spawnEvent.Invoke(this); }
    }

    public void SpawnDebug()
    {
        if (blockSpawn) return;
        var playing = Application.isPlaying;
        Stage currentStage = null;
        if (playing) currentStage = G.GetService<SpecialGameStatesService>().GetState<CurrentGameStage>().Get();
        if (playing) currentStage?.StageConfigs.Add(this);
        var cameraTr = G.GetManager<CameraManager>().GetCameraTransform();
        var dataPos = cameraTr.position + cameraTr.forward;
        var instance = SpawnManager.Spawn(configPrefab, dataPos, objectData[0].rotation);
        if (instance.TryGetComponent<InteractableObject>(out var interactableObject) && playing) interactableObject.Enable();
        instance.transform.localScale = objectData[0].scale;
        instance.name = configPrefab.name;
        instances.Add(instance);
        if (spawnEvents.Count == 0 || !playing) return;
        foreach (var spawnEvent in spawnEvents) { spawnEvent.Invoke(this); }
    }

    [Button]
    public void Despawn()
    {
        var playing = Application.isPlaying;
        var livingInstances = new List<StageObject>();
        foreach (var instance in instances) { if (instance != null) livingInstances.Add(instance); }
        if (!playing) { foreach (var instance in livingInstances) { SpawnManager.DespawnImmediate(instance.gameObject); } instances.Clear(); return; }
        foreach (var instance in livingInstances) { if (instance.TryGetComponent<InteractableObject>(out var interactableObject)) interactableObject.Disable(); }
        foreach (var instance in livingInstances) { SpawnManager.Despawn(instance.gameObject); }
        foreach (var despawnEvent in despawnEvents) { despawnEvent.Invoke(this); }
        instances.Clear();
    }

    [Button]
    public void SaveState()
    {
        if (Application.isPlaying) if (!saveStateBetweenStages) return;
        for (var i = 0; i < instances.Count; i++)
        {
            if (objectData.Count <= i)
            {
                objectData.Add(new ObjectData
                {
                    position = instances[i].transform.localPosition,
                    rotation = instances[i].transform.localRotation,
                    scale = instances[i].transform.localScale
                });
                continue;
            }
            objectData[i].position = instances[i].transform.localPosition;
            objectData[i].rotation = instances[i].transform.localRotation;
            objectData[i].scale = instances[i].transform.localScale;
        }
    }

    public void CopyConfig(ObjectConfig config)
    {
        configName = config.configName;
        configPrefab = config.configPrefab;
        objectData.Clear();
        foreach (var data in config.objectData)
        {
            var copy = new ObjectData
            {
                position = data.position,
                rotation = data.rotation,
                scale = data.scale
            };
            objectData.Add(copy);
        }
        traits = new List<ObjectTrait>(config.traits);
        spawnEvents = new List<BaseSpecialEvent>(config.spawnEvents);
        despawnEvents = new List<BaseSpecialEvent>(config.despawnEvents);
        stages = new List<StageNum>(config.stages);
        saveStateBetweenStages = config.saveStateBetweenStages;
    }
    
    public ObjectConfig GetConfigByName(string getName) => configName[..configName.IndexOf('_')] == getName ? this : null;
    public List<StageObject> GetInstances() => instances;
}

[Serializable]
public class ObjectData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}

public enum ObjectTrait
{
    Locked,
    Unlocked
}