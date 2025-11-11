public class StageOne : Stage
{
    public override void StartStage()
    {
        EventService.Subscribe<OnStageLoadEvent>(FirstPart);
        EventService.Subscribe<OnTabletExaminedEvent>(SecondPart);
        EventService.Subscribe<OnRevolverGrabbedEvent>(ThirdPart);
        EventService.Subscribe<OnPersonShotEvent>(FourthPart);
        base.StartStage();
    }
    
    private void FirstPart(OnStageLoadEvent _)
    {
        EventService.Invoke(new OnMonitorUiChangedEvent { Type = OnMonitorUiChangedEvent.EventType.ShouldReadTablet });
    }

    private void SecondPart(OnTabletExaminedEvent _)
    {
        EventService.Invoke(new OnMonitorUiChangedEvent { Type = OnMonitorUiChangedEvent.EventType.ShouldGrabRevolver });
    }

    private void ThirdPart(OnRevolverGrabbedEvent _)
    {
        EventService.Invoke(new OnMonitorUiChangedEvent { Type = OnMonitorUiChangedEvent.EventType.ShouldShootPerson });
    }

    private void FourthPart(OnPersonShotEvent _)
    {
        EventService.Invoke(new OnMonitorUiChangedEvent { Type = OnMonitorUiChangedEvent.EventType.GoodJob });
        EventService.Invoke(new OnStageEndEvent { IsSuccessful = true });
    }

    public override void EndStage()
    {
        EventService.Unsubscribe<OnStageLoadEvent>(FirstPart);
        EventService.Unsubscribe<OnTabletExaminedEvent>(SecondPart);
        EventService.Unsubscribe<OnRevolverGrabbedEvent>(ThirdPart);
        EventService.Unsubscribe<OnPersonShotEvent>(FourthPart);
        base.EndStage();
    }
}