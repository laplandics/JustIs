using UnityEngine;

public class Person : InteractableObject, IShootable, IExaminable
{
    [Header("Essentials")]
    [SerializeField] private PersonType[] personTypes;
    [SerializeField] private Rigidbody keyRigidBody;
    [SerializeField] private Transform bodyKey;

    [Header("IExaminable settings")]
    [SerializeField] private Canvas ui;
    [SerializeField] private Transform visual;
    [SerializeField] private Transform buttonsContainer;
    private ExamineStoryRoutine _storyRoutine;
    
    [Header("IShootable settings")]
    [SerializeField] private float timeToShoot;
    private bool _isShot;
    
    [Header("Ui settings")]
    [SerializeField] private PersonUIHandler uiHandler;
    [SerializeField] private Transform examineContainer;
    
    [Header("Mood settings")]
    [SerializeField] private PersonMoodHandler moodHandler;

    public PersonType CurrentPersonType { get; private set; }
    public PersonUIHandler UIHandler => uiHandler;
    
    protected override void Launch()
    {
        if (!Application.isPlaying) return;
        G.GetManager<RoutineManager>().StartUpdateAction(UpdateColliderRotation);
        CurrentPersonType = GetPersonType();
        moodHandler.Initialize();
        uiHandler.InitializeUi();
        _storyRoutine = new ExamineStoryRoutine(buttonsContainer, examineContainer, CurrentPersonType.ExamineStories, this);
    }

    private void UpdateColliderRotation() { transform.rotation = bodyKey.rotation; }

    private PersonType GetPersonType()
    {
        var currentStage = DataInjector.InjectState<CurrentGameStage>().Get().StageNum;
        foreach (var personType in personTypes)
        {
            if (personType.typeStage != currentStage) continue;
            personType.Initialize(this);
            EventService.Invoke(new ConfigEvents.Person_NewPersonTypeChosen {PersonType = personType});
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
        EventService.Invoke(new ConfigEvents.Person_ShotEvent {Person = this});
    }

    public void ReleaseAim() {}

    public void Examine()
    {
        ui.gameObject.SetActive(true);
        DataInjector.InjectState<TalkingWithPerson>().Set(true, this);
        _storyRoutine.StartExamine();
    }

    public void Release()
    {
        ui.gameObject.SetActive(false);
        DataInjector.InjectState<TalkingWithPerson>().Set(false);
        _storyRoutine.StopExamine();
    }

    public override void Disable()
    {
        G.GetManager<RoutineManager>()?.StopUpdateAction(UpdateColliderRotation);
        CurrentPersonType?.Deinitialize();
        CurrentPersonType = null;
        uiHandler.DeInitializeUi();
        moodHandler.Deinitialize();
        _storyRoutine.Dispose();
        DataInjector.InjectState<TalkingWithPerson>().Set(false);
        base.Disable();
    }
}
