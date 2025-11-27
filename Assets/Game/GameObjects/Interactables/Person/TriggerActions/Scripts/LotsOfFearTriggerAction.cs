using UnityEngine;

[CreateAssetMenu(fileName = "LotsOfFear", menuName = "GameData/TriggerActions/LotsOfFear")]
public class LotsOfFearTriggerAction : TriggerAction
{
    [SerializeField] private float triggerTrustAmount;
    
    public override void PrepareToPerform()
    {
        var mood = DataInjector.InjectState<CurrentPersonMood>()?.Get<float>();
        ReadyToPerform = mood <= triggerTrustAmount;
        EventService.Unsubscribe<ConfigEvents.Person_TalkEvent>(Perform);
        EventService.Subscribe<ConfigEvents.Person_TalkEvent>(Perform);
    }

    private void Perform(ConfigEvents.Person_TalkEvent eventData)
    {
        if (!ReadyToPerform || eventData.Person != targetPerson) return;
        Debug.Log("Person is too terrified to talk");
    }
}