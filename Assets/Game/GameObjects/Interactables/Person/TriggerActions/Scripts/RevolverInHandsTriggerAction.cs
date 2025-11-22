using UnityEngine;

[CreateAssetMenu(fileName = "RevolverInHands", menuName = "GameData/TriggerActions/RevolverInHands")]
public class RevolverInHandsTriggerAction : TriggerAction
{
    public override void PrepareToPerform()
    {
        ReadyToPerform = G.GetService<SpecialGameStatesService>().GetState<IsRevolverInHands>().Get();
        EventService.Unsubscribe<OnPlayerTalkToPersonEvent>(Perform);
        EventService.Subscribe<OnPlayerTalkToPersonEvent>(Perform);
    }

    private void Perform(OnPlayerTalkToPersonEvent eventData)
    {
        if (!ReadyToPerform || eventData.Target != targetPerson) return;
        Debug.Log("Player is holding the revolver and talk to person");
    }
}