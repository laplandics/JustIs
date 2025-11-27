using System.Collections;
using UnityEngine;

public class Computer : InteractableObject, IExaminable
{
    [SerializeField] private Canvas ui;
    [SerializeField] private Transform visual;
    [SerializeField] private ComputerUIHandler computerUIHandler;
    [SerializeField] private Transform textContainer;
    private bool _isExamining;

    public Transform TextContainer => textContainer;
    public Canvas UI => ui;
    public Transform Visual => visual;
    public void Examine()
    {
        _isExamining = true;
        ui.gameObject.SetActive(true);
        computerUIHandler.InitializeUi(this);
        G.GetManager<RoutineManager>().StartRoutine(ExamineRoutine());
    }

    public IEnumerator ExamineRoutine()
    {
        yield return new WaitUntil(() => !_isExamining);
    }

    public void Release()
    {
        _isExamining = false;
        computerUIHandler.DeInitializeUi();
        ui.gameObject.SetActive(false);
    }

    public void MarkObject(ObjectConfig config)
    {
        if (config.traits.Contains(ObjectTrait.Locked)) {config.traits.Remove(ObjectTrait.Locked); config.traits.Add(ObjectTrait.Unlocked);}
        EventService.Invoke(new ConfigEvents.Computer_NewObjectSelectedToPrintEvent {ObjectConfig = config});
    }
}
