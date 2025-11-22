public class OnStageLoadEvent {}
public class OnStageEndEvent { public StageNum NextStage; }
public class OnTabletExaminedEvent {}
public class OnRevolverGrabStatusChanged { public bool IsGrabbed; }
public class OnPersonShotEvent { public Person Person; }
public class OnMonitorShotEvent {}
public class OnNewStageObjectSelectedToPrintEvent { public ObjectConfig ObjectConfig; }
public class OnMonitorUiChangedEvent { public MonitorUiType Command; public Monitor SpecificTarget; }
public class OnPlayerTalkToPersonEvent { public Person Target; }
public class OnPersonMoreFearEvent
{
    public Person Target;
    public float FearAmount;
    public FearSource Source;
}
public class OnPersonMoreTrustEvent
{
    public Person Target;
    public float TrustAmount;
    public TrustSource Source;
}