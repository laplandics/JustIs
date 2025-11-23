using UnityEngine;

public class Grab : BaseInteraction
{
    private Transform _target;
    
    public override bool IsRelevant(Collider colliderInfo)
    {
        _target = G.GetManager<PlayerInputsManager>().GetPlayer().Hand;
        if (_target.transform.childCount > 0) return false;
        if (!base.IsRelevant(colliderInfo)) { return false; }
        UpdateUI(true);
        return true;
    }

    public override void PerformInteraction()
    {
        var iGrabable = GetComponent<InteractableObject>() as IGrabable;
        iGrabable?.Grab(_target);
    }

    public override void CancelInteraction() {}

    public override void Reset()
    {
        base.Reset();
        _target = null;
    }
}