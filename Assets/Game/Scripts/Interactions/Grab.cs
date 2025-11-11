using UnityEngine;

public class Grab : MonoBehaviour, IInteractionPreset
{
    [SerializeField] private float distance; 
    [SerializeField] private Sprite targetSprite;
    private Transform _target;
    
    public Sprite TargetSprite => targetSprite;
    
    public bool IsRelevant()
    {
        _target = G.GetManager<PlayerManager>().GetPlayer().Hand;
        var cameraTr = G.GetManager<CameraManager>().GetCameraTransform();
        if (_target.transform.childCount > 0) return false;
        var isInDistance = !(Vector3.Distance(cameraTr.position, transform.position) > distance);
        if (isInDistance) UpdateUI(true);
        return isInDistance;
    }

    public void PerformInteraction()
    {
        var iGrabable = GetComponent<InteractableObject>() as IGrabable;
        iGrabable?.Grab(_target);
    }

    public void UpdateUI(bool showUI) => EventService.Invoke(new OnUpdateUIEvent {Sprite = showUI ? TargetSprite : null});

    public void Reset()
    {
        UpdateUI(false);
        _target = null;
    }
}