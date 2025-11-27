using UnityEngine;
using static EventService;

public class GameManager : MonoBehaviour, ISceneManager
{
    public void Initialize()
    {
        Subscribe<GameEvents.OnGameStarted>(StartGame);
    }

    private void StartGame(GameEvents.OnGameStarted eventData)
    {
        G.GetManager<GameStageManager>().BeginCycle();
        UiManager.HideCursor();
    }
    
    public void Deinitialize()
    {
        G.GetManager<GameStageManager>().EndCycle();
        UiManager.ShowCursor();
    }
}