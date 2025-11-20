using UnityEngine;

[CreateAssetMenu(fileName = "CurrentGameStage", menuName = "SpecialGameStates/CurrentGameStage")]
public class CurrentGameStage : SpecialGameState<StageNum>
{
    public override void Set(StageNum stage)
    {
        value = stage;
        EventService.Invoke(new OnStageStartEvent {StartedStage = stage});
    }

    public override StageNum Get() { return value; }
}