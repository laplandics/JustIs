using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ExamineStory
{
    private static readonly Dictionary<Type, StoryObject> StoryObjects = new();

    public static void RegisterStoryObject(Type storyReceiver)
    {
        if (!StoryObjects.TryAdd(storyReceiver, null)) return;
        var storyObjects = Assembly.GetAssembly(typeof(StoryObject)).GetTypes().Where(t => t.IsSubclassOf(typeof(StoryObject))).ToList();
        var instances = storyObjects.Select(type => (StoryObject)Activator.CreateInstance(type)).ToList();
        foreach (var storyObject in instances) { if (storyObject.GetReceiverType == storyReceiver) StoryObjects[storyReceiver] = storyObject; }
        StoryObjects[storyReceiver].Register();
    }
    
    public static List<List<string>> GetStoryLines(Type storyReceiver) => StoryObjects[storyReceiver].GetStoryLines();

    public static void UnregisterStoryObject(Type storyReceiver)
    {
        if(!StoryObjects.TryGetValue(storyReceiver, out var storyObject)) return;
        storyObject.Unregister();
        StoryObjects.Remove(storyReceiver);
    }
}

public abstract class StoryObject
{
    protected readonly  List<List<string>> StoryLines = new();
    public virtual Type GetReceiverType => null;
    
    public virtual void Register() {}
    public  List<List<string>> GetStoryLines() => StoryLines;
    public virtual void Unregister() {}
}

public class AncestorsStory : StoryObject
{
    public override Type GetReceiverType => typeof(AncestorsStatue);

    public override void Register()
    {
        EventService.Subscribe<ConfigEvents.AncestorsStatue_ExaminedEvent>(SetFirstExamineLines);
    }

    private void SetFirstExamineLines(ConfigEvents.AncestorsStatue_ExaminedEvent _)
    {
        var newLines = new List<string> {Story.Ancestors.FirstExamine1, Story.Ancestors.FirstExamine2, Story.Ancestors.FirstExamine3};
        StoryLines.Add(newLines);
        EventService.Unsubscribe<ConfigEvents.AncestorsStatue_ExaminedEvent>(SetFirstExamineLines);
    }

    public override void Unregister()
    {
        EventService.Unsubscribe<ConfigEvents.AncestorsStatue_ExaminedEvent>(SetFirstExamineLines);
    }
}

public class PersonStory : StoryObject
{
    public override Type GetReceiverType => typeof(Person);

    public override void Register()
    {
        EventService.Subscribe<ConfigEvents.Person_NewPersonTypeChosen>(SetFirstMetLines);
    }

    private void SetFirstMetLines(ConfigEvents.Person_NewPersonTypeChosen eventData)
    {
        StoryLines.Add(PersonType.FirstMetLines);
        EventService.Unsubscribe<ConfigEvents.Person_NewPersonTypeChosen>(SetFirstMetLines);
    }

    public override void Unregister()
    {
        EventService.Unsubscribe<ConfigEvents.Person_NewPersonTypeChosen>(SetFirstMetLines);
    }
}