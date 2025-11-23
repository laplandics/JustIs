using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStageManager : MonoBehaviour, ISceneManager
{
    private readonly Dictionary<StageNum, Stage> _stages = new();
    private Coroutine _stageCycleRoutine;
    private Stage _currentStage;
    private bool _isGameInProgress;
    private bool _hasStageChanged;

    public void Initialize()
    {
        var stages = gameObject.GetComponentsInChildren<Stage>().ToArray();
        foreach (var stage in stages) { _stages.Add(stage.StageNum, stage); }
        EventService.Subscribe<OnStageEndEvent>(ChangeStage);
        EventService.Subscribe<OnGameEnded>(DestroyStages);
        _currentStage = _stages[StageNum.StageOne];
    }

    public void BeginCycle() { _isGameInProgress = true; _stageCycleRoutine = G.GetManager<RoutineManager>().StartRoutine(StageCycle()); }

    private IEnumerator StageCycle()
    {
        while (_isGameInProgress)
        {
            var currentStage = _currentStage;
            _currentStage.StartStage();
            yield return new WaitUntil(() => _hasStageChanged);
            _hasStageChanged = false;
            yield return new WaitForSeconds(0.3f);
            currentStage.EndStage();
        }
    }

    private void ChangeStage(OnStageEndEvent eventData)
    {
        if (!_stages.TryGetValue(eventData.NextStage, out var stage)) {Debug.LogWarning("Stage is not implemented"); return;}
        _currentStage = stage;
        _hasStageChanged = true;
        if (!_isGameInProgress) _stageCycleRoutine = G.GetManager<RoutineManager>().StartRoutine(StageCycle());
    }

    public void EndCycle() {_isGameInProgress = false; if (_stageCycleRoutine != null) G.GetManager<RoutineManager>().EndRoutine(_stageCycleRoutine);}

    private void DestroyStages(OnGameEnded _)
    {
        _currentStage.EndStage();
        _stages.Clear();
        DataInjector.InjectState<CurrentGameStage>().Set(null);
    }
    
    public void Deinitialize()
    {
        EventService.Unsubscribe<OnGameEnded>(DestroyStages);
        EventService.Unsubscribe<OnStageEndEvent>(ChangeStage);
    }
}

public enum StageNum
{
    None,
    StageOne,
    StageTwo,
    StageThree,
    StageFour,
}
