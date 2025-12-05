using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "[TypeName]_PT", menuName = "GameData/PersonType")]
public class PersonType : ScriptableObject
{
    public StageNum typeStage;
    public TriggerAction[] triggers;
    private Person _person;
    
    [Header("Examine stories")]
    [SerializeField] private StoryData[] examineStories;
    public StoryData[] ExamineStories => examineStories;
    
    [Header("Mood settings")]
    public float defaultMood;
    [SerializeField] private SourceMoodSettings[] moodSources;
    private readonly Dictionary<MoodChangeSource, SourceMoodSettings> _moodSettings = new();
    
    public void Initialize(Person person)
    {
        _person = person;
        foreach (var source in moodSources) {_moodSettings.TryAdd(source.source, source);}
        foreach (var trigger in triggers) { trigger.targetPerson = _person; }
        EventService.Subscribe<StateEvents.OnSomeStateChangedEvent>(CatchSomeStateInvocation);
    }

    private void CatchSomeStateInvocation(StateEvents.OnSomeStateChangedEvent eventData)
    {
        foreach (var trigger in triggers) { if (trigger.state == eventData.State) trigger.PrepareToPerform(); }
    }
    
    public SourceMoodSettings GetMoodSetting(MoodChangeSource source) => _moodSettings[source];

    public void Deinitialize()
    {
        _moodSettings.Clear();
        _person = null;
        foreach (var trigger in triggers) { trigger.Reset(); }
        EventService.Unsubscribe<StateEvents.OnSomeStateChangedEvent>(CatchSomeStateInvocation);
    }
}

[Serializable]
public class SourceMoodSettings
{
    public MoodChangeSource source;
    public float impactMultiplier;
    public float maxAmount;
}

public enum MoodChangeSource
{
    DefaultChange,
    WeaponAim
}