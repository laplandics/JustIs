using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Story.Person;

[CreateAssetMenu(fileName = "CowardlyPersonFirstMet", menuName = "GameData/Story/Person/CowardlyType/FirstMet")]
public class CowardlyPersonFirstMetStory : StoryData
{
    public override bool GetStatus(IExaminable target)
    {
        Target = target;
        if (!DataInjector.InjectState<TalkingWithPerson>().Get<bool>()) return false;
        SetInitialStory();
        return true;
    }

    private void SetInitialStory()
    {
        var textLine1 = FirstMeetText1;
        var choiceLine1 = FirstMeetChoice1;
        var choiceLine2 = FirstMeetChoice2;
        var choices = new Dictionary<string, Action<Button>>
        {
            [choiceLine1] = SayHi,
            [choiceLine2] = Leave
        };
        StoryElements = new UiStoryElement[]
        {
            new TextBlock(new List<string> {textLine1}),
            new Choice(choices)
        };
    }

    private void SayHiStory()
    {
        var textLine1 = FirstMeetAfterChoice1Text1;
        var choiceLine1 = FirstMeetChoice2;
        var choices = new Dictionary<string, Action<Button>>
        {
            [choiceLine1] = Leave
        };
        StoryElements = new UiStoryElement[]
        {
            new TextBlock(new List<string> {textLine1}),
            new Choice(choices)
        };
    }

    private void SayHi(Button button)
    {
        button.interactable = false;
        SayHiStory();
        var storyEvent = new ExamineEvents.OnStoryChoiceMadeEvent.StoryPathBegin { Target = Target, Path = this };
        EventService.Invoke(new ExamineEvents.OnStoryChoiceMadeEvent {StoryEvent = storyEvent});
    }

    private void Leave(Button button)
    {
        EventService.Invoke(new UiEvents.OnUiInteractionEnded());
    }
}