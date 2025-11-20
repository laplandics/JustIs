using UnityEngine;

[CreateAssetMenu(fileName = "IsGameStarted", menuName = "SpecialGameStates/IsGameStarted")]
public class IsGameStarted : SpecialGameState<bool>
{
    public override void Set(bool status)
    {
        value = status;
        if(status) EventService.Invoke(new OnGameStarted());
    }

    public override bool Get() { return value; }
}