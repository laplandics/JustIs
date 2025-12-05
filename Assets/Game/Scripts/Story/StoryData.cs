using UnityEngine;

public abstract class StoryData : ScriptableObject
{
    protected UiStoryElement[] StoryElements;
    protected IExaminable Target;

    public abstract bool GetStatus(IExaminable target);
    
    public UiStoryElement[] GetStoryElements() => StoryElements;

    public virtual void ResetStoryState()
    {
        StoryElements = null;
        Target = null;
    }
}