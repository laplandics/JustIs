public class StageOne : Stage
{
    public override void StartStage()
    {
        EventService.Subscribe<StageEvents.OnStageLoadEvent>(FirstPart);
        EventService.Subscribe<ConfigEvents.Tablet_ExaminedEvent>(SecondPart);
        EventService.Subscribe<ConfigEvents.Revolver_GrabStatusChangedEvent>(ThirdPart);
        EventService.Subscribe<ConfigEvents.Person_ShotEvent>(FourthPart);
        base.StartStage();
    }
    
    private void FirstPart(StageEvents.OnStageLoadEvent _)
    {
        DataInjector.InjectState<CurrentMonitorUi>().Set(MonitorUiType.ShouldReadTablet);
    }

    private void SecondPart(ConfigEvents.Tablet_ExaminedEvent _)
    {
        DataInjector.InjectState<CurrentMonitorUi>().Set(MonitorUiType.ShouldGrabRevolver);
    }

    private void ThirdPart(ConfigEvents.Revolver_GrabStatusChangedEvent eventData)
    {
        var uiType = eventData.IsGrabbed ? MonitorUiType.ShouldShootPerson : MonitorUiType.ShouldGrabRevolver;
        DataInjector.InjectState<CurrentMonitorUi>().Set(uiType);
    }

    private void FourthPart(ConfigEvents.Person_ShotEvent _)
    {
        
        DataInjector.InjectState<CurrentMonitorUi>().Set(MonitorUiType.GoodJob);
        EventService.Invoke(new StageEvents.OnStageEndEvent { NextStage = StageNum.StageTwo });
    }

    public override void EndStage()
    {
        EventService.Unsubscribe<StageEvents.OnStageLoadEvent>(FirstPart);
        EventService.Unsubscribe<ConfigEvents.Tablet_ExaminedEvent>(SecondPart);
        EventService.Unsubscribe<ConfigEvents.Revolver_GrabStatusChangedEvent>(ThirdPart);
        EventService.Unsubscribe<ConfigEvents.Person_ShotEvent>(FourthPart);
        base.EndStage();
    }
}