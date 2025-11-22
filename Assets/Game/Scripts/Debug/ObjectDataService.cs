using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectDataService", menuName = "Services/ObjectDataService")]
public class ObjectDataService : ScriptableObject, IGameService
{
    [SerializeField] private List<ObjectConfig> configs; 
    private List<ObjectConfig> _initialConfigs;

    public void Run() { CopyConfigs(); }

    private void CopyConfigs()
    {
        _initialConfigs = new List<ObjectConfig>();
        foreach (var config in configs)
        {
            var initialObjectsData = new List<ObjectData>();
            var initialStageList = new List<StageNum>();
            var initialTraitList = new List<ObjectTrait>();
            foreach (var objectData in config.objectData)
            {
                var initialObjectData = new ObjectData
                {
                    prefab = objectData.prefab,
                    position = objectData.position,
                    rotation = objectData.rotation,
                    scale = objectData.scale,
                };
                initialObjectsData.Add(initialObjectData);
            }
            foreach (var stage in config.stages) { initialStageList.Add(stage); }
            foreach (var trait in config.traits) { initialTraitList.Add(trait); }
            var initialConfig = new ObjectConfig
            {
                name = config.name,
                despawnEvents = config.despawnEvents,
                spawnEvents = config.spawnEvents,
                objectData = initialObjectsData,
                saveStateBetweenStages = config.saveStateBetweenStages,
                stages = initialStageList,
                traits = initialTraitList
            };
            _initialConfigs.Add(initialConfig);
        }
    }
    
    [Button]
    private void LoadObjects()
    {
        Debug.LogWarning("Cut debug loader");
        var guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;
            if (!prefab.TryGetComponent<StageObject>(out var stageObject)) continue;
            var objectData = new ObjectData { prefab = stageObject, scale = prefab.transform.localScale };
            var config = new ObjectConfig { objectData = new List<ObjectData> {objectData}};
            if (ConfigExists(config)) continue;
            config.name = prefab.name + "_CO";
            configs.Add(config);
        }
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    private bool ConfigExists(ObjectConfig config)
    {
        if (configs.Count == 0) return false;
        foreach (var objectConfig in configs)
        {
            if (objectConfig.objectData.Count == 0) continue;
            if (objectConfig.objectData[0].prefab == config.objectData[0].prefab) return true;
        }
        return false;
    }

    [Button]
    private void SpawnObjects() { foreach (var obj in configs) { obj.Spawn(); } }
    
    [Button]
    private void DespawnObjects() { foreach (var obj in configs) { obj.Despawn(); } }

    [Button]
    private void SaveObjectsStates()
    {
        foreach (var obj in configs) { obj.SaveState(); }
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    } 
    
    public ObjectConfig[] GetConfigs() { return configs.ToArray(); }
    
    public ObjectConfig[] GetConfigsByStage(Stage stage) => configs.Where(c => c.stages.Contains(stage.StageNum)).ToArray();
    
    public bool TryGetObjectConfigByName(string objectName, out ObjectConfig config)
    {
        config = null;
        foreach (var conf in configs)
        {
            config = conf.GetConfigByName(objectName);
            if (config == null) continue;
            return true;
        }
        return false;
    }

    public void Stop() { configs.Clear(); foreach (var config in _initialConfigs) configs.Add(config); }
}