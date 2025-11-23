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
    
    [Header("IShootable settings")]
    [SerializeField] private float timeToShoot;

    private bool _firstShootAttempt = true;
    private MonitorUiType _savedUiType = MonitorUiType.Error;
    private MonitorUIChanger _uIChanger;

    public bool IsMonitorBroken {get; private set;}
    
    protected override void Launch()
    {
        _uIChanger = new MonitorUIChanger(this);
        EventService.Subscribe<OnMonitorUiChangedEvent>(_uIChanger.SetMonitorUi);
        DataInjector.InjectState<CurrentMonitorUi>().Set(MonitorUiType.Error);
    }

    public void TakeAim(float time, out bool isShot)
    {
        isShot = false;
        if (IsMonitorBroken) {isShot = true; return;}
        if (_firstShootAttempt) _savedUiType = DataInjector.InjectState<CurrentMonitorUi>().Get<MonitorUiType>();
        _firstShootAttempt = false;
        DataInjector.InjectState<CurrentMonitorUi>().Set(MonitorUiType.ShouldNotShootMonitor);
        if (!(time >= timeToShoot)) return;
        isShot = true;
        DataInjector.InjectState<CurrentMonitorUi>().Set(MonitorUiType.Error, this);
        IsMonitorBroken = true;
        DataInjector.InjectState<CurrentMonitorUi>().Set(MonitorUiType.GoodJob);
        EventService.Invoke(new OnMonitorShotEvent());
    }

    public void ReleaseAim()
    {
        DataInjector.InjectState<CurrentMonitorUi>().Set(_savedUiType);
        _firstShootAttempt = true;
    }

    public override void Disable()
    {
        DataInjector.InjectState<CurrentMonitorUi>().Set(MonitorUiType.Error);
        EventService.Unsubscribe<OnMonitorUiChangedEvent>(_uIChanger.SetMonitorUi);
        base.Disable();
    }
}
