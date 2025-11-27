using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "ObjectDataService", menuName = "Services/ObjectDataService")]
public class ConfigsService : ScriptableObject, IGameService
{
    [SerializeField] private AssetLabelReference label;
    [SerializeField] private List<string> savedPaths = new();
    [SerializeField] private List<ObjectConfig> configs = new();
    [SerializeField] private List<ObjectConfig> savedConfigs = new();
    
    private bool _assetsLoaded;
    
    public IEnumerator Run()
    {
        _assetsLoaded = false;
        Addressables.LoadAssetsAsync<ObjectConfig>(label, SaveConfig).Completed += _ => _assetsLoaded = true;
        yield return new WaitUntil(() => _assetsLoaded);
    }
    
    private void SaveConfig(ObjectConfig config)
    {
        configs.Add(config);
        var savedConfig = CreateInstance(typeof(ObjectConfig)) as ObjectConfig;
        if (savedConfig == null) return;
        savedConfig.CopyConfig(config);
        savedConfigs.Add(savedConfig);
    }

    [Button]
    private void CreateObjectConfigs()
    {
        Debug.LogWarning("Cut debug loader");
        var guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;
            if (!prefab.TryGetComponent<StageObject>(out var stageObject)) continue;
            var objectData = new ObjectData { scale = prefab.transform.localScale };
            var config = CreateInstance<ObjectConfig>();
            config.configPrefab = stageObject;
            config.objectData = new List<ObjectData> { objectData };
            config.configName = prefab.name + "_CO";
            var savePath = $"Assets/Game/Data/ObjectConfigs/{config.configName}.asset";
            if (savedPaths.Contains(savePath)) continue;
            AssetDatabase.CreateAsset(config, savePath);
            savedPaths.Add(savePath);
        }
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    public ObjectConfig GetConfigByName(string confName)
    {
        foreach (var config in configs) if (config.configName == confName) return config;
        return null;
    }

    public List<ObjectConfig> GetConfigsByStage(StageNum stage)
    {
        var configList = new List<ObjectConfig>(configs);
        foreach (var config in configs) { if (!config.stages.Contains(stage)) configList.Remove(config); }
        configList.Sort((a, b) => a.spawnOrder.CompareTo(b.spawnOrder));
        return configList;
    }

    public void Stop()
    {
        if (configs.Count != savedConfigs.Count) { Debug.LogWarning("Configs not saved. Saved configs count is not equal to configs count"); return; }
        for (var i = 0; i < configs.Count; i++) { if (configs[i].configName == savedConfigs[i].configName) configs[i].CopyConfig(savedConfigs[i]); }
        foreach (var config in configs) config.Despawn();
        savedConfigs.Clear();
        configs.Clear();
    }
}