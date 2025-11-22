using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStageManager : MonoBehaviour, ISceneManager
{
    private readonly Dictionary<StageNum, Stage> _stages = new();
    private Stage _currentStage;
    private bool _isGameInProgress;
    private bool _hasStageChanged;

    public void Initialize()
    {
        var stages = gameObject.GetComponentsInChildren<Stage>().ToArray();
        foreach (var stage in stages) { _stages.Add(stage.StageNum, stage); }
        EventService.Subscribe<OnStageEndEvent>(ChangeStage);
        _currentStage = _stages[StageNum.StageOne];
    }

    public void BeginCycle() { _isGameInProgress = true; G.GetManager<RoutineManager>().StartRoutine(StageCycle()); }

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
        if (!_stages.TryGetValue(eventData.NextStage, out var stage)) {EndCycle(); return;}
        _currentStage = stage;
        _hasStageChanged = true;
    }

    private void EndCycle() { Debug.Log("This stage is not implemented yet"); }
    
    public void Deinitialize()
    {
        _stages.Clear();
        G.GetService<SpecialGameStatesService>().GetState<CurrentGameStage>().Set(null);
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
