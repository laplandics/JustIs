public class Printer : InteractableObject
{
    public ObjectConfig SelectedObject { get; set; }
    
    protected override void Launch()
    {
        EventService.Subscribe<ConfigEvents.Computer_NewObjectSelectedToPrintEvent>(ChangeSelectedObject);
    }

    private void ChangeSelectedObject(ConfigEvents.Computer_NewObjectSelectedToPrintEvent eventData) { SelectedObject = eventData.ObjectConfig; }

    public override void Disable()
    {
        EventService.Unsubscribe<ConfigEvents.Computer_NewObjectSelectedToPrintEvent>(ChangeSelectedObject);
        base.Disable();
    }
}