using UnityEngine;

public class OnGameStarted {}
public class OnPlayerSpawnedEvent { public Player Player; }
public class OnPlayerDespawnedEvent {}
public class OnInteractionEventStarted { public IInteractionPreset Preset; }
public class OnInteractionEventEnded {}
public class OnUpdateUIEvent { public Sprite Sprite; }
public class OnObjectSpawnEvent { public MonoBehaviour Object; }
public class OnMonitorUiChangedEvent
{
    public EventType Type;
    public enum EventType
    {
        ShouldReadTablet,
        ShouldGrabRevolver,
        ShouldShootPerson,
        GoodJob
    }
}