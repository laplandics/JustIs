using UnityEngine;

[CreateAssetMenu(fileName = "CurrentGameStage", menuName = "SpecialGameStates/CurrentGameStage")]
public class CurrentGameStage : SpecialGameState<Stage>
{
    public override void Set(Stage stage) { value = stage; base.Set(stage); }
    public override Stage Get() { return value; }
}