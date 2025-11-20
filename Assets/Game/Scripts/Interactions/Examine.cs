using UnityEngine;

public class Examine : BaseInteraction, ICameraLocker, IPlayerLocker
{
    [SerializeField] private Transform cameraPoint;
    private Vector3 _lastCameraLocalPosition;
    
    public CameraConfigurationPreset CameraConfigPreset => GetCameraConfiguration();
    
    public override bool IsRelevant(Collider colliderInfo)
    {
        if (base.IsRelevant(colliderInfo)) {UpdateUI(true); return true;}
        return false;
    }

    public override void PerformInteraction()
    {
        EventService.Invoke(new OnInteractionEventStarted {Preset = this});
        G.GetManager<PlayerManager>().GetPlayer().Hand.gameObject.SetActive(false);
        GetComponent<IExaminable>()?.Examine();
        EventService.Subscribe<OnInteractionEventEnded>(EndExamineInteraction);
    }

    public override void CancelInteraction() {}

    private void EndExamineInteraction(OnInteractionEventEnded _)
    {
        EventService.Unsubscribe<OnInteractionEventEnded>(EndExamineInteraction);
        GetComponent<IExaminable>()?.Release();
        G.GetManager<PlayerManager>().GetPlayer().Hand.gameObject.SetActive(true);
    }

    private CameraConfigurationPreset GetCameraConfiguration()
    {
        var preset = new CameraConfigurationPreset();
        preset.IsFree = false;
        preset.CameraParent = cameraPoint;
        preset.CameraLocalPosition = Vector3.zero;
        preset.CameraLocalRotation = Vector3.forward;
        return preset;
    }
}