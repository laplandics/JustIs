using System.Collections;
using UnityEngine;

public class Shoot : BaseInteraction
{
    [SerializeField] private float minDistance;
    private Revolver _revolver;
    private bool _isInteractionCancelled = true;
    private IShootable _shootable;

    public override bool IsRelevant(Collider colliderInfo)
    {
        var hand = G.GetManager<PlayerManager>().GetPlayer().Hand;
        if (hand.childCount == 0) return false;
        
        _revolver = hand.GetChild(0).gameObject.GetComponent<Revolver>();
        if (_revolver == null) return false;
        
        var playerCam = G.GetManager<CameraManager>().GetCameraTransform();
        var isInDistance = Vector3.Distance(transform.position, playerCam.position) > minDistance;
        if (!isInDistance) return false;
        
        if (InteractionCollider != colliderInfo) return false;
        
        UpdateUI(true);
        return true;
    }

    public override void PerformInteraction()
    {
        if (!gameObject.TryGetComponent(out _shootable)) return;
        _isInteractionCancelled = false;
        G.GetManager<RoutineManager>().StartRoutine(WaitForShoot());
    }

    private IEnumerator WaitForShoot()
    {
        var time = 0.0f;
        while (!_isInteractionCancelled)
        {
            time += Time.deltaTime;
            _shootable.TakeAim(time, out var isShot);
            if (isShot) CancelInteraction();
            yield return null;
        }
        _shootable.ReleaseAim();
        _shootable = null;
    }

    public override void CancelInteraction() { _isInteractionCancelled = true; }

    public override void Reset()
    {
        CancelInteraction();
        UpdateUI(false);
        _revolver = null;
    }
}
