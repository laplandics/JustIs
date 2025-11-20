using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStatesService", menuName = "Services/GameStatesService")]
public class SpecialGameStatesService : ScriptableObject, IGameService
{
    [SerializeField] private SpecialGameState[] states;
    private Dictionary<Type, SpecialGameState> _states;

    public void Run() { LoadStates(); }

    private void LoadStates()
    {
        _states = new Dictionary<Type, SpecialGameState>();
        foreach (var state in states) { _states[state.GetType()] = state; }
    }

    public TState GetState<TState>() where TState : SpecialGameState
    {
        if (_states.TryGetValue(typeof(TState), out var state)) { return (TState)state; }
        Debug.LogError($"State of type {typeof(TState).Name} not found!");
        return null;
    }

    public void Stop() {_states = null;}
}