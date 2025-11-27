using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonMoodHandler : MonoBehaviour
{
    private Person _person;
    private PersonType _currentPersonType;
    private readonly Dictionary<MoodChangeSource, float> _moodSources = new();
    private ConfigEvents.Person_MoodChangedEvent _internalMoodEvent = new();
    private float _currentMood;
    private bool _isInitialized;
    private Coroutine _defaultMoodChangeRoutine;

    public void Initialize()
    {
        _isInitialized = true;
        _person = GetComponent<Person>();
        _currentPersonType = _person.CurrentPersonType;
        _currentMood = _currentPersonType.defaultMood;
        EventService.Subscribe<ConfigEvents.Person_MoodChangedEvent>(UpdateMood);
        DataInjector.InjectState<CurrentPersonMood>().Set(_person, _currentMood);
        _defaultMoodChangeRoutine = G.GetManager<RoutineManager>().StartRoutine(DefaultMoodChangeRoutine());
    }

    private IEnumerator DefaultMoodChangeRoutine()
    {
        while (_isInitialized)
        {
            var difference = _person.CurrentPersonType.defaultMood - _currentMood;
            var noDiff = difference <= 0.001f;
            SendNewMoodDirectly(difference, MoodChangeSource.DefaultChange, !noDiff);
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    public void SendNewMoodDirectly(float amount, MoodChangeSource source, bool useMult = true)
    {
        var rightAmount = amount * _currentPersonType.GetMoodSetting(source).impactMultiplier;
        _internalMoodEvent.Person = _person;
        _internalMoodEvent.Amount = useMult ? rightAmount : amount;
        _internalMoodEvent.Source = source;
        EventService.Invoke(_internalMoodEvent);
    }
    
    private void UpdateMood(ConfigEvents.Person_MoodChangedEvent eventData)
    {
        if (eventData.Person != _person) return;
        var source = eventData.Source;
        var amount = eventData.Amount;
        if (!_moodSources.TryAdd(source, amount))
        {
            if (Math.Abs(_moodSources[source]) >= _currentPersonType.GetMoodSetting(source).maxAmount) return;
            _moodSources[source] += amount;
        }
        _currentMood += eventData.Amount;
        _person.UIHandler.UpdateUiMood(_currentMood);
        DataInjector.InjectState<CurrentPersonMood>().Set(_person, _currentMood);
    }

    public void Deinitialize()
    {
        _isInitialized = false;
        if (_defaultMoodChangeRoutine != null) G.GetManager<RoutineManager>().EndRoutine(_defaultMoodChangeRoutine);
        _currentPersonType = null;
        _person = null;
        _moodSources.Clear();
        _internalMoodEvent = new ConfigEvents.Person_MoodChangedEvent();
        EventService.Unsubscribe<ConfigEvents.Person_MoodChangedEvent>(UpdateMood);
    }
}