using UnityEngine;

public class Computer : InteractableObject, IExaminable
{
    [SerializeField] private Canvas examineUi;
    [SerializeField] private Transform visual;
    [SerializeField] private ComputerUIHandler computerUIHandler;
    
    public Canvas ExamineUi => examineUi;
    public Transform Visual => visual;
    public void Examine()
    {
        examineUi.gameObject.SetActive(true);
        computerUIHandler.InitializeUi(this);
    }

    public void MarkObject(ObjectConfig config)
    {
        config.traits.Remove(ObjectTrait.Locked);
        config.traits.Add(ObjectTrait.Unlocked);
        EventService.Invoke(new OnNewStageObjectSelectedToPrintEvent {ObjectConfig = config});
    }

    public void Release()
    {
        computerUIHandler.DeInitializeUi();
        examineUi.gameObject.SetActive(false);
    }
}
