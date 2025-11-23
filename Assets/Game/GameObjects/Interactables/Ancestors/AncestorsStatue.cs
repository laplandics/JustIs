using UnityEngine;

public class AncestorsStatue : InteractableObject, IExaminable, IGrabable
{
    [SerializeField] private Collider interactionCollider;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private Canvas examineUi;
    [SerializeField] private Transform visual;
    
    public Canvas ExamineUi => examineUi;
    public Transform Visual => visual;
    public Transform HoldPoint => holdPoint;
    public Rigidbody Rb => rb;
    public Collider InteractionCollider => interactionCollider;
    
    public void Examine()
    {
        examineUi.gameObject.SetActive(true);
        Debug.Log(Story.Ancestors.FirstExamine);
    }

    public void Release()
    {
        examineUi.gameObject.SetActive(false);
    }

    public void Grab(Transform parent)
    {
        Rb.isKinematic = true;
        var tr = transform;
        tr.SetParent(parent);
        tr.position = parent.position;
        tr.localScale = Vector3.one;
        tr.localPosition -= HoldPoint.localPosition;
        tr.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
    }

    public void Release(Transform parent, Vector3 position)
    {
        interactionCollider.enabled = true;
        var tr = transform;
        tr.SetParent(parent);
        tr.position = position;
        var cameraTr = G.GetManager<CameraManager>().GetCameraTransform();
        var cameraPos = new Vector3(cameraTr.position.x, position.y, cameraTr.position.z);
        tr.rotation = Quaternion.LookRotation(cameraPos - position);
        tr.localScale = Vector3.one;
        Rb.isKinematic = false;
    }
}