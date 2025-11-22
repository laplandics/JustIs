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
    private CurrentMonitorUi _stateData;

    public bool IsMonitorBroken {get; private set;}
    
    protected override void Launch()
    {
        _uIChanger = new MonitorUIChanger(this);
        EventService.Subscribe<OnMonitorUiChangedEvent>(_uIChanger.SetMonitorUi);
        _stateData = G.GetService<SpecialGameStatesService>().GetState<CurrentMonitorUi>();
        _stateData.Set(MonitorUiType.Error);
    }

    public void TakeAim(float time, out bool isShot)
    {
        isShot = false;
        if (IsMonitorBroken) {isShot = true; return;}
        if (_firstShootAttempt) _savedUiType = _stateData.Get<MonitorUiType>();
        _firstShootAttempt = false;
        _stateData.Set(MonitorUiType.ShouldNotShootMonitor);
        if (!(time >= timeToShoot)) return;
        isShot = true;
        _stateData.Set(MonitorUiType.Error, this);
        IsMonitorBroken = true;
        _stateData.Set(MonitorUiType.GoodJob);
        EventService.Invoke(new OnMonitorShotEvent());
    }

    public void ReleaseAim()
    {
        _stateData.Set(_savedUiType);
        _firstShootAttempt = true;
    }

    private void OnDestroy()
    {
        EventService.Unsubscribe<OnMonitorUiChangedEvent>(_uIChanger.SetMonitorUi);
    }
}
