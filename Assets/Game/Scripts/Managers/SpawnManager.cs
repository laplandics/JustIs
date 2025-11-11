using UnityEngine;

[ExecuteAlways]
public class SpawnManager :  MonoBehaviour, ISceneManager
{
    public void Initialize() {}

    public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : Component
    {
        return parent ? Instantiate(prefab, position, rotation, parent) : Instantiate(prefab, position, rotation);
    }
    
    public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        return parent ? Instantiate(prefab, position, rotation, parent) : Instantiate(prefab, position, rotation);
    }
    
    public static void Despawn(GameObject obj) => Destroy(obj);
    public static void DespawnImmediate(GameObject obj) => DestroyImmediate(obj);
    
    public void Deinitialize() {}
}