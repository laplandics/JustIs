using System.Collections;
using UnityEngine;

public class Print : BaseInteraction
{
    [SerializeField] private Printer printer;
    [SerializeField] private Transform printingSpot;
    private Coroutine _printingRoutine;
    private GameObject _printingStageObject;
    private bool _isUsing;
    
    public override bool IsRelevant(Collider colliderInfo)
    {
        return false;
    }

    public override void PerformInteraction()
    {
        
    }

    public override void Reset()
    {
        
    }
}
