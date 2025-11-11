using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "StageObjectsService", menuName = "Services/StageObjectsService")]
public class StageObjectsService : ScriptableObject, IGameService
{
    public StageObject[] stageObjects;

    [Button]
    public void LoadStageObjects()
    {
        foreach (var stageObject in stageObjects)
        {
            stageObject.Load();
        }
        EventService.Invoke(new OnStageLoadEvent());
    }

    [Button]
    public void UnloadStageObjects()
    {
        foreach (var stageObject in stageObjects)
        {
            stageObject.Unload();
        }
    }
    
    public void Run() {}
    public void Stop() {}
}