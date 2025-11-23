using UnityEngine;
using static EventService;

public class GameManager : MonoBehaviour, ISceneManager
{
    public void Initialize()
    {
        Subscribe<OnGameStarted>(StartGame);
    }

    private void StartGame(OnGameStarted eventData)
    {
        G.GetManager<GameStageManager>().BeginCycle();
        G.GetManager<UiManager>().HideCursor();
    }
    
    public void Deinitialize()
    {
        G.GetManager<GameStageManager>().EndCycle();
        G.GetManager<UiManager>().ShowCursor();
    }
}