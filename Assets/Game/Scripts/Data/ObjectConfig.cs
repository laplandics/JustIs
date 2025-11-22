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
    public List<StageNum> stages = new();
    public bool saveStateBetweenStages;
    
    [Header("InGame info")]
    [SerializeField] private List<StageObject> instances = new();

    public ObjectConfig GetConfigByName(string getName) => name[..name.IndexOf('_')] == getName ? this : null;

    public void Spawn(bool cameraPos = false)
    {
        var playing = Application.isPlaying;
        if (playing) if (traits.Contains(ObjectTrait.Locked)) return;
        foreach (var data in objectData)
        {
            var dataPos = data.position;
            if (cameraPos)
            {
                var cameraTr = G.GetManager<CameraManager>().GetCameraTransform();
                dataPos = cameraTr.position + cameraTr.forward;
            }
            var instance = SpawnManager.Spawn(data.prefab, dataPos, data.rotation);
            if (instance.TryGetComponent<InteractableObject>(out var interactableObject) && playing) interactableObject.Enable();
            instance.transform.localScale = data.scale;
            instance.name = data.prefab.name;
            instances.Add(instance);
        }
        if (spawnEvents.Count == 0 || !playing) return;
        foreach (var spawnEvent in spawnEvents) { spawnEvent.Invoke(this); }
    }

    public void Despawn()
    {
        var playing = Application.isPlaying;
        if (!playing) { foreach (var instance in instances) { SpawnManager.DespawnImmediate(instance.gameObject); } instances.Clear(); return; }
        foreach (var instance in instances) { SpawnManager.Despawn(instance.gameObject); }
        foreach (var despawnEvent in despawnEvents) { despawnEvent.Invoke(this); }
        instances.Clear();
    }

    public void SaveState()
    {
        if (Application.isPlaying) if (!saveStateBetweenStages) return;
        for (var i = 0; i < instances.Count; i++)
        {
            if (objectData.Count <= i)
            {
                objectData.Add(new ObjectData
                {
                    prefab = objectData[0].prefab,
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
    
    public List<StageObject> GetInstances() => instances;
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
    Locked,
    Unlocked
}