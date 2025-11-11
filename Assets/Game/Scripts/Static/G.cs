using System;
using System.Collections.Generic;

public static class G
{
    private static readonly Dictionary<Type, ISceneManager> Managers = new();
    private static readonly Dictionary<Type, IGameService> Services = new();
    
    public static void CacheGameServices(List<ISceneManager> managers)
    {
        Managers.Clear();
        foreach (var manager in managers) { Managers.Add(manager.GetType(), manager); }
    }

    public static void CacheSceneManagers(List<IGameService> services)
    {
        Services.Clear();
        foreach (var service in services) { Services.Add(service.GetType(), service); }
    }

    public static T GetService<T>()
    {
        if (!Services.TryGetValue(typeof(T), out var service)) return default;
        return service is not T searchingService ? default : searchingService;
    }
    
    public static T GetManager<T>()
    {
        if (!Managers.TryGetValue(typeof(T), out var manager)) return default;
        return manager is not T searchingManager ? default : searchingManager;
    }

    public static void ResetData() { Managers.Clear(); Services.Clear(); }
}