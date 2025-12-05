using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExamineStoryRoutine : IDisposable
{
    private Transform _buttonsContainer;
    private Transform _examineContainer;
    private StoryData[] _examineStories;
    private IExaminable _examinable;
    private StoryData _specificStoryPath;
    private bool _isExaminingEnd;
    private bool _isSpecificPathEnd;

    private Transform _lastButtonsUi;
    private List<UiStoryElement> _existingElements;
    
    public ExamineStoryRoutine(Transform buttonsContainer, Transform examineContainer, StoryData[] stories, IExaminable examinable)
    {
        _buttonsContainer = buttonsContainer;
        _examineContainer = examineContainer;
        _examineStories = stories;
        _examinable = examinable;
    }

    public void StartExamine()
    {
        _isExaminingEnd = false;
        EventService.Subscribe<ExamineEvents.OnStoryChoiceMadeEvent>(SetSpecificPath);
        G.GetManager<RoutineManager>().StartRoutine(ExamineRoutine());
    }

    private void SetSpecificPath(ExamineEvents.OnStoryChoiceMadeEvent eventData)
    {
        switch (eventData.StoryEvent)
        {
            case ExamineEvents.OnStoryChoiceMadeEvent.StoryPathBegin beginPathEvent when beginPathEvent.Target != _examinable:
                return;
            case ExamineEvents.OnStoryChoiceMadeEvent.StoryPathBegin beginPathEvent:
                _isSpecificPathEnd = false;
                _specificStoryPath = beginPathEvent.Path;
                break;
            case ExamineEvents.OnStoryChoiceMadeEvent.StoryPathEnd endPathEvent:
                _isSpecificPathEnd = true;
                _specificStoryPath = null;
                break;
        }
        ClearButtons(_lastButtonsUi, _examineContainer);
    }
    
    private IEnumerator ExamineRoutine()
    {
        _existingElements = new List<UiStoryElement>();
        ExamineStart:
        var storyElements = new List<UiStoryElement>();
        foreach (var story in _examineStories)
        {
            var getStory = new List<UiStoryElement>();
            if(!story.GetStatus(_examinable)) continue;
            getStory.AddRange(story.GetStoryElements());
            storyElements.AddRange(getStory);
            _existingElements.AddRange(getStory);
        }
        
        var textUi = _examineContainer;
        foreach (var storyElement in storyElements) {if(storyElement is TextBlock textElement) textElement.SpawnElements(textUi);}
        var buttonsUi = SpawnManager.Spawn(_buttonsContainer, Vector3.zero, Quaternion.identity, _examineContainer);
        _lastButtonsUi = buttonsUi;
        foreach (var storyElement in storyElements) {if(storyElement is Choice choiceElement) choiceElement.SpawnElements(buttonsUi);}

        _isSpecificPathEnd = false;
        while (!_isExaminingEnd && !_isSpecificPathEnd)
        {
            if (_specificStoryPath == null) { yield return null; continue; }
            var specificStoryElements = _specificStoryPath.GetStoryElements();
            _existingElements.AddRange(specificStoryElements);
            foreach (var storyElement in specificStoryElements) {if(storyElement is TextBlock textElement) textElement.SpawnElements(textUi);}
            var newButtonsUi = SpawnManager.Spawn(_buttonsContainer, Vector3.zero, Quaternion.identity, _examineContainer);
            _lastButtonsUi = newButtonsUi;
            foreach (var storyElement in specificStoryElements) {if(storyElement is Choice choiceElement) choiceElement.SpawnElements(newButtonsUi);}
            _specificStoryPath = null;
        }
        
        if(_isSpecificPathEnd && !_isExaminingEnd) { goto ExamineStart; }
        foreach (var element in _existingElements) element.DespawnElements();
        if (buttonsUi == null) yield break;
        SpawnManager.Despawn(buttonsUi.gameObject);
    }

    private void ClearButtons(Transform buttonsUi, Transform textUi)
    {
        var chosenText = string.Empty;
        foreach (var button in buttonsUi.GetComponentsInChildren<Button>())
        {
            if (button.interactable) continue;
            chosenText = button.GetComponentInChildren<TMP_Text>().text;
        }
        SpawnManager.Despawn(buttonsUi.gameObject);
        var newTextBlock = new TextBlock(new List<string> { chosenText });
        newTextBlock.SpawnElements(textUi);
        _existingElements.Add(newTextBlock);
    }

    public void StopExamine()
    {
        _isExaminingEnd = true;
        EventService.Unsubscribe<ExamineEvents.OnStoryChoiceMadeEvent>(SetSpecificPath);
    }

    public void Dispose()
    {
        _buttonsContainer = null;
        _examineContainer = null;
        _examineStories = null;
        _examinable = null;
        _isExaminingEnd = false;
        _isSpecificPathEnd = false;
        _specificStoryPath = null;
        EventService.Unsubscribe<ExamineEvents.OnStoryChoiceMadeEvent>(SetSpecificPath);
    }
}