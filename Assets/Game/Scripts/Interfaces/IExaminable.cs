using UnityEngine;

public interface IExaminable
{
    public Canvas ExamineUi { get; }
    public Transform Visual { get; }
    
    public void Examine();
    public void Release();
}