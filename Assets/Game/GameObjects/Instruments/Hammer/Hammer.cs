using UnityEngine;

public class Hammer : InteractableObject, IGrabable
{
    [SerializeField] private GameObject nail;
    [SerializeField] private Collider interactionCollider;
    [SerializeField] private Rigidbody rb; 
    [SerializeField] private Transform holdPoint;

    protected override void Launch()
    {
        nail.SetActive(false);
    }

    public void Grab(Transform parent)
    {
        rb.isKinematic = true;
        var tr = transform;
        tr.SetParent(parent);
        tr.position = parent.position;
        tr.rotation = parent.rotation;
        tr.localScale = Vector3.one;
        tr.localPosition -= holdPoint.localPosition;
        tr.localPosition = new Vector3(tr.localPosition.x, -0.15f, tr.localPosition.z);
        tr.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
    }

   public void Release(Transform parent, Vector3 position)
   {
       interactionCollider.enabled = true; 
       var tr = transform; 
       tr.SetParent(parent); 
       tr.position = position; 
       tr.rotation = Quaternion.Euler(GetRotation()); 
       tr.localScale = Vector3.one; 
       rb.isKinematic = false;
   }
   
   private static Vector3 GetRotation()
   {
      var randomX = Random.Range(0, 360);
      var randomY = Random.Range(0, 360);
      var randomZ = Random.Range(0, 360);
      var rotationRandom = new Vector3(randomX, randomY, randomZ);
      return rotationRandom;
   }
}
