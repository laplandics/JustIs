using UnityEngine;

[CreateAssetMenu(fileName = "IsAncestorsStatueExamined", menuName = "SpecialGameStates/IsAncestorsStatueExamined")]
public class IsAncestorsStatueExamined : SpecialGameState<bool>
{
    private bool _isExamined;
    
    public override void Set(bool newValue)
    {
        value = newValue;
        if (!_isExamined) EventService.Invoke(new ConfigEvents.AncestorsStatue_ExaminedEvent());
        _isExamined = true;
        base.Set(newValue);
    }

    public override bool Get() => value;
}