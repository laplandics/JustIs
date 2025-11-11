using UnityEngine;
using static EventService;

public class GameManager : MonoBehaviour, ISceneManager
{
    public void Initialize()
    {
        Subscribe<OnManagersInitializedEvent>(StartGame);
    }

    private void StartGame(OnManagersInitializedEvent eventData)
    {
        G.GetManager<GameStageManager>().BeginCycle();
        G.GetManager<UiManager>().HideCursor();
    }
    
    public void Deinitialize()
    {
        G.GetManager<UiManager>().ShowCursor();
    }
}