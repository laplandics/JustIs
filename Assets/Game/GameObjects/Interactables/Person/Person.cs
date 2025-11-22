using System.Collections.Generic;
using UnityEngine;

public class Person : InteractableObject, IShootable, IExaminable
{
    [Header("Essentials")]
    [SerializeField] private PersonType[] personTypes;
    [SerializeField] private Rigidbody keyRigidBody;
    [SerializeField] private Transform bodyKey;
    
    [Header("IExaminable settings")]
    [SerializeField] private Canvas examineUi;
    [SerializeField] private Transform visual;
    
    [Header("IShootable settings")]
    [SerializeField] private float timeToShoot;
    
    [Header("Ui settings")]
    [SerializeField] private PersonUIHandler uiHandler;
    
    [Header("Trust/Fear settings")]
    [SerializeField] private float fromSourceMaxAmount;
    
    private readonly Dictionary<FearSource, float> _fearSources = new();
    private float _currentFear;
    private readonly Dictionary<TrustSource, float> _trustSources = new();
    private float _currentTrust;
    private OnPersonMoreFearEvent _internalFearEvent = new();
    private PersonType _currentPersonType;
    private bool _isShot;
    
    public Canvas ExamineUi => examineUi;
    public Transform Visual => visual;
    
    protected override void Launch()
    {
        if (!Application.isPlaying) return;
        G.GetManager<RoutineManager>().StartUpdateAction(UpdateColliderRotation);
        EventService.Subscribe<OnPersonMoreFearEvent>(AddFear);
        EventService.Subscribe<OnPersonMoreTrustEvent>(AddTrust);
        ChangePersonType();
        uiHandler.InitializeUi(_currentPersonType.defaultTrust, _currentPersonType.defaultFear);
    }

    private void UpdateColliderRotation() { transform.rotation = bodyKey.rotation; }

    private void ChangePersonType()
    {
        var currentState = G.GetService<SpecialGameStatesService>().GetState<CurrentGameStage>().Get().StageNum;
        foreach (var personType in personTypes)
        {
            if (personType.typeStage != currentState) continue;
            personType.Initialize(this);
            _currentPersonType = personType;
            break;
        }
        _currentFear = _currentPersonType.defaultFear;
        _currentTrust = _currentPersonType.defaultTrust;
    }

    public void TakeAim(float time, out bool isShot)
    {
        isShot = false;
        if (_isShot) {isShot = true; return;}

        _internalFearEvent.Target = this;
        _internalFearEvent.FearAmount = time * 0.005f;
        _internalFearEvent.Source = FearSource.HoldFire;
        EventService.Invoke(_internalFearEvent);
        
        if (!(time >= timeToShoot)) return;
        isShot = true;
        GetShot();
    }

    public void ReleaseAim()
    {
        
    }

    private void GetShot()
    {
        _isShot = true;
        var rawVector = G.GetManager<CameraManager>().GetCameraTransform().position - bodyKey.position;
        var vector = new Vector3(-rawVector.x, 3f, -rawVector.z);
        keyRigidBody.AddForce(vector * 100f, ForceMode.Impulse);
        EventService.Invoke(new OnPersonShotEvent {Person = this});
    }

    public void Examine()
    {
        examineUi.gameObject.SetActive(true);
        EventService.Invoke(new OnPlayerTalkToPersonEvent {Target = this});
    }

    public void Release()
    {
        examineUi.gameObject.SetActive(false);
    }

    private void AddFear(OnPersonMoreFearEvent eventData)
    {
        if (eventData.Target != this) return;
        if (_fearSources.TryAdd(eventData.Source, eventData.FearAmount)) { CalculateTrustFear(); return; }
        if (_fearSources[eventData.Source] >= fromSourceMaxAmount) { CalculateTrustFear(); return; }
        _fearSources[eventData.Source] += eventData.FearAmount;
        uiHandler.UpdateUiFear(eventData.FearAmount);
        CalculateTrustFear();
    }

    private void AddTrust(OnPersonMoreTrustEvent eventData)
    {
        if (eventData.Target != this) return;
        if (_trustSources.TryAdd(eventData.Source, eventData.TrustAmount)) { CalculateTrustFear(); return; }
        if (_trustSources[eventData.Source] >= fromSourceMaxAmount) { CalculateTrustFear(); return; }
        _trustSources[eventData.Source] += eventData.TrustAmount;
        uiHandler.UpdateUiTrust(eventData.TrustAmount);
    }

    private void CalculateTrustFear()
    {
        var trustFear = (_currentPersonType.defaultTrust, _currentPersonType.defaultFear);
        _currentFear += trustFear.defaultFear;
        _currentTrust += trustFear.defaultTrust;
        Debug.LogWarning("Current trust and current fear implementation continue");
        foreach (var source in _fearSources) { trustFear.defaultTrust += source.Value; }
        foreach (var source in _trustSources) { trustFear.defaultFear += source.Value; }
        G.GetService<SpecialGameStatesService>().GetState<CurrentPersonTrustFearAmount>().Set(this, trustFear);
    }

    private void OnDestroy()
    {
        G.GetManager<RoutineManager>()?.StopUpdateAction(UpdateColliderRotation);
        _currentPersonType?.Deinitialize();
        _currentPersonType = null;
        _fearSources.Clear();
        _trustSources.Clear();
        _internalFearEvent = null;
        uiHandler.DeInitializeUi();
    }
}

public enum FearSource
{
    HoldFire
}

public enum TrustSource
{
        
}
