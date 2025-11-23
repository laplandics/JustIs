public class OnStageLoadEvent {}
public class OnStageEndEvent { public StageNum NextStage; }
public class OnTabletExaminedEvent {}
public class OnRevolverGrabStatusChanged { public bool IsGrabbed; }
public class OnPersonShotEvent { public Person Person; }
public class OnMonitorShotEvent {}
public class OnNewStageObjectSelectedToPrintEvent { public ObjectConfig ObjectConfig; }
public class OnMonitorUiChangedEvent { public MonitorUiType Command; public Monitor SpecificTarget; }
public class OnPlayerTalkToPersonEvent { public Person Person; }
public class OnPersonMoodChangedEvent {public Person Person; public MoodChangeSource Source; public float Amount;}