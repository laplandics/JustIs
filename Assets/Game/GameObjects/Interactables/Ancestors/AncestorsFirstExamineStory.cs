using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AncestorsFirstExamine", menuName = "GameData/Story/Ancestors/FirstExamine")]
public class AncestorsFirstExamineStory : StoryData
{
    public override bool GetStatus(IExaminable target)
    {
        Target = target;
        if (!DataInjector.InjectState<IsAncestorsStatueExamined>().Get()) return false;
        SetInitialStory();
        return true;
    }

    private void SetInitialStory()
    {
        var textLine1 = Story.Ancestors.FirstExamineText1;
        var textLine2 = Story.Ancestors.FirstExamineText2;
        var textLine3 = Story.Ancestors.FirstExamineText3;
        var choiceLine1 = Story.Ancestors.FirstExamineChoice1;
        var choiceLine2 = Story.Ancestors.FirstExamineChoice2;
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

    private void SetExamineStory()
    {
        var textLine1 = Story.Ancestors.FirstExamineAfterChoice1Text1;
        var choiceLine1 = Story.Ancestors.FirstExamineAfterChoice1Choice1;
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
}