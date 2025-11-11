using UnityEngine;

public class Shoot : MonoBehaviour, IInteractionPreset
{
    [SerializeField] private Sprite targetSprite;
    [SerializeField] private float minDistance;
    private Revolver _revolver;
    
    public Sprite TargetSprite => targetSprite;

    public bool IsRelevant()
    {
        var hand = G.GetManager<PlayerManager>().GetPlayer().Hand;
        if (hand.childCount == 0) return false;
        _revolver = hand.GetChild(0).gameObject.GetComponent<Revolver>();
        if (_revolver == null) return false;
        var playerCam = G.GetManager<CameraManager>().GetCameraTransform();
        var isInDistance = Vector3.Distance(transform.position, playerCam.position) > minDistance;
        if (isInDistance) UpdateUI(true);
        return isInDistance;
    }

    public void PerformInteraction()
    {
        if (!gameObject.TryGetComponent<IShootable>(out var shootable)) return;
        shootable.GetShot();
    }

    public void UpdateUI(bool showUI) => EventService.Invoke(new OnUpdateUIEvent {Sprite = showUI ? TargetSprite : null});
    
    public void Reset() { _revolver = null; UpdateUI(false); }
}
