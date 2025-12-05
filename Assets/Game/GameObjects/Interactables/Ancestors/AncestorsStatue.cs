using UnityEngine;

public class AncestorsStatue : InteractableObject, IExaminable, IGrabable
{
    [Header("Grab settings")]
    [SerializeField] private Collider interactionCollider;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform holdPoint;
    
    [Header("Examine settings")]
    [SerializeField] private Canvas ui;
    [SerializeField] private Transform visual;
    [SerializeField] private Transform examineContainer;
    [SerializeField] private Transform buttonsContainer;
    [SerializeField] private StoryData[] examineStories;
    private ExamineStoryRoutine _storyRoutine;

    protected override void Launch()
    {
        _storyRoutine = new ExamineStoryRoutine(buttonsContainer, examineContainer, examineStories, this);
    }

    public void Examine()
    {
        ui.gameObject.SetActive(true);
        G.GetService<SpecialGameStatesService>().GetState<IsAncestorsStatueExamined>().Set(true);
        _storyRoutine.StartExamine();
    }

    public void Release()
    {
        ui.gameObject.SetActive(false);
        G.GetService<SpecialGameStatesService>().GetState<IsAncestorsStatueExamined>().Set(false);
        _storyRoutine.StopExamine();
    }

    public void Grab(Transform parent)
    {
        rb.isKinematic = true;
        var tr = transform;
        tr.SetParent(parent);
        tr.position = parent.position;
        tr.localScale = Vector3.one;
        tr.localPosition -= holdPoint.localPosition;
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
        rb.isKinematic = false;
    }

    public override void Disable()
    {
        _storyRoutine.Dispose();
        base.Disable();
    }
}