using UnityEngine;

public abstract class StoryData : ScriptableObject
{
    protected UiStoryElement[] StoryElements;
    protected IExaminable Target;

    public abstract bool GetStatus(IExaminable target);
    
    public UiStoryElement[] GetStoryElements() => StoryElements;
}