using UnityEngine;

public interface IGrabable
{
    public Transform HoldPoint { get; }
    public Rigidbody Rb { get; }
    public void Grab(Transform parent);
    public void Release(Transform parent, Vector3 position);
}