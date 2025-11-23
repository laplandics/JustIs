using UnityEngine;

[CreateAssetMenu(fileName = "IsGameStarted", menuName = "SpecialGameStates/IsGameStarted")]
public class IsGameStarted : SpecialGameState<bool>
{
    public override void Set(bool status)
    {
        value = status;
        if (status) EventService.Invoke(new OnGameStarted());
        else EventService.Invoke(new OnGameEnded());
        base.Set(status);
    }

    public override bool Get() { return value; }
}