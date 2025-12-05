using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour, ISceneManager
{
    [SerializeField] private Canvas canvasPrefab;
    [SerializeField] private Transform canvasParent;
    
    [Header("Examine UI Elements")]
    [SerializeField] private GameObject examineTextPrefab;
    [SerializeField] private GameObject examineChoiceButtonPrefab;
    
    private InteractableObject _interactable;
    private Canvas _canvas;
    private UiTarget _uiTarget;

    public void Initialize()
    {
        _canvas = SpawnManager.Spawn(canvasPrefab, Vector3.zero, Quaternion.identity, canvasParent);
        var targetPoint = _canvas.GetComponentInChildren<Image>();
        _uiTarget = new UiTarget(targetPoint);
        DataInjector.InjectState<GlobalCanvasSpawned>().Set(_canvas);
    }

    public static void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public List<TMP_Text> SpawnExamineTexts(List<string> texts, Transform parent)
    {
        var result = new List<TMP_Text>();
        foreach (var text in texts)
        {
            var textObject = SpawnManager.Spawn(examineTextPrefab, Vector3.zero, Quaternion.identity, parent);
            textObject.transform.localPosition = Vector3.zero;
            var tmpText = textObject.GetComponent<TMP_Text>();
            tmpText.text = text;
            result.Add(tmpText);
        }
        return result;
    }

    public List<Button> SpawnExamineChoices(List<string> choices, Transform container)
    {
        var choiceButtons = new List<Button>();
        foreach (var choice in choices)
        {
            var choiceObject = SpawnManager.Spawn(examineChoiceButtonPrefab, Vector3.zero, Quaternion.identity, container);
            choiceObject.transform.localPosition = Vector3.zero;
            var button = choiceObject.GetComponent<Button>();
            choiceButtons.Add(button);
            choiceObject.GetComponent<TMP_Text>().text = choice;
        }
        return choiceButtons;
    }

    public void DespawnExamineElements(List<GameObject> elements)
    {
        foreach (var element in elements) { SpawnManager.Despawn(element); }
        elements.Clear();
    }
    
    public void Deinitialize()
    {
        
        _uiTarget.Dispose();
        _uiTarget = null;
        _canvas = null;
    }
}