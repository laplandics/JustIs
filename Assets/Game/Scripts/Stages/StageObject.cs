using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "[ObjectName]_SO", menuName = "GameData/StageObject")]
public class StageObject : ScriptableObject
{
    public string id;
    public GameObject prefab;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale = Vector3.one;
    public bool load;
    [Space]
    [SerializeField] private ScriptableObject onLoadEvent;
    [SerializeField] private ScriptableObject onUnloadEvent;
    [Space]
    [SerializeField] private List<StageNum> excludeStages = new();
    [HideInInspector] public GameObject instance;

    [Button]
    private void GenerateID() => id = Guid.NewGuid().ToString();

    private void OnValidate()
    {
        EventService.Unsubscribe<OnStageStartEvent>(UpdateLoadStatus);
        EventService.Subscribe<OnStageStartEvent>(UpdateLoadStatus);
    }

    private void UpdateLoadStatus(OnStageStartEvent eventData)
    {
        var currentStage = eventData.StartedStage;
        load = !excludeStages.Contains(currentStage);
    }
    
    [Button]
    public void Load()
    {
        if (!load) return;
        instance = SpawnManager.Spawn(prefab, position, rotation);
        instance.transform.localScale = scale;
        if (instance.TryGetComponent<InteractableObject>(out var interactable)) interactable.Enable();
        if (onLoadEvent == null) return;
        if (onLoadEvent is not ISpecialEvent specialEvent) return;
        specialEvent.Invoke(this);
    }
    
    [Button]
    public void Unload()
    {
        if (instance == null) return;
        if (onUnloadEvent != null && onUnloadEvent is ISpecialEvent specialEvent) specialEvent.Invoke(this);
        if (Application.isPlaying) SpawnManager.Despawn(instance);
        else SpawnManager.DespawnImmediate(instance);
    }
}