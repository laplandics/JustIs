using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour, ISceneManager
{
    [SerializeField] private UiSettings uiSettings;
    [SerializeField] private Transform canvasParent;
    private InteractableObject _interactable;
    private Canvas _canvas;
    private Image _targetPoint;
    private bool _hideTargetPoint;

    public void Initialize()
    {
        _canvas = SpawnManager.Spawn(uiSettings.canvas, Vector3.zero, Quaternion.identity, canvasParent);
        _targetPoint = _canvas.GetComponentInChildren<Image>();
        EventService.Subscribe<OnInteractionEventStarted>(HideTargetPoint);
        EventService.Subscribe<OnInteractionEventEnded>(ShowTargetPoint);
        EventService.Subscribe<OnUpdateUIEvent>(UpdateTargetPoint);
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    private void HideTargetPoint(OnInteractionEventStarted _) { ResetTargetPoint(); ShowCursor(); _hideTargetPoint = true; }
    private void ResetTargetPoint() { _targetPoint.color = Color.clear; }
    private void ShowTargetPoint(OnInteractionEventEnded _) { HideCursor(); _hideTargetPoint = false; }

    private void UpdateTargetPoint(OnUpdateUIEvent eventData)
    {
        if (_hideTargetPoint) return;
        if (!_targetPoint) return;
        if (eventData.Sprite == null) {ResetTargetPoint(); return;}
        _targetPoint.sprite = eventData.Sprite;
        _targetPoint.color = Color.white;
    }
    
    public void Deinitialize()
    {
        EventService.Unsubscribe<OnInteractionEventStarted>(HideTargetPoint);
        EventService.Unsubscribe<OnInteractionEventEnded>(ShowTargetPoint);
        _canvas = null;
        _targetPoint = null;
    }
}