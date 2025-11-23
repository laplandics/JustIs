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
    private bool _isShot;
    
    [Header("Ui settings")]
    [SerializeField] private PersonUIHandler uiHandler;
    
    [Header("Mood settings")]
    [SerializeField] private PersonMoodHandler moodHandler;
    
    
    public PersonType CurrentPersonType { get; private set; }

    public PersonUIHandler UIHandler => uiHandler;
    public PersonMoodHandler MoodHandler => moodHandler;
    public Canvas ExamineUi => examineUi;
    public Transform Visual => visual;
    
    protected override void Launch()
    {
        if (!Application.isPlaying) return;
        G.GetManager<RoutineManager>().StartUpdateAction(UpdateColliderRotation);
        CurrentPersonType = GetPersonType();
        moodHandler.Initialize();
        uiHandler.InitializeUi();
    }

    private void UpdateColliderRotation() { transform.rotation = bodyKey.rotation; }

    private PersonType GetPersonType()
    {
        var currentStage = DataInjector.InjectState<CurrentGameStage>().Get().StageNum;
        foreach (var personType in personTypes)
        {
            if (personType.typeStage != currentStage) continue;
            personType.Initialize(this);
            return personType;
        }
        Debug.LogError("No person type found");
        return null;
    }

    public void TakeAim(float time, out bool isShot)
    {
        isShot = false;
        if (_isShot) {isShot = true; return;}
        moodHandler.SendNewMoodDirectly(-time, MoodChangeSource.WeaponAim);
        if (!(time >= timeToShoot)) return;
        isShot = true;
        GetShot();
    }

    private void GetShot()
    {
        _isShot = true;
        var rawVector = G.GetManager<CameraManager>().GetCameraTransform().position - bodyKey.position;
        var vector = new Vector3(-rawVector.x, 3f, -rawVector.z);
        keyRigidBody.AddForce(vector * 100f, ForceMode.Impulse);
        EventService.Invoke(new OnPersonShotEvent {Person = this});
    }

    public void ReleaseAim() {}

    public void Examine()
    {
        examineUi.gameObject.SetActive(true);
        EventService.Invoke(new OnPlayerTalkToPersonEvent {Person = this});
    }

    public void Release() { examineUi.gameObject.SetActive(false); }

    public override void Disable()
    {
        G.GetManager<RoutineManager>()?.StopUpdateAction(UpdateColliderRotation);
        CurrentPersonType?.Deinitialize();
        CurrentPersonType = null;
        uiHandler.DeInitializeUi();
        moodHandler.Deinitialize();
        base.Disable();
    }
}
