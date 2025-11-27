using UnityEngine;

[CreateAssetMenu(fileName = "IsGameStarted", menuName = "SpecialGameStates/IsGameStarted")]
public class IsGameStarted : SpecialGameState<bool>
{
    public override void Set(bool status)
    {
        value = status;
        if (status) EventService.Invoke(new GameEvents.OnGameStarted());
        else EventService.Invoke(new GameEvents.OnGameEnded());
        base.Set(status);
    }

    public override bool Get() { return value; }
}