using UnityEngine;

public abstract class BaseInteraction : MonoBehaviour, IInteractionPreset
{
    [SerializeField] private Sprite targetSprite;
    [SerializeField] private Collider interactionCollider;
    [SerializeField] private float distance; 
    
    public Sprite TargetSprite => targetSprite;
    public Collider InteractionCollider => interactionCollider;
    public float Distance => distance;
    
    public virtual bool IsRelevant(Collider colliderInfo)
    {
        var cameraTr = G.GetManager<CameraManager>().GetCameraTransform();
        var isInDistance = !(Vector3.Distance(cameraTr.position, transform.position) > Distance);
        if (!isInDistance) return false;
        if (colliderInfo != interactionCollider) return false;
        return true;
    }

    public virtual void PerformInteraction() {}

    public virtual void CancelInteraction() {}

    public virtual void UpdateUI(bool showUI) => EventService.Invoke(new UiEvents.OnUpdateUIEvent {Sprite = showUI ? TargetSprite : null});

    public virtual void Reset() => UpdateUI(false);
}