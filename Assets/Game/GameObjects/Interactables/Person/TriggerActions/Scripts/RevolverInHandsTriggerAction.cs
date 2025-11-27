using UnityEngine;

[CreateAssetMenu(fileName = "RevolverInHands", menuName = "GameData/TriggerActions/RevolverInHands")]
public class RevolverInHandsTriggerAction : TriggerAction
{
    public override void PrepareToPerform()
    {
        ReadyToPerform = DataInjector.InjectState<IsRevolverInHands>().Get();
        EventService.Unsubscribe<ConfigEvents.Person_TalkEvent>(Perform);
        EventService.Subscribe<ConfigEvents.Person_TalkEvent>(Perform);
    }

    private void Perform(ConfigEvents.Person_TalkEvent eventData)
    {
        if (!ReadyToPerform || eventData.Person != targetPerson) return;
        Debug.Log("Player is holding the revolver and talk to person");
    }
}