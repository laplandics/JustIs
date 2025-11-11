using UnityEngine;

public class Person : InteractableObject, IShootable, IExaminable
{
    [SerializeField] private Rigidbody keyRigidBody;
    [SerializeField] private Transform bodyKey;
    [SerializeField] private Canvas examineUi;
    [SerializeField] private Transform visual;
    private bool _isShot;
    
    public Canvas ExamineUi => examineUi;
    public Transform Visual => visual;
    
    protected override void Launch()
    {
        G.GetManager<RoutineManager>().StartUpdateAction(UpdateRotation);
    }

    private void UpdateRotation()
    {
        transform.localRotation = bodyKey.localRotation;
    }

    public void GetShot()
    {
        if (_isShot) return;
        _isShot = true;
        var rawVector = G.GetManager<CameraManager>().GetCameraTransform().position - bodyKey.position;
        var vector = new Vector3(-rawVector.x, 3f, -rawVector.z);
        keyRigidBody.AddForce(vector * 100f, ForceMode.Impulse);
        EventService.Invoke(new OnPersonShotEvent {Person = this});
    }

    public void Examine()
    {
        examineUi.gameObject.SetActive(true);
    }

    public void Release()
    {
        examineUi.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        G.GetManager<RoutineManager>()?.StopUpdateAction(UpdateRotation);
    }
}
