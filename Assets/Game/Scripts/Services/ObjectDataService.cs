using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectDataService", menuName = "Services/ObjectDataService")]
public class ObjectDataService : ScriptableObject, IGameService
{
    [SerializeField] private List<ObjectConfig> configs;
    
    public void Run() {}

    // Debug Functions
    
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
    //

    public bool TryGetObjectConfigByType<T>(out ObjectConfig config) where T : MonoBehaviour
    {
        config = null;
        foreach (var conf in configs)
        {
            config = conf.GetConfigByType<T>();
            if (config == null) continue;
            return true;
        }
        return false;
    }

    public bool TryGetObjectConfigsByType<T>(out ObjectConfig[] configurations) where T : MonoBehaviour
    {
        var configList = new List<ObjectConfig>();
        foreach (var config in configs)
        {
            if (config.GetConfigByType<T>() == null) continue;
            configList.Add(config);
        }
        if (configList.Count == 0) {configurations = null; return false;}
        configurations = configList.ToArray();
        return true;
    }

    public bool TryGetObjectConfigsByTrait(ObjectTrait trait, out ObjectConfig[] configurations)
    {
        var configList = new List<ObjectConfig>();
        foreach (var config in configs)
        {
            if (config.GetConfigByTrait(trait) == null) continue;
            configList.Add(config);
        }
        if (configList.Count == 0) {configurations = null; return false;}
        configurations = configList.ToArray();
        return true;
    }

    public void Stop()
    {
        
    }
}