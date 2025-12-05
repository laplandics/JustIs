using UnityEngine;

public class Computer : InteractableObject, IExaminable
{
    [SerializeField] private Canvas ui;
    [SerializeField] private Transform visual;
    [SerializeField] private ComputerUIHandler computerUIHandler;
    [SerializeField] private Transform examineContainer;
    [SerializeField] private Transform buttonsContainer;

    public void Examine()
    {
        ui.gameObject.SetActive(true);
        computerUIHandler.InitializeUi(this);
    }

    public void Release()
    {
        computerUIHandler.DeInitializeUi();
        ui.gameObject.SetActive(false);
    }

    public void MarkObject(ObjectConfig config)
    {
        if (config.traits.Contains(ObjectTrait.Locked)) {config.traits.Remove(ObjectTrait.Locked); config.traits.Add(ObjectTrait.Unlocked);}
        EventService.Invoke(new ConfigEvents.Computer_NewObjectSelectedToPrintEvent {ObjectConfig = config});
    }
}
