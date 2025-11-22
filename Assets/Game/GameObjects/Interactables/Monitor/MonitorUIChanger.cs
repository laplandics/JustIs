using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonitorUIChanger
{
    private readonly Monitor _monitor;
    private readonly Canvas _monitorCanvas;
    private readonly Image _uiPanel;
    private readonly TMP_Text _monitorText;
    private readonly Sprite _redMonitor;
    private readonly Sprite _blackMonitor;

    public MonitorUIChanger(Monitor monitor)
    {
        _monitor = monitor;
        _monitorCanvas = monitor.monitorCanvas;
        _uiPanel = monitor.uiPanel;
        _monitorText = monitor.monitorText;
        _redMonitor = monitor.redMonitor;
        _blackMonitor = monitor.blackMonitor;
    }
    
    public void SetMonitorUi(OnMonitorUiChangedEvent eventData)
    {
        if (eventData.SpecificTarget != null && eventData.SpecificTarget != _monitor) return;
        if (_monitor.IsMonitorBroken) return;
        switch (eventData.Command)
        {
            case MonitorUiType.Error:
                ChangeMonitor(Color.white, _redMonitor, "ERR");
                break;
            case MonitorUiType.ShouldReadTablet:
                ChangeMonitor(Color.red, _blackMonitor, "READ THE RULES");
                break;
            case MonitorUiType.ShouldGrabRevolver:
                ChangeMonitor(Color.red, _blackMonitor, "GRAB THE GUN");
                break;
            case MonitorUiType.ShouldShootPerson:
                ChangeMonitor(Color.red, _blackMonitor, "SHOOT HIM");
                break;
            case MonitorUiType.GoodJob:
                ChangeMonitor(Color.green, _blackMonitor, ":)");
                break;
            case MonitorUiType.ShouldNotShootMonitor:
                ChangeMonitor(Color.white, _redMonitor, "DON'T!");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ChangeMonitor(Color textColor, Sprite sprite = null, string text = null)
    {
        if (sprite != null) { _uiPanel.sprite = sprite; }

        if (text == null) return;
        _monitorText.text = text;
        _monitorText.color = textColor;
    }
}

public enum MonitorUiType
{
    Error,
    ShouldReadTablet,
    ShouldGrabRevolver,
    ShouldShootPerson,
    ShouldNotShootMonitor,
    GoodJob
}