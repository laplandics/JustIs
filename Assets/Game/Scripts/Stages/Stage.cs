using UnityEngine;

public abstract class Stage : MonoBehaviour
{
    [SerializeField] private StageNum stageNum;
    
    public virtual void StartStage()
    {
        EventService.Invoke(new OnStageStartEvent {StartedStage = stageNum});
        SpawnStage();
    }

    public virtual void EndStage()
    {
        DespawnStage();
    }
    
    private static void SpawnStage()
    {
        G.GetService<StageObjectsService>().LoadStageObjects();
    }

    private static void DespawnStage()
    {
        G.GetService<StageObjectsService>().UnloadStageObjects();
    }
}

public enum StageNum
{
    StageOne,
    StageTwo,
    StageThree,
    StageFour,
}