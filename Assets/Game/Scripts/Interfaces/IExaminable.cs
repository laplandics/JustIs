using System.Collections;
using UnityEngine;

public interface IExaminable
{
    public Canvas UI { get; }
    public Transform Visual { get; }
    public Transform TextContainer { get; }
    
    public void Examine();
    public IEnumerator ExamineRoutine();
    public void Release();
}