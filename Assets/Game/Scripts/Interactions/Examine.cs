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
        var player = G.GetManager<PlayerInputsManager>().GetPlayer();
        player.Hand.gameObject.SetActive(false);
        player.SetVisual(false);
        GetComponent<IExaminable>()?.Examine();
        EventService.Subscribe<OnUiInteractionEnded>(EndExamineInteraction);
    }

    public override void CancelInteraction() {}

    private void EndExamineInteraction(OnUiInteractionEnded _)
    {
        EventService.Unsubscribe<OnUiInteractionEnded>(EndExamineInteraction);
        GetComponent<IExaminable>()?.Release();
        var player = G.GetManager<PlayerInputsManager>().GetPlayer();
        player.Hand.gameObject.SetActive(true);
        player.SetVisual(true);
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