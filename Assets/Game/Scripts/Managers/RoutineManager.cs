using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutineManager : MonoBehaviour, ISceneManager
{
    private readonly List<Action> _updateActions = new();
    private readonly List<Action> _fixedUpdateActions = new();
    private readonly List<Action> _lateUpdateActions = new();

    public void Initialize() {}
    public Coroutine StartRoutine(IEnumerator routine) {var newRoutine = StartCoroutine(routine); return newRoutine;}
    public void EndRoutine(Coroutine coroutine) {StopCoroutine(coroutine);}
    public void StartUpdateAction(Action action) => _updateActions.Add(action);
    public void StartFixedUpdateAction(Action action) => _fixedUpdateActions.Add(action);
    public void StartLateUpdateAction(Action action) => _lateUpdateActions.Add(action);
    public void StopUpdateAction(Action action) => _updateActions.Remove(action);
    public void StopFixedUpdateAction(Action action) => _fixedUpdateActions.Remove(action);
    public void StopLateUpdateAction(Action action) => _lateUpdateActions.Remove(action);
    private void Update() {foreach (var action in _updateActions){action?.Invoke();}}
    private void FixedUpdate(){foreach (var action in _fixedUpdateActions){action?.Invoke();}}
    private void LateUpdate(){foreach (var action in _lateUpdateActions){action?.Invoke();}}

    public void Deinitialize() { _updateActions.Clear(); _fixedUpdateActions.Clear(); _lateUpdateActions.Clear(); }
    
    private void OnDestroy() { StopAllCoroutines(); }
}