using UnityEngine;
using UnityEngine.UI;

public class PersonUIHandler : MonoBehaviour
{
    public Image trustIndicator;
    
    public void InitializeUi() { trustIndicator.fillAmount = GetComponent<Person>().CurrentPersonType.defaultMood; }

    public void UpdateUiMood(float currentTrust) { trustIndicator.fillAmount = currentTrust; }

    public void DeInitializeUi() { }
}
