using UnityEngine;

public interface IGrabable
{
    public void Grab(Transform parent);
    public void Release(Transform parent, Vector3 position);
}