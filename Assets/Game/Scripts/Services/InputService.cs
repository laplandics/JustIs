using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputService", menuName = "Services/InputService")]
public class InputService : ScriptableObject, IGameService
{
    private Dictionary<InputsType, string> _actions;
    private PlayerInputsManager _playerInputsManager;

    public IEnumerator Run()
    {
        _actions = new Dictionary<InputsType, string>();
        EventService.Subscribe<GameEvents.OnGameStarted>(AssignManagers);
        EventService.Subscribe<UiEvents.OnUiInteractionStarted>(LockPlayer);
        EventService.Subscribe<UiEvents.OnUiInteractionEnded>(FreePlayer);
        yield break;
    }
    
    private void AssignManagers(GameEvents.OnGameStarted obj) { _playerInputsManager = G.GetManager<PlayerInputsManager>(); }
    private void LockPlayer(UiEvents.OnUiInteractionStarted data) { ChangeInputs(data.InputsType); }
    private void FreePlayer(UiEvents.OnUiInteractionEnded _) => ChangeInputs(InputsType.Player);

    public void ChangeInputs(InputsType type) { _playerInputsManager.GetPlayerInput().SwitchCurrentActionMap(type.ToString()); }

    public void Stop()
    {
        EventService.Unsubscribe<GameEvents.OnGameStarted>(AssignManagers);
        EventService.Unsubscribe<UiEvents.OnUiInteractionStarted>(LockPlayer);
        EventService.Unsubscribe<UiEvents.OnUiInteractionEnded>(FreePlayer);
        _actions.Clear();
        _actions = null;
        _playerInputsManager = null;
    }
}

public enum InputsType
{
    Player,
    Interaction,
    UI,
    Debug
}