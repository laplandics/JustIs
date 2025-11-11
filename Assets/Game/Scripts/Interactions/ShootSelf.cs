using System.Collections;
using UnityEngine;

public class ShootSelf : MonoBehaviour, IInteractionPreset
{
    [SerializeField] private Sprite targetSprite;
    private Transform _cameraTr;
    private Transform _shootSelfPoint;
    private Transform _hand;
    private Coroutine _checkRoutine;
    private Revolver _revolver;
    private bool _isChecking;
    private int _layerMask;
    private bool _canBePerformed;
    
    public Sprite TargetSprite => targetSprite;
    
    public bool IsRelevant()
    {
        if (_isChecking) return true;
        var player = G.GetManager<PlayerManager>().GetPlayer();
        var hand = player.Hand;
        if (hand.childCount == 0) return false;
        var revolver = hand.GetChild(0).GetComponent<Revolver>();
        if (revolver == null) return false;
        
        _hand = hand;
        _revolver = revolver;
        _cameraTr = G.GetManager<CameraManager>().GetCameraTransform();
        _layerMask = ~G.GetManager<PlayerManager>().GetIgnoredLayers();
        _shootSelfPoint = G.GetManager<PlayerManager>().GetPlayer().ShootSelfPoint;
        
        UpdateUI(true);
        _checkRoutine = G.GetManager<RoutineManager>().StartRoutine(CheckConditions());
        return true;
    }

    private IEnumerator CheckConditions()
    {
        _isChecking = true;
        while (true)
        {
            var ray = new Ray(_cameraTr.position, _cameraTr.forward);
            if (!Physics.Raycast(ray, out var hit, 100f, _layerMask)) { _isChecking = false; Reset(); yield break; }
            if (!hit.collider.TryGetComponent<SuicidePlane>(out _)) { _isChecking = false; Reset(); yield break; }
            PrepareInteraction();
            yield return null;
        }
    }

    private void PrepareInteraction()
    {
        if (_canBePerformed) return;
        IGrabable grabRevolver = _revolver;
        grabRevolver.Grab(_shootSelfPoint);
        _canBePerformed = true;
    }

    private void ReleaseInteraction()
    {
        if (!_canBePerformed) return;
        IGrabable grabRevolver = _revolver;
        grabRevolver.Grab(_hand);
        _canBePerformed = false;
    }

    public void PerformInteraction()
    {
        if (!_canBePerformed) return;
        print($"{this} performed");
    }

    public void UpdateUI(bool showUI) => EventService.Invoke(new OnUpdateUIEvent {Sprite = showUI ? TargetSprite : null});
    
    public void Reset()
    {
        if (_isChecking) return;
        if (_checkRoutine != null) G.GetManager<RoutineManager>()?.EndRoutine(_checkRoutine);
        ReleaseInteraction();
        UpdateUI(false);
        _cameraTr = null;
        _shootSelfPoint = null;
        _hand = null;
        _checkRoutine = null;
        _revolver = null;
        _isChecking = false;
        _canBePerformed = false;
    }
}
