using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonUIHandler : MonoBehaviour
{
    public Image trustIndicator;
    public Image fearIndicator;
    private Dictionary<FearSource, float> _accumulatedFearFromSource = new();
    
    public void InitializeUi(float defaultTrust, float defaultFear)
    {
        trustIndicator.fillAmount = defaultTrust;
        fearIndicator.fillAmount = defaultFear;
    }

    public void UpdateUiFear(float amount)
    {
        fearIndicator.fillAmount += amount;
    }
    
    public void UpdateUiTrust(float amount)
    {
        trustIndicator.fillAmount += amount;
    }
    
    public void DeInitializeUi() {}
}
