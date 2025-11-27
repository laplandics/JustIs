using System.Collections;
using System.Collections.Generic;
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
    private bool _isExamining;
    
    [Header("IShootable settings")]
    [SerializeField] private float timeToShoot;
    private bool _isShot;
    
    [Header("Ui settings")]
    [SerializeField] private PersonUIHandler uiHandler;
    [SerializeField] private Transform textContainer;
    
    [Header("Mood settings")]
    [SerializeField] private PersonMoodHandler moodHandler;

    public Transform TextContainer => textContainer;
    public PersonType CurrentPersonType { get; private set; }
    public PersonUIHandler UIHandler => uiHandler;
    public PersonMoodHandler MoodHandler => moodHandler;
    public Canvas UI => ui;
    public Transform Visual => visual;
    
    protected override void Launch()
    {
        if (!Application.isPlaying) return;
        ExamineStory.RegisterStoryObject(GetType());
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
        _isExamining = true;
        ui.gameObject.SetActive(true);
        EventService.Invoke(new ConfigEvents.Person_TalkEvent {Person = this});
        G.GetManager<RoutineManager>().StartRoutine(ExamineRoutine());
    }
    
    public IEnumerator ExamineRoutine()
    {
        var lines = ExamineStory.GetStoryLines(GetType());
        var textBlocks = new List<TextBlock>();
        foreach (var line in lines)
        {
            var textBlock = new TextBlock(line);
            textBlock.SpawnElements(textContainer);
            textBlocks.Add(textBlock);
        }
        yield return new WaitUntil(() => !_isExamining);
        foreach (var textBlock in textBlocks) textBlock.DespawnElements();
    }

    public void Release()
    {
        _isExamining = false;
        ui.gameObject.SetActive(false);
    }

    public override void Disable()
    {
        G.GetManager<RoutineManager>()?.StopUpdateAction(UpdateColliderRotation);
        CurrentPersonType?.Deinitialize();
        CurrentPersonType = null;
        uiHandler.DeInitializeUi();
        moodHandler.Deinitialize();
        ExamineStory.UnregisterStoryObject(GetType());
        base.Disable();
    }
}
