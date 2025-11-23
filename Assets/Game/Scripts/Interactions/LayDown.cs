using UnityEngine;

public class LayDown : BaseInteraction
{
    private IGrabable _layingObject;
    private RaycastHit _hit;

    public override bool IsRelevant(Collider colliderInfo)
    {
        if (!TryGetRaycastHit()) return false;
        var player = G.GetManager<PlayerInputsManager>().GetPlayer();
        if (player.Hand.childCount == 0) return false;
        _layingObject = player.Hand.GetChild(0)?.GetComponent<InteractableObject>() as IGrabable;
        if (_layingObject == null) return false;
        if (InteractionCollider != colliderInfo) return false;
        UpdateUI(true);
        return true;
    }

    public override void PerformInteraction()
    {
        var target = _hit.point + new Vector3(0, 0.2f, 0);
        _layingObject.Release(null, target);
    }
    
    public override void CancelInteraction() {}

    private bool TryGetRaycastHit()
    {
        var playerManager = G.GetManager<PlayerInputsManager>();
        var cameraTransform = G.GetManager<CameraManager>().GetCameraTransform();
        var ray = new Ray(cameraTransform.position, cameraTransform.forward);
        var layerMask = ~playerManager.GetIgnoredLayers();
        var canLayDown = Physics.Raycast(ray.origin, ray.direction, out _hit, Distance, layerMask);
        return canLayDown;
    }
}