using System.Collections.Generic;
using UnityEngine;

public abstract class Stage : MonoBehaviour
{
    [SerializeField] private StageNum stageNum;
    [SerializeField] private bool spawnPlayer = true;
    [SerializeField] private List<ObjectConfig> stageConfigs;
    
    public StageNum StageNum => stageNum;
    public HashSet<ObjectConfig> StageConfigs { get; } = new();

    public virtual void StartStage()
    {
        DataInjector.InjectState<CurrentGameStage>().Set(this);
        var configs = G.GetService<ConfigsService>().GetConfigsByStage(StageNum);
        foreach (var config in configs) { config.Spawn(); }
        if(spawnPlayer) SpawnPlayer();
        stageConfigs = new List<ObjectConfig>(StageConfigs);
        EventService.Invoke(new StageEvents.OnStageLoadEvent());
    }

    private void SpawnPlayer()
    {
        var playerConfig = G.GetService<ConfigsService>().GetConfigByName(nameof(Player)+"_CO");
        playerConfig.Spawn();
    }

    public virtual void EndStage()
    {
        foreach (var config in StageConfigs) { config.SaveState(); config.Despawn(); }
        StageConfigs.Clear();
        stageConfigs = null;
    }
}