using UnityEngine;

public abstract class Stage : MonoBehaviour
{
    [SerializeField] private StageNum stageNum;
    [SerializeField] private ObjectConfig[] stageObjects;
    
    public StageNum StageNum => stageNum;
    public ObjectConfig[] StageObjects => stageObjects;
    
    public virtual void StartStage()
    {
        G.GetService<SpecialGameStatesService>().GetState<CurrentGameStage>().Set(this);
        SpawnStage();
    }

    public virtual void EndStage()
    {
        DespawnStage();
    }
    
    private void SpawnStage()
    {
        stageObjects = G.GetService<ObjectDataService>().GetConfigsByStage(this);
        foreach (var stageObject in stageObjects) { stageObject.Spawn(); }
        EventService.Invoke(new OnStageLoadEvent());
    }

    private void DespawnStage()
    {
        foreach (var stageObject in stageObjects) { stageObject.SaveState(); }
        foreach (var config in G.GetService<ObjectDataService>().GetConfigs()) { config.Despawn(); }
    }

    private void OnDestroy()
    {
        foreach (var stageObject in stageObjects) { stageObject.GetInstances().Clear(); }
    }
}