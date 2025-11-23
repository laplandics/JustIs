public class Printer : InteractableObject
{
    public ObjectConfig SelectedObject { get; set; }
    
    protected override void Launch()
    {
        EventService.Subscribe<OnNewStageObjectSelectedToPrintEvent>(ChangeSelectedObject);
    }

    private void ChangeSelectedObject(OnNewStageObjectSelectedToPrintEvent eventData) { SelectedObject = eventData.ObjectConfig; }

    public override void Disable()
    {
        EventService.Unsubscribe<OnNewStageObjectSelectedToPrintEvent>(ChangeSelectedObject);
        base.Disable();
    }
}