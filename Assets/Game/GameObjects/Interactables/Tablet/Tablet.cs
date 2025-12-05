using UnityEngine;

public class Tablet : InteractableObject, IExaminable
{
    [SerializeField] private Canvas ui;
    [SerializeField] private Transform visual;
    [SerializeField] private Transform examineContainer;
    [SerializeField] private Transform buttonsContainer;

    public void Examine()
    {
        ui.gameObject.SetActive(true);
    }

    public void Release()
    {
        ui.gameObject.SetActive(false);
        EventService.Invoke(new ConfigEvents.Tablet_ExaminedEvent());
    }
}
