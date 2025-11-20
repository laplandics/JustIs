using System.Collections;
using UnityEngine;

public class Shoot : BaseInteraction
{
    [SerializeField] private float minDistance;
    [SerializeField] private float timeToShoot;
    private Revolver _revolver;
    private bool _isInteractionCancelled = true;

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
        if (!gameObject.TryGetComponent<IShootable>(out var shootable)) return;
        _isInteractionCancelled = false;
        G.GetManager<RoutineManager>().StartRoutine(WaitForShoot(shootable));
    }

    private IEnumerator WaitForShoot(IShootable shootable)
    {
        var time = 0.0f;
        while (!_isInteractionCancelled)
        {
            time += Time.deltaTime;
            if(time >= timeToShoot) {shootable.GetShot(); CancelInteraction(); }
            yield return null;
        }
    }

    public override void CancelInteraction() { _isInteractionCancelled = true; }

    public override void Reset() { _revolver = null; UpdateUI(false); CancelInteraction(); }
}
