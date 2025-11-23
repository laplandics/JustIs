using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputService", menuName = "Services/InputService")]
public class InputService : ScriptableObject, IGameService
{
    private Dictionary<InputsType, string> _actions;
    private PlayerInputsManager _playerInputsManager;

    public void Run()
    {
        _actions = new Dictionary<InputsType, string>();
        EventService.Subscribe<OnGameStarted>(AssignManagers);
        EventService.Subscribe<OnUiInteractionStarted>(LockPlayer);
        EventService.Subscribe<OnUiInteractionEnded>(FreePlayer);
    }
    
    private void AssignManagers(OnGameStarted obj) { _playerInputsManager = G.GetManager<PlayerInputsManager>(); }
    private void LockPlayer(OnUiInteractionStarted data) { ChangeInputs(data.InputsType); }
    private void FreePlayer(OnUiInteractionEnded _) => ChangeInputs(InputsType.Player);

    public void ChangeInputs(InputsType type) { _playerInputsManager.GetPlayerInput().SwitchCurrentActionMap(type.ToString()); }

    public void Stop()
    {
        EventService.Unsubscribe<OnGameStarted>(AssignManagers);
        EventService.Unsubscribe<OnUiInteractionStarted>(LockPlayer);
        EventService.Unsubscribe<OnUiInteractionEnded>(FreePlayer);
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