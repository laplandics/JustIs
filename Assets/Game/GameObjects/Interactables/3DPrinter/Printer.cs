public class Printer : InteractableObject
{
    public ObjectConfig SelectedObject { get; set; }
    
    protected override void Launch()
    {
        EventService.Subscribe<OnNewStageObjectSelectedToPrintEvent>(ChangeSelectedObject);
    }

    private void ChangeSelectedObject(OnNewStageObjectSelectedToPrintEvent eventData) { SelectedObject = eventData.ObjectConfig; }
}