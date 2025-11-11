using UnityEngine;
using UnityEngine.UI;

public class TabletUi : MonoBehaviour
{
    [SerializeField] private Button button;
    
    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        Debug.Log("OnButtonClick");
    }
}
