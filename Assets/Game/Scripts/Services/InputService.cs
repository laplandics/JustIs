using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputService", menuName = "Services/InputService")]
public class InputService : ScriptableObject, IGameService
{
    [SerializeField] private InputTypeNamePair[] inputMaps;
    
    private Dictionary<InputsType, string> _actions;
    private PlayerManager _playerManager;

    public void Run()
    {
        _actions = new Dictionary<InputsType, string>();
        foreach (var inputMap in inputMaps) { _actions.TryAdd(inputMap.type, inputMap.name); }
        EventService.Subscribe<OnGameStarted>(AssignManagers);
        EventService.Subscribe<OnInteractionEventStarted>(LockPlayer);
        EventService.Subscribe<OnInteractionEventEnded>(FreePlayer);
    }
    
    private void AssignManagers(OnGameStarted obj) { _playerManager = G.GetManager<PlayerManager>(); }
    private void LockPlayer(OnInteractionEventStarted eventData) { if (eventData.Preset is IPlayerLocker) ChangeInputs(InputsType.Interaction); }
    private void FreePlayer(OnInteractionEventEnded _) => ChangeInputs(InputsType.Player);

    public void ChangeInputs(InputsType type) { _playerManager.GetPlayerInput().SwitchCurrentActionMap(_actions[type]); }

    public void Stop()
    {
        EventService.Unsubscribe<OnGameStarted>(AssignManagers);
        EventService.Unsubscribe<OnInteractionEventStarted>(LockPlayer);
        EventService.Unsubscribe<OnInteractionEventEnded>(FreePlayer);
        _actions.Clear();
        _actions = null;
        _playerManager = null;
    }
}

[Serializable]
public class InputTypeNamePair
{
    public InputsType type;
    public string name;
}
public enum InputsType
{
    Player,
    Interaction,
    UI,
}