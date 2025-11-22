using UnityEngine;

public class OnGameStarted {}
public class OnPlayerSpawnedEvent { public Player Player; }
public class OnPlayerDespawnedEvent {}
public class OnUiInteractionStarted { public InputsType InputsType; public CameraConfigurationPreset CameraConfigPreset; }
public class OnUiInteractionEnded {}
public class OnUpdateUIEvent { public Sprite Sprite; }
public class OnSomeStateChangedEvent { public SpecialGameState State; }