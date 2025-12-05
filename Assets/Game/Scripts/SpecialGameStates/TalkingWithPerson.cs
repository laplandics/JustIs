using UnityEngine;

[CreateAssetMenu(fileName = "TalkingWithPerson", menuName = "SpecialGameStates/TalkingWithPerson")]
public class TalkingWithPerson : SpecialGameState<bool, Person>
{
    public override void Set(bool newValue1, Person newValue2 = null)
    {
        value1 = newValue1;
        value2 = newValue2;
        if (newValue1) EventService.Invoke(new ConfigEvents.Person_TalkEvent {Person = newValue2});
        base.Set(newValue1, newValue2);
    }
}