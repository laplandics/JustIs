using UnityEngine;

[CreateAssetMenu(fileName = "LotsOfFear", menuName = "GameData/TriggerActions/LotsOfFear")]
public class LotsOfFearTriggerAction : TriggerAction
{
    [SerializeField] private float triggerTrustAmount;
    
    public override void PrepareToPerform()
    {
        var mood = DataInjector.InjectState<CurrentPersonMood>().Get<float>();
        ReadyToPerform = mood <= triggerTrustAmount;
        EventService.Unsubscribe<OnPlayerTalkToPersonEvent>(Perform);
        EventService.Subscribe<OnPlayerTalkToPersonEvent>(Perform);
    }

    private void Perform(OnPlayerTalkToPersonEvent eventData)
    {
        if (!ReadyToPerform || eventData.Person != targetPerson) return;
        Debug.Log("Person is too terrified to talk");
    }
}