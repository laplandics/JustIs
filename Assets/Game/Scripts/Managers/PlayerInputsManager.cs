using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsManager : MonoBehaviour, ISceneManager
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private LayerMask ignoredLayers;
    [SerializeField] private PlayerSettings playerSettings;
    private Player _player;
    private FirstPersonController _fpController;
    private AimRaycaster _aimRaycaster;
    private bool _isPlayerActive;

    public void Initialize()
    {
        EventService.Subscribe<ConfigEvents.Player_SpawnedEvent>(AssignPlayer);
        EventService.Subscribe<ConfigEvents.Player_DespawnedEvent>(ClearPlayer);
    }

    private void AssignPlayer(ConfigEvents.Player_SpawnedEvent eventData)
    {
        _player = eventData.Player;
        AssignPlayerHelpers();
        _isPlayerActive = true;
    }

    private void AssignPlayerHelpers()
    {
        _fpController = new FirstPersonController(_player.Controller, playerSettings);
        _aimRaycaster = new AimRaycaster();
    }

    private void ClearPlayer(ConfigEvents.Player_DespawnedEvent _) { UnsubscribeCamera(); Deinitialize(); }
    
    private void UnsubscribeCamera()
    {
        var cameraManager = G.GetManager<CameraManager>();
        if (cameraManager != null) _player?.FreeFpCamera(cameraManager);
    }
    
    private void OnMove(InputValue value)
    {
        if (!_isPlayerActive) return;
        _fpController.SetMoveInput = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        if (!_isPlayerActive) return;
        _fpController.SetLookInput = value.Get<Vector2>();
    }

    private void OnInteract(InputValue value)
    {
        if (!_isPlayerActive) return;
        _aimRaycaster.Interactable?.Interact(value.isPressed);
    }
    
    //Cut
    private void OnEndInteraction(InputValue value)
    {
        Debug.LogWarning("Redo interaction end");
        if (!_isPlayerActive) return;
        EventService.Invoke(new UiEvents.OnUiInteractionEnded());
    }

    private void OnLock(InputValue value)
    {
        G.GetService<InputService>().ChangeInputs(InputsType.Interaction);
        Debug.LogWarning("Cut OnLock input");
    }
    //
    
    public Player GetPlayer() => _player;
    public LayerMask GetIgnoredLayers() => ignoredLayers;
    public PlayerInput GetPlayerInput() => playerInput;
    
    public void Deinitialize()
    {
        _fpController?.Dispose();
        _aimRaycaster?.Dispose();
        _fpController = null;
        _aimRaycaster = null;
        _player = null;
        _isPlayerActive = false;
    }
}