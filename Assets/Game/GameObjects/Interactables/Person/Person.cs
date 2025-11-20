using UnityEngine;

public class Person : InteractableObject, IShootable, IExaminable
{
    [SerializeField] private PersonType[] personTypes;
    [SerializeField] private Rigidbody keyRigidBody;
    [SerializeField] private Transform bodyKey;
    [SerializeField] private Canvas examineUi;
    [SerializeField] private Transform visual;
    private TypeConfig[] _personTypeConfigs; 
    private PersonType _currentPersonType;
    private bool _isShot;
    
    public Canvas ExamineUi => examineUi;
    public Transform Visual => visual;
    
    protected override void Launch()
    {
        if (!Application.isPlaying) return;
        G.GetManager<RoutineManager>().StartUpdateAction(UpdateColliderRotation);
        ChangePersonType();
    }

    private void UpdateColliderRotation()
    {
        transform.rotation = bodyKey.rotation;
    }

    private void ChangePersonType()
    {
        var currentState = G.GetService<SpecialGameStatesService>().GetState<CurrentGameStage>().Get();
        foreach (var personType in personTypes)
        {
            if (personType.stageNum != currentState) continue;
            _currentPersonType = personType;
            _personTypeConfigs = _currentPersonType.personTypeConfigs;
            break;
        }
    }

    public void GetShot()
    {
        if (_isShot) return;
        _isShot = true;
        var rawVector = G.GetManager<CameraManager>().GetCameraTransform().position - bodyKey.position;
        var vector = new Vector3(-rawVector.x, 3f, -rawVector.z);
        keyRigidBody.AddForce(vector * 100f, ForceMode.Impulse);
        EventService.Invoke(new OnPersonShotEvent {Person = this});
    }

    public void Examine()
    {
        examineUi.gameObject.SetActive(true);
        foreach (var config in _personTypeConfigs) { config.PerformConfiguration(this); }
    }

    public void Release()
    {
        examineUi.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        G.GetManager<RoutineManager>()?.StopUpdateAction(UpdateColliderRotation);
        _currentPersonType = null;
    }
}
