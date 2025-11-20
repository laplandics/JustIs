using UnityEngine;

public abstract class Stage : MonoBehaviour
{
    [SerializeField] private StageNum stageNum;
    
    public virtual void StartStage()
    {
        G.GetService<SpecialGameStatesService>().GetState<CurrentGameStage>().Set(stageNum);
        SpawnStage();
    }

    public virtual void EndStage()
    {
        DespawnStage();
    }
    
    private static void SpawnStage()
    {
        
    }

    private static void DespawnStage()
    {
        
    }
}

public enum StageNum
{
    None,
    StageOne,
    StageTwo,
    StageThree,
    StageFour,
}