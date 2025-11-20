using System;
using UnityEngine;

public class AimRaycaster : IDisposable
{
    private readonly int _layerMask;
    private InteractableObject _interactable;
    private RoutineManager _routineManager;
    private Transform _rayOrigin;
    private float _lastDistance;
    private bool _distanceChanged;

    public InteractableObject Interactable
    {
        get => _interactable;
        private set
        {
            if (value == _interactable && !_distanceChanged) return;
            if (value == _interactable && _distanceChanged) { _interactable?.DistanceToObjectChanged(InteractableCollider); return; }
            if (value != _interactable) _interactable?.InteractableChanged();
            _interactable = value;
            _interactable?.PrepareInteraction(InteractableCollider);
        }
    }
    
    private Collider InteractableCollider { get; set; }
    
    public AimRaycaster()
    {
        _layerMask = ~G.GetManager<PlayerManager>().GetIgnoredLayers();
        _rayOrigin = G.GetManager<CameraManager>().GetCameraTransform();
        _routineManager = G.GetManager<RoutineManager>();
        _routineManager.StartUpdateAction(FindTarget);
    }

    private void FindTarget()
    {
        _distanceChanged = false;
        var ray = new Ray(_rayOrigin.position, _rayOrigin.forward);
        if (!Physics.Raycast(ray, out var hit, 100f, _layerMask)) { Interactable = null; return; }
        if (Mathf.Approximately(hit.distance, _lastDistance)) return;
        var instance = hit.collider;
        _lastDistance = hit.distance;
        _distanceChanged = true;
        if (!instance.gameObject.TryGetComponent<InteractableObject>(out var interactable)) { Interactable = null; return; }
        InteractableCollider = instance;
        Interactable = interactable;
    }
    
    public void Dispose()
    {
        _routineManager.StopUpdateAction(FindTarget);
        _rayOrigin = null;
        _rayOrigin = null;
        Interactable = null;
        _routineManager = null;
    }
}