using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour, ISceneManager
{
    [SerializeField] private CinemachineCamera cmCamera;
    [SerializeField] private Transform camTr;
    private bool _isCameraLocked;

    public void Initialize()
    {
        EventService.Subscribe<OnPlayerSpawnedEvent>(AssignCameraToPlayer);
        EventService.Subscribe<OnInteractionEventStarted>(LockCameraToObject);
        EventService.Subscribe<OnInteractionEventEnded>(ReturnCameraToPlayer);
    }

    private void AssignCameraToPlayer(OnPlayerSpawnedEvent eventData) { AssignCameraToPlayer(); }
    public Transform GetCameraTransform() => camTr;
    public CinemachineCamera GetCamera() => cmCamera;
    private void AssignCameraToPlayer() { G.GetManager<PlayerManager>().GetPlayer().SetFpCamera(camTr, this); }
    public void RotateCamera(Vector3 rotation)
    {
        if (_isCameraLocked) return;
        camTr.localRotation = Quaternion.Euler(rotation);
    }
    public void MoveCamera(Vector3 position) => camTr.localPosition = position; 
    public void AssignCamera(Transform parent) => camTr.SetParent(parent);
    
    private void ConfigureCamera(CameraConfigurationPreset config)
    {
        if (config.IsFree) {AssignCameraToPlayer(); _isCameraLocked = false; return;}
        AssignCamera(config.CameraParent);
        MoveCamera(config.CameraLocalPosition);
        RotateCamera(config.CameraLocalRotation);
        _isCameraLocked = true;
        camTr.localScale = Vector3.one;
    }

    private void LockCameraToObject(OnInteractionEventStarted eventData)
    {
        if (eventData.Preset is not ICameraLocker locker) return;
        ConfigureCamera(locker.CameraConfigPreset);
    }

    private void ReturnCameraToPlayer(OnInteractionEventEnded _)
    {
        ConfigureCamera(new CameraConfigurationPreset {IsFree = true});
    }

    public void Deinitialize()
    {
        EventService.Unsubscribe<OnPlayerSpawnedEvent>(AssignCameraToPlayer);
        EventService.Unsubscribe<OnInteractionEventStarted>(LockCameraToObject);
        EventService.Unsubscribe<OnInteractionEventEnded>(ReturnCameraToPlayer);
        _isCameraLocked = false;
    }
}

public class CameraConfigurationPreset
{
    public bool IsFree;
    public Transform CameraParent;
    public Vector3 CameraLocalPosition;
    public Vector3 CameraLocalRotation;
}