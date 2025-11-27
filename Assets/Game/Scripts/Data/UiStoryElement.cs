using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class UiStoryElement
{
    protected List<string> Elements { get; set; }
    protected List<GameObject> Instances;
    public virtual void SpawnElements(Transform ui) {}
    public virtual void DespawnElements() {}
}

public class TextBlock : UiStoryElement
{
    public TextBlock(List<string> texts) { Elements = texts; }
    
    public override void SpawnElements(Transform ui)
    {
        Instances = new List<GameObject>();
        var texts = G.GetManager<UiManager>().SpawnExamineTexts(Elements, ui);
        for (var i = 0; i < Elements.Count; i++) { texts[i].text = Elements[i]; Instances.Add(texts[i].gameObject); }
    }

    public override void DespawnElements()
    {
        G.GetManager<UiManager>().DespawnExamineElements(Instances);
        Instances.Clear();
    }
}

public class Choice : UiStoryElement
{
    private readonly Dictionary<string, Action> _choices;
    private GameObject _container;
    
    public Choice(Dictionary<string, Action> choices)
    {
        Elements = choices.Keys.ToList();
        _choices = choices;
    }
    public override void SpawnElements(Transform ui)
    {
        Instances = new List<GameObject>();
        var choiceButtons = G.GetManager<UiManager>().SpawnExamineChoices(Elements, ui, out _container);
        foreach (var button in choiceButtons)
        {
            var choiceButtonText = button.GetComponentInChildren<TMP_Text>().text;
            button.onClick.AddListener(_choices[choiceButtonText].Invoke);
            Instances.Add(button.gameObject);
        }
    }
    public override void DespawnElements()
    {
        G.GetManager<UiManager>().DespawnExamineElements(Instances);
        G.GetManager<UiManager>().DespawnExamineElement(_container);
    }
}

public class Next : UiStoryElement
{
    public override void SpawnElements(Transform ui) {}
}