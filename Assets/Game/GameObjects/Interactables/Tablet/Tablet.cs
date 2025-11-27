using System.Collections;
using UnityEngine;

public class Tablet : InteractableObject, IExaminable
{
    [SerializeField] private Canvas ui;
    [SerializeField] private Transform visual;
    [SerializeField] private Transform textContainer;
    private bool _isExamining;
    
    public Transform TextContainer => textContainer;
    public Canvas UI => ui;
    public Transform Visual => visual;

    public void Examine()
    {
        _isExamining = true;
        ui.gameObject.SetActive(true);
        G.GetManager<RoutineManager>().StartRoutine(ExamineRoutine());
    }

    public IEnumerator ExamineRoutine()
    {
        yield return new WaitUntil(() => !_isExamining);
    }

    public void Release()
    {
        _isExamining = false;
        ui.gameObject.SetActive(false);
        EventService.Invoke(new ConfigEvents.Tablet_ExaminedEvent());
    }
}
