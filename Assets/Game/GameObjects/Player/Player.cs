using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private CharacterController playerController;
    [SerializeField] private Transform hand;
    [SerializeField] private Transform shootSelfPoint;
    private Transform _camera;

    public CharacterController Controller => playerController;
    public Transform Hand => hand;
    public Transform ShootSelfPoint => shootSelfPoint;

    public void SetFpCamera(Transform fpCamera, CameraManager manager)
    {
        _camera = fpCamera;
        manager.AssignCamera(cameraPoint);
        manager.RotateCamera(Vector3.forward);
        manager.MoveCamera(Vector3.zero);
        hand.SetParent(_camera);
        shootSelfPoint.SetParent(_camera);
    }

    public void FreeFpCamera(CameraManager manager)
    {
        manager.AssignCamera(null);
        manager.RotateCamera(Vector3.forward);
        manager.MoveCamera(Vector3.zero);
        _camera = null;
    }
}