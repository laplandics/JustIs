using System.Collections;
using System.Collections.Generic;
using ConfigEvents;
using UnityEngine;

public class AncestorsStatue : InteractableObject, IExaminable, IGrabable
{
    [Header("Grab settings")]
    [SerializeField] private Collider interactionCollider;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform holdPoint;
    public Collider InteractionCollider => interactionCollider;
    public Rigidbody Rb => rb;
    public Transform HoldPoint => holdPoint;
    
    [Header("Examine settings")]
    [SerializeField] private Canvas ui;
    [SerializeField] private Transform textContainer;
    [SerializeField] private Transform visual;
    public Canvas UI => ui;
    public Transform TextContainer => textContainer;
    public Transform Visual => visual;

    private bool _isExamining;
    private bool _examinedOnce;

    protected override void Launch() { ExamineStory.RegisterStoryObject(GetType()); }

    public void Examine()
    {
        ui.gameObject.SetActive(true);
        _isExamining = true;
        if (!_examinedOnce) { _examinedOnce = true; EventService.Invoke(new AncestorsStatue_ExaminedEvent()); }
        G.GetManager<RoutineManager>().StartRoutine(ExamineRoutine());
    }

    public IEnumerator ExamineRoutine()
    {
        var lines = ExamineStory.GetStoryLines(GetType());
        var uiTr = textContainer.transform;
        var textBlocks = new List<TextBlock>();
        foreach (var line in lines)
        {
            var textBlock = new TextBlock(line);
            textBlock.SpawnElements(uiTr);
            textBlocks.Add(textBlock);
        }
        yield return new WaitUntil(() => !_isExamining);
        foreach (var textBlock in textBlocks) { textBlock.DespawnElements(); }
    }
    
    public void Release()
    {
        _isExamining = false;
        ui.gameObject.SetActive(false);
    }

    public void Grab(Transform parent)
    {
        Rb.isKinematic = true;
        var tr = transform;
        tr.SetParent(parent);
        tr.position = parent.position;
        tr.localScale = Vector3.one;
        tr.localPosition -= HoldPoint.localPosition;
        tr.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
    }

    public void Release(Transform parent, Vector3 position)
    {
        interactionCollider.enabled = true;
        var tr = transform;
        tr.SetParent(parent);
        tr.position = position;
        var cameraTr = G.GetManager<CameraManager>().GetCameraTransform();
        var cameraPos = new Vector3(cameraTr.position.x, position.y, cameraTr.position.z);
        tr.rotation = Quaternion.LookRotation(cameraPos - position);
        tr.localScale = Vector3.one;
        Rb.isKinematic = false;
    }

    public override void Disable()
    {
        ExamineStory.UnregisterStoryObject(GetType());
        base.Disable();
    }
}