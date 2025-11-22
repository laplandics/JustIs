public class StageOne : Stage
{
    public override void StartStage()
    {
        EventService.Subscribe<OnStageLoadEvent>(FirstPart);
        EventService.Subscribe<OnTabletExaminedEvent>(SecondPart);
        EventService.Subscribe<OnRevolverGrabStatusChanged>(ThirdPart);
        EventService.Subscribe<OnPersonShotEvent>(FourthPart);
        base.StartStage();
    }
    
    private void FirstPart(OnStageLoadEvent _)
    {
        G.GetService<SpecialGameStatesService>().GetState<CurrentMonitorUi>().Set(MonitorUiType.ShouldReadTablet);
    }

    private void SecondPart(OnTabletExaminedEvent _)
    {
        G.GetService<SpecialGameStatesService>().GetState<CurrentMonitorUi>().Set(MonitorUiType.ShouldGrabRevolver);
    }

    private void ThirdPart(OnRevolverGrabStatusChanged eventData)
    {
        var uiType = eventData.IsGrabbed ? MonitorUiType.ShouldShootPerson : MonitorUiType.ShouldGrabRevolver;
        G.GetService<SpecialGameStatesService>().GetState<CurrentMonitorUi>().Set(uiType);
    }

    private void FourthPart(OnPersonShotEvent _)
    {
        
        G.GetService<SpecialGameStatesService>().GetState<CurrentMonitorUi>().Set(MonitorUiType.GoodJob);
        EventService.Invoke(new OnStageEndEvent { NextStage = StageNum.StageTwo });
    }

    public override void EndStage()
    {
        EventService.Unsubscribe<OnStageLoadEvent>(FirstPart);
        EventService.Unsubscribe<OnTabletExaminedEvent>(SecondPart);
        EventService.Unsubscribe<OnRevolverGrabStatusChanged>(ThirdPart);
        EventService.Unsubscribe<OnPersonShotEvent>(FourthPart);
        base.EndStage();
    }
}