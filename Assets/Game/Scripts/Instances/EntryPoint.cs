using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private ScriptableObject[] gameServices;
    [SerializeField] private MonoBehaviour[] sceneManagers;
    
    private IEnumerator Start()
    {
        Application.quitting += OnEnd;
        yield return InitializeGameServices();
        yield return InitializeSceneManagers();
        DataInjector.InjectState<IsGameStarted>().Set(true);
        EnableInputs();
    }

    private IEnumerator InitializeGameServices()
    {
        var services = new List<IGameService>();
        foreach (var service in gameServices)
        {
            if (service is not IGameService gameService) continue;
            yield return gameService.Run();
            services.Add(gameService);
        }
        G.CacheSceneManagers(services);
    }

    private IEnumerator InitializeSceneManagers()
    {
        var managers = new List<ISceneManager>();
        foreach (var manager in sceneManagers)
        {
            if (manager is not ISceneManager sceneManager) continue;
            sceneManager.Initialize();
            managers.Add(sceneManager);
            yield return null;
        }
        G.CacheGameServices(managers);
    }

    private void EnableInputs() { G.GetService<InputService>().ChangeInputs(InputsType.Player); }

    private void OnEnd()
    {
        Application.quitting -= OnEnd;
        DataInjector.InjectState<IsGameStarted>().Set(false);
        DeinitializeSceneManagers();
        StopGameServices();
        G.ResetData();
    }

    private void DeinitializeSceneManagers()
    {
        foreach (var manager in sceneManagers)
        {
            if (manager is not ISceneManager sceneManager) continue;
            sceneManager.Deinitialize();
        }
    }

    private void StopGameServices()
    {
        foreach (var service in gameServices)
        {
            if (service is not IGameService gameService) continue;
            gameService.Stop();
        }
    }
}
