using UnityEngine;

public class RoomCamera : InteractableObject
{
    [SerializeField] private Transform hinge;
    [SerializeField] private Transform cam;
    
    protected override void Launch()
    {
        if (!Application.isPlaying) return;
        G.GetManager<RoutineManager>().StartUpdateAction(UpdateCameraRotation);
    }

    private void UpdateCameraRotation()
    {
        var cameraTr = G.GetManager<CameraManager>().GetCameraTransform();
        cam.localRotation = Quaternion.LookRotation(cameraTr.position - cam.position);
    }

    public override void Disable()
    {
        G.GetManager<RoutineManager>().StopUpdateAction(UpdateCameraRotation);
        base.Disable();
    }
}
