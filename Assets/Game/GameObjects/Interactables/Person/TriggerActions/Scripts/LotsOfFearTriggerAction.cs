using UnityEngine;

[CreateAssetMenu(fileName = "LotsOfFear", menuName = "GameData/TriggerActions/LotsOfFear")]
public class LotsOfFearTriggerAction : TriggerAction
{
    [SerializeField] private float maxFear;
    
    public override void PrepareToPerform()
    {
        var trustFear = G.GetService<SpecialGameStatesService>().GetState<CurrentPersonTrustFearAmount>().Get<(float, float)>();
        ReadyToPerform = trustFear.Item1 >= maxFear;
        EventService.Unsubscribe<OnPlayerTalkToPersonEvent>(Perform);
        EventService.Subscribe<OnPlayerTalkToPersonEvent>(Perform);
    }

    private void Perform(OnPlayerTalkToPersonEvent eventData)
    {
        if (!ReadyToPerform || eventData.Target != targetPerson) return;
        Debug.Log("Слишком боится");
    }
}