using UnityEngine;

[CreateAssetMenu(fileName = "IsRevolverInHands", menuName = "SpecialGameStates/IsRevolverInHands")]
public class IsRevolverInHands : SpecialGameState<bool>
{
    public override void Set(bool status)
    {
        value = status;
        EventService.Invoke(new OnRevolverGrabStatusChanged {IsGrabbed = status});
        base.Set(status);
    }

    public override bool Get() { return value; }
}