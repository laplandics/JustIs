using UnityEngine;

[CreateAssetMenu(fileName = "[TypeName]_PT", menuName = "GameData/PersonType")]
public class PersonType : ScriptableObject
{
    public StageNum typeStage;
    public TriggerAction[] triggers;
    public float defaultTrust;
    public float defaultFear;
    
    private Person _person;
    private bool _someStateInvoked;

    public void Initialize(Person person)
    {
        _person = person;
        foreach (var trigger in triggers) { trigger.targetPerson = _person; }
        EventService.Subscribe<OnSomeStateChangedEvent>(CatchSomeStateInvocation);
    }

    private void CatchSomeStateInvocation(OnSomeStateChangedEvent eventData)
    {
        foreach (var trigger in triggers) { if (trigger.state == eventData.State) trigger.PrepareToPerform(); }
    }

    public void Deinitialize()
    {
        _person = null;
        foreach (var trigger in triggers) { trigger.Reset(); }
        EventService.Unsubscribe<OnSomeStateChangedEvent>(CatchSomeStateInvocation);
    }
}