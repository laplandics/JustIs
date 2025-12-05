using UnityEngine;

public class Revolver : InteractableObject, IGrabable
{
    [SerializeField] private Collider interactionCollider;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private Rigidbody rb;
    
    public Rigidbody Rb => rb;
    public Transform HoldPoint => holdPoint;
    public Collider InteractionCollider => interactionCollider;

    public void Grab(Transform parent)
    {
        interactionCollider.enabled = false;
        Rb.isKinematic = true;
        var tr = transform;
        tr.SetParent(parent);
        tr.position = parent.position;
        tr.rotation = parent.rotation;
        tr.localScale = Vector3.one;
        tr.localPosition -= HoldPoint.localPosition;
        tr.localRotation = Quaternion.Inverse(HoldPoint.localRotation);
        
        DataInjector.InjectState<IsRevolverInHands>().Set(true);
    }

    public void Release(Transform parent, Vector3 position)
    {
        interactionCollider.enabled = true;
        var tr = transform;
        tr.SetParent(parent);
        tr.position = position;
        tr.rotation = Quaternion.Euler(GetRotation());
        tr.localScale = Vector3.one;
        Rb.isKinematic = false;
        
        DataInjector.InjectState<IsRevolverInHands>().Set(false);
    }
    
    private static Vector3 GetRotation()
    {
        var randomX = Random.Range(0, 360);
        var randomY = Random.Range(0, 360);
        var randomZ = Random.Range(0, 360);
        var rotationRandom = new Vector3(randomX, randomY, randomZ);
        return rotationRandom;
    }

    public override void Disable()
    {
        DataInjector.InjectState<IsRevolverInHands>().Set(false);
        base.Disable();
    }
}