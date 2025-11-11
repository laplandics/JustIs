using UnityEngine;

public class AncestorsStatue : InteractableObject, IExaminable
{
    [SerializeField] private Canvas examineUi;
    [SerializeField] private Transform visual;
    
    public Canvas ExamineUi => examineUi;
    public Transform Visual => visual;
    public void Examine()
    {
        examineUi.gameObject.SetActive(true);
    }

    public void Release()
    {
        examineUi.gameObject.SetActive(false);
    }
}