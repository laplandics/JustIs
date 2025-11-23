using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComputerUIHandler : MonoBehaviour
{
    [SerializeField] private Transform contentContainer;
    [SerializeField] private GameObject buttonPrefab;
    private readonly List<GameObject> _buttons = new();
    
    public void InitializeUi(Computer computer)
    {
        var currentStage = DataInjector.InjectState<CurrentGameStage>().Get();
        var stageObjects = currentStage.StageObjects;
        var unlockableObjects = stageObjects.Where(o => o.traits.Contains(ObjectTrait.Locked) || o.traits.Contains(ObjectTrait.Unlocked)).ToList();
        foreach (var unlockableObject in unlockableObjects)
        {
            var buttonObject = SpawnManager.Spawn(buttonPrefab, Vector3.zero, Quaternion.identity, contentContainer);
            buttonObject.transform.localPosition = Vector3.zero;
            buttonObject.transform.localRotation = Quaternion.identity;
            var buttonText = buttonObject.GetComponentInChildren<TMP_Text>();
            buttonText.text = unlockableObject.name;
            var button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => computer.MarkObject(unlockableObject));
            _buttons.Add(buttonObject);
        }
    }
    
    public void DeInitializeUi()
    {
        foreach (var button in _buttons) { SpawnManager.Despawn(button); }
        _buttons.Clear();
    }
}