using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Monitor : InteractableObject, IShootable
{
    public Canvas monitorCanvas;
    public Image uiPanel;
    public TMP_Text monitorText;
    
    [Header("Sprite prefabs")]
    public Sprite redMonitor;
    public Sprite blackMonitor;
    
    private MonitorUIChanger _uIChanger;
    
    protected override void Launch()
    {
        _uIChanger = new MonitorUIChanger(this);
        EventService.Subscribe<OnMonitorUiChangedEvent>(_uIChanger.SetMonitorUi);
        _uIChanger.ChangeMonitor(Color.white, redMonitor, "ERR");
    }

    public void GetShot()
    {
        Debug.Log("Monitor was shot");
        EventService.Invoke(new OnMonitorShotEvent());
    }
}
