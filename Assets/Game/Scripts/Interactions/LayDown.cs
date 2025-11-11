using UnityEngine;

public class LayDown : MonoBehaviour, IInteractionPreset
{
    [SerializeField] private float distance; 
    [SerializeField] private Sprite targetSprite;
    private IGrabable _layingObject;
    private RaycastHit _hit;

    public Sprite TargetSprite => targetSprite;

    public bool IsRelevant()
    {
        if (!TryGetRaycastHit()) return false;
        var player = G.GetManager<PlayerManager>().GetPlayer();
        if (player.Hand.childCount == 0) return false;
        _layingObject = player.Hand.GetChild(0)?.GetComponent<InteractableObject>() as IGrabable;
        if (_layingObject == null) return false;
        UpdateUI(true);
        return true;
    }

    public void PerformInteraction()
    {
        var target = _hit.point + new Vector3(0, 0.1f, 0);
        _layingObject.Release(null, target);
    }

    private bool TryGetRaycastHit()
    {
        var playerManager = G.GetManager<PlayerManager>();
        var cameraTransform = G.GetManager<CameraManager>().GetCameraTransform();
        var ray = new Ray(cameraTransform.position, cameraTransform.forward);
        var layerMask = ~playerManager.GetIgnoredLayers();
        var canLayDown = Physics.Raycast(ray.origin, ray.direction, out _hit, distance, layerMask);
        return canLayDown;
    }
    
    public void UpdateUI(bool showUI) => EventService.Invoke(new OnUpdateUIEvent {Sprite = showUI ? TargetSprite : null});

    public void Reset()
    {
        UpdateUI(false);
    }
}