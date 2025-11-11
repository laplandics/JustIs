using Unity.Cinemachine;
using UnityEngine;

public class Examine : MonoBehaviour, IInteractionPreset, ICameraLocker, IPlayerLocker
{
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private float distance;
    [SerializeField] private Sprite targetSprite;
    private CinemachineCamera _cmCamera;
    private Vector3 _lastCameraLocalPosition;
    
    public Sprite TargetSprite => targetSprite;
    public CameraConfigurationPreset CameraConfigPreset => GetCameraConfiguration();
    
    public bool IsRelevant()
    {
        _cmCamera = G.GetManager<CameraManager>().GetCamera();
        var isInDistance = !(Vector3.Distance(_cmCamera.transform.position, transform.position) > distance);
        if (isInDistance) UpdateUI(true);
        return isInDistance;
    }

    public void PerformInteraction()
    {
        EventService.Invoke(new OnInteractionEventStarted {Preset = this});
        G.GetManager<PlayerManager>().GetPlayer().Hand.gameObject.SetActive(false);
        GetComponent<IExaminable>()?.Examine();
        EventService.Subscribe<OnInteractionEventEnded>(EndExamineInteraction);
    }

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
    
    public void UpdateUI(bool showUI) => EventService.Invoke(new OnUpdateUIEvent {Sprite = showUI ? TargetSprite : null});
    
    public void Reset()
    {
        UpdateUI(false);
        _cmCamera = null;
    }
}