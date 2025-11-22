using System;
using System.Collections;
using UnityEngine;

public class Print : BaseInteraction
{
    [SerializeField] private Printer printer;
    [SerializeField] private Transform printingSpot;
    private Coroutine _printRoutine;
    private bool _isUsing;
    
    public override bool IsRelevant(Collider colliderInfo)
    {
        if (_isUsing) return false;
        var config = printer.SelectedObject;
        if (config == null) return false;
        if (printingSpot.childCount > 0) return false;
        if (!base.IsRelevant(colliderInfo)) return false;
        if (config.objectData.Count == 0) return false;
        if (!config.objectData[0].prefab.TryGetComponent<IGrabable>(out _)) return false;
        UpdateUI(true);
        return true;
    }

    public override void PerformInteraction()
    {
        printer.SelectedObject.Spawn();
        var grabObject = printer.SelectedObject.GetInstances()[^1].GetComponent<IGrabable>();
        if (grabObject == null) throw new Exception("Printing object is not grabObject");
        grabObject.Grab(printingSpot);
        _printRoutine = G.GetManager<RoutineManager>().StartRoutine(PrintObject());
    }
    
    private IEnumerator PrintObject()
    {
        _isUsing = true;
        printer.GetComponent<Collider>().enabled = false;
        yield return new WaitUntil(() => printingSpot.childCount == 0);
        printer.GetComponent<Collider>().enabled = true;
        _isUsing = false;
        printer.SelectedObject = null;
        
    }
}
