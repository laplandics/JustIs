using System;
using System.Collections;
using UnityEngine;

public class FirstPersonController : IDisposable
{
    private readonly Coroutine _updateTransformRoutine;
    private CharacterController _characterController;
    private PlayerSettings _playerSettings;
    private CameraManager _cameraManager;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private float _currentPitch;
    
    public Vector2 SetMoveInput {set => _moveInput = value; }
    public Vector2 SetLookInput {set => _lookInput = value; }
    private float CurrentPitch { get => _currentPitch; set => _currentPitch = Mathf.Clamp(value, -_playerSettings.pitchLimit, _playerSettings.pitchLimit); }
    private Vector3 CurrentVelocity { get; set; }

    public FirstPersonController(CharacterController characterController, PlayerSettings playerSettings)
    {
        _cameraManager = G.GetManager<CameraManager>();
        _characterController = characterController;
        _playerSettings = playerSettings;
        _updateTransformRoutine = G.GetManager<RoutineManager>().StartRoutine(UpdateTransform());
    }

    private IEnumerator UpdateTransform()
    {
        while (true)
        {
            UpdatePosition();
            UpdateRotation();
            yield return null;
        }
    }

    private void UpdatePosition()
    {
        var motion = _characterController.transform.forward * _moveInput.y + _characterController.transform.right * _moveInput.x;
        motion.y = 0f;
        motion.Normalize();
        var velocity = GetVelocity(motion);
        _characterController.Move(velocity * Time.deltaTime);
    }

    private Vector3 GetVelocity(Vector3 motion)
    {
        CurrentVelocity = motion.sqrMagnitude >= 0.01f ? Vector3.MoveTowards(
            CurrentVelocity, 
            motion * _playerSettings.maxSpeed, 
            _playerSettings.acceleration * Time.deltaTime): 
        Vector3.MoveTowards(
            CurrentVelocity, 
            Vector3.zero, 
            _playerSettings.acceleration * Time.deltaTime);
        var verticalVelocity = _playerSettings.useGravity ? Physics.gravity.y * 600f * Time.deltaTime : 0f;
        var fullVelocity = new Vector3(CurrentVelocity.x, verticalVelocity, CurrentVelocity.z);
        return fullVelocity;
    }

    private void UpdateRotation()
    {
        var input = new Vector2(_lookInput.x * _playerSettings.lookSensitivity.x, _lookInput.y * _playerSettings.lookSensitivity.y);
        CurrentPitch -= input.y;
        _cameraManager.RotateCamera(new Vector3(CurrentPitch, 0f, 0f));
        _characterController.transform.Rotate(Vector3.up * input.x);
    }

    public void Dispose()
    {
        G.GetManager<RoutineManager>()?.EndRoutine(_updateTransformRoutine);
        _characterController = null;
        _cameraManager = null;
        _playerSettings = null;
    }
}
