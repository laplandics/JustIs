using System;
using UnityEngine;

[Serializable]
public class Event {}
public class ConfigEvent : Event {}

namespace GameEvents
{
    public class OnGameStarted : Event {}
    public class OnGameEnded : Event {}
}

namespace UiEvents
{
    public class OnUiInteractionStarted : Event { public InputsType InputsType; public CameraConfigurationPreset CameraConfigPreset; }
    public class OnUiInteractionEnded : Event {}
    public class OnUpdateUIEvent : Event { public Sprite Sprite; }
}

namespace StateEvents
{
    public class OnSomeStateChangedEvent : Event { public SpecialGameState State; }
}

namespace StageEvents
{
    public class OnStageLoadEvent : Event {}
    public class OnStageEndEvent : Event { public StageNum NextStage; }
}

namespace ConfigEvents
{
    public class Player_SpawnedEvent : ConfigEvent { public Player Player; }
    public class Player_DespawnedEvent : ConfigEvent {}
    
    public class Tablet_ExaminedEvent : ConfigEvent {}
    
    public class Revolver_GrabStatusChangedEvent : ConfigEvent { public bool IsGrabbed; }
    
    public class Computer_NewObjectSelectedToPrintEvent : ConfigEvent { public ObjectConfig ObjectConfig; }
    
    public class Monitor_ShotEvent : ConfigEvent {}
    public class Monitor_UiChangedEvent : ConfigEvent { public MonitorUiType Command; public Monitor SpecificTarget; }
    
    public class Person_NewPersonTypeChosen : ConfigEvent { public PersonType PersonType; }
    public class Person_ShotEvent : ConfigEvent { public Person Person; }
    public class Person_TalkEvent : ConfigEvent { public Person Person; }
    public class Person_MoodChangedEvent : ConfigEvent { public Person Person; public MoodChangeSource Source; public float Amount; }

    public class AncestorsStatue_ExaminedEvent : ConfigEvent {}
}

namespace ExamineEvents
{
    public class OnStoryChoiceMadeEvent : Event
    {
        public ChoiceEvent StoryEvent;

        public class ChoiceEvent
        {
            
        }
        
        public class StoryPathBegin : ChoiceEvent
        {
            public IExaminable Target;
            public StoryData Path;
        }

        public class StoryPathEnd : ChoiceEvent
        {
            
        }
    }
}