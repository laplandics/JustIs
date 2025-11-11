using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStageManager : MonoBehaviour, ISceneManager
{
    private List<Stage> _stages;
    private Stage _currentStage;
    private bool _isGameInProgress;

    public void Initialize()
    {
        _stages = gameObject.GetComponentsInChildren<Stage>().ToList();
        
        EventService.Subscribe<OnStageEndEvent>(ChangeStage);
        
        _currentStage = _stages[0];
    }

    public void BeginCycle() { _isGameInProgress = true; G.GetManager<RoutineManager>().StartRoutine(StageCycle()); }

    private IEnumerator StageCycle()
    {
        while (_isGameInProgress)
        {
            var currentStage = _currentStage;
            LoadStage();
            yield return new WaitUntil(() => _currentStage != currentStage);
            yield return new WaitForSeconds(3f);
            UnloadStage(currentStage);
        }
    }

    private void LoadStage() { _currentStage.StartStage(); }
    
    private void UnloadStage(Stage stage) { stage.EndStage(); }

    private void ChangeStage(OnStageEndEvent eventData)
    {
        var currentStageIndex = _stages.IndexOf(_currentStage);
        var nextStageIndex = currentStageIndex + 1;
        if (nextStageIndex >= _stages.Count)
        {
            EndCycle();
            return;
        }
        _currentStage = _stages[nextStageIndex];
    }

    private void EndCycle()
    {
        _isGameInProgress = false;
    }
    
    public void Deinitialize()
    {
        _stages.Clear();
        EventService.Unsubscribe<OnStageEndEvent>(ChangeStage);
    }
}
