using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UiStoryElement
{
    protected List<string> Elements { get; set; }
    protected List<GameObject> Instances;
    public virtual void SpawnElements(Transform container) {}
    public virtual void DespawnElements() {}
}

public class TextBlock : UiStoryElement
{
    public TextBlock(List<string> texts) { Elements = texts; }
    
    public override void SpawnElements(Transform container)
    {
        Instances = new List<GameObject>();
        var texts = G.GetManager<UiManager>().SpawnExamineTexts(Elements, container);
        foreach (var text in texts) { Instances.Add(text.gameObject); }
    }

    public override void DespawnElements()
    {
        G.GetManager<UiManager>().DespawnExamineElements(Instances);
        Instances.Clear();
    }
}

public class Choice : UiStoryElement
{
    private readonly Dictionary<string, Action<Button>> _choices;
    
    public Choice(Dictionary<string, Action<Button>> choices)
    {
        Elements = choices.Keys.ToList();
        _choices = choices;
    }
    public override void SpawnElements(Transform container)
    {
        Instances = new List<GameObject>();
        var choiceButtons = G.GetManager<UiManager>().SpawnExamineChoices(Elements, container);
        foreach (var button in choiceButtons)
        {
            Instances.Add(button.gameObject);
            var buttonText = button.GetComponentInChildren<TMP_Text>().text;
            button.onClick.AddListener(() => _choices[buttonText].Invoke(button));
        }
    }
    public override void DespawnElements()
    {
        G.GetManager<UiManager>().DespawnExamineElements(Instances);
    }
}

public class Next : UiStoryElement
{
    public override void SpawnElements(Transform container) {}
}