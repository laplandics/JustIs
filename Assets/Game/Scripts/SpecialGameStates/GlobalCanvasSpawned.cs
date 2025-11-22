using UnityEngine;

[CreateAssetMenu(fileName = "GlobalCanvasSpawned", menuName = "SpecialGameStates/GlobalCanvasSpawned")]
public class GlobalCanvasSpawned : SpecialGameState<Canvas>
{
    public override void Set(Canvas canvas) { value = canvas; base.Set(canvas); }
    public override Canvas Get() { return value; }
}