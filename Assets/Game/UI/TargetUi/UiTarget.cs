using System;
using UnityEngine;
using UnityEngine.UI;

public class UiTarget : IDisposable
{
    private Image _targetPoint;
    private bool _hideTargetPoint;

    public UiTarget(Image targetPoint)
    {
        _targetPoint = targetPoint;
        EventService.Subscribe<UiEvents.OnUiInteractionStarted>(HideTargetPoint);
        EventService.Subscribe<UiEvents.OnUiInteractionEnded>(ShowTargetPoint);
        EventService.Subscribe<UiEvents.OnUpdateUIEvent>(UpdateTargetPoint);
    }
    
    private void HideTargetPoint(UiEvents.OnUiInteractionStarted _) { ResetTargetPoint(); UiManager.ShowCursor(); _hideTargetPoint = true; }
    private void ResetTargetPoint() { _targetPoint.color = Color.clear; }
    private void ShowTargetPoint(UiEvents.OnUiInteractionEnded _) { UiManager.HideCursor(); _hideTargetPoint = false; }

    private void UpdateTargetPoint(UiEvents.OnUpdateUIEvent eventData)
    {
        if (_hideTargetPoint) return;
        if (!_targetPoint) return;
        if (eventData.Sprite == null) {ResetTargetPoint(); return;}
        _targetPoint.sprite = eventData.Sprite;
        _targetPoint.color = Color.white;
    }

    public void Dispose()
    {
        EventService.Unsubscribe<UiEvents.OnUiInteractionStarted>(HideTargetPoint);
        EventService.Unsubscribe<UiEvents.OnUiInteractionEnded>(ShowTargetPoint);
        EventService.Unsubscribe<UiEvents.OnUpdateUIEvent>(UpdateTargetPoint);
        _targetPoint = null;
    }
}