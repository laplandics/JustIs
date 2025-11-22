using UnityEngine;

public class Examine : BaseInteraction, ICameraLocker, IInputsChanger
{
    [SerializeField] private Transform cameraPoint;
    private Vector3 _lastCameraLocalPosition;
    
    public CameraConfigurationPreset CameraConfigPreset => GetCameraConfiguration();
    public InputsType InputsType => InputsType.Interaction;
    
    public override bool IsRelevant(Collider colliderInfo)
    {
        if (base.IsRelevant(colliderInfo)) {UpdateUI(true); return true;}
        return false;
    }

    public override void PerformInteraction()
    {
        EventService.Invoke(new OnUiInteractionStarted {InputsType = InputsType, CameraConfigPreset = CameraConfigPreset});
        G.GetManager<PlayerManager>().GetPlayer().Hand.gameObject.SetActive(false);
        GetComponent<IExaminable>()?.Examine();
        EventService.Subscribe<OnUiInteractionEnded>(EndExamineInteraction);
    }

    public override void CancelInteraction() {}

    private void EndExamineInteraction(OnUiInteractionEnded _)
    {
        EventService.Unsubscribe<OnUiInteractionEnded>(EndExamineInteraction);
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