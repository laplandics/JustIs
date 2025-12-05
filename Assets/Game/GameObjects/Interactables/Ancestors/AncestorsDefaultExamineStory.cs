using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Story.Ancestors;

[CreateAssetMenu(fileName = "AncestorsDefaultExamine", menuName = "GameData/Story/Ancestors/DefaultExamine")]
public class AncestorsDefaultExamineStory : StoryData
{
    [SerializeField] private bool isAlreadySeen;
    
    public override bool GetStatus(IExaminable target)
    {
        Target = target;
        if (!DataInjector.InjectState<IsAncestorsStatueExamined>().Get()) return false;
        if (isAlreadySeen) SetAltInitialStory();
        else SetInitialStory();
        isAlreadySeen = true;
        return true;
    }

    private void SetInitialStory()
    {
        var textLine1 = DefaultExamineText1;
        var textLine2 = DefaultExamineText2;
        var textLine3 = DefaultExamineText3;
        var choiceLine1 = DefaultExamineChoice1;
        var choiceLine2 = DefaultExamineChoice2;
        var choices = new Dictionary<string, Action<Button>>
        {
            [choiceLine1] = Examine,
            [choiceLine2] = Release
        };
        StoryElements = new UiStoryElement[]
        {
            new TextBlock(new List<string> {textLine1, textLine2, textLine3}),
            new Choice(choices)
        };
    }

    private void SetAltInitialStory()
    {
        var textLine1 = DefaultExamineText1Alt;
        var choiceLine1 = DefaultExamineChoice1;
        var choiceLine2 = DefaultExamineChoice2;
        var choices = new Dictionary<string, Action<Button>>
        {
            [choiceLine1] = Examine,
            [choiceLine2] = Release
        };
        StoryElements = new UiStoryElement[]
        {
            new TextBlock(new List<string> {textLine1}),
            new Choice(choices)
        };
    }

    private void SetExamineStory()
    {
        var textLine1 = DefaultExamineAfterChoice1Text1;
        var choiceLine1 = DefaultExamineAfterChoice1Choice1;
        var choices = new Dictionary<string, Action<Button>>
        {
            [choiceLine1] = GoBack,
        };
        StoryElements = new UiStoryElement[]
        {
            new TextBlock(new List<string> { textLine1 }),
            new Choice(choices)
        };
    }

    private void Release(Button button)
    {
        EventService.Invoke(new UiEvents.OnUiInteractionEnded());
    }

    private void Examine(Button button)
    {
        button.interactable = false;
        SetExamineStory();
        var storyEvent = new ExamineEvents.OnStoryChoiceMadeEvent.StoryPathBegin { Target = Target, Path = this };
        EventService.Invoke(new ExamineEvents.OnStoryChoiceMadeEvent { StoryEvent = storyEvent });
    }

    private void GoBack(Button button)
    {
        button.interactable = false;
        var storyEvent = new ExamineEvents.OnStoryChoiceMadeEvent.StoryPathEnd();
        EventService.Invoke(new ExamineEvents.OnStoryChoiceMadeEvent { StoryEvent =  storyEvent });
    }

    public override void ResetStoryState()
    {
        isAlreadySeen = false;
        base.ResetStoryState();
    }
}