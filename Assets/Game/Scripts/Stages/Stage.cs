using UnityEngine;

public abstract class Stage : MonoBehaviour
{
    [SerializeField] private StageNum stageNum;
    [SerializeField] private ObjectConfig[] stageObjects;
    
    public StageNum StageNum => stageNum;
    public ObjectConfig[] StageObjects => stageObjects;
    
    public virtual void StartStage()
    {
        DataInjector.InjectState<CurrentGameStage>().Set(this);
        stageObjects = G.GetService<ObjectDataService>().GetConfigsByStage(this);
        foreach (var stageObject in stageObjects) { stageObject.Spawn(); }
        EventService.Invoke(new OnStageLoadEvent());
    }

    public virtual void EndStage()
    {
        foreach (var stageObject in stageObjects) { stageObject.SaveState(); }
        foreach (var config in G.GetService<ObjectDataService>().GetConfigs()) { config.Despawn(); }
    }
}