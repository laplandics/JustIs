using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonitorUIChanger
{
    private readonly Canvas _monitorCanvas;
    private readonly Image _uiPanel;
    private readonly TMP_Text _monitorText;
    private readonly Sprite _redMonitor;
    private readonly Sprite _blackMonitor;

    public MonitorUIChanger(Monitor monitor)
    {
        _monitorCanvas = monitor.monitorCanvas;
        _uiPanel = monitor.uiPanel;
        _monitorText = monitor.monitorText;
        _redMonitor = monitor.redMonitor;
        _blackMonitor = monitor.blackMonitor;
    }
    
    public void SetMonitorUi(OnMonitorUiChangedEvent eventData)
    {
        var eventType = eventData.Type;
        switch (eventType)
        {
            case OnMonitorUiChangedEvent.EventType.ShouldReadTablet:
                ChangeMonitor(Color.red, _blackMonitor, "READ THE RULES");
                break;
            case OnMonitorUiChangedEvent.EventType.ShouldGrabRevolver:
                ChangeMonitor(Color.red, _blackMonitor, "GRAB THE GUN");
                break;
            case OnMonitorUiChangedEvent.EventType.ShouldShootPerson:
                ChangeMonitor(Color.red, _blackMonitor, "SHOOT HIM");
                break;
            case OnMonitorUiChangedEvent.EventType.GoodJob:
                ChangeMonitor(Color.green, _blackMonitor, ":)");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ChangeMonitor(Color textColor, Sprite sprite = null, string text = null)
    {
        if (sprite != null) { _uiPanel.sprite = sprite; }

        if (text == null) return;
        _monitorText.text = text;
        _monitorText.color = textColor;
    }
}