using System;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class TestSceneSaver : MonoBehaviour
{
    public SceneState[] states;
    
    [Serializable]
    public class SceneState
    {
        public GameObject objectOnScene;
        public StageObject stageObject;
    }

    [Button]
    private void SaveSceneState()
    {
        foreach (var state in states)
        {
            if (state.objectOnScene == null) continue;
            state.stageObject.position = state.objectOnScene.transform.position;
            state.stageObject.rotation = state.objectOnScene.transform.rotation;
            state.stageObject.scale = state.objectOnScene.transform.localScale;
            EditorUtility.SetDirty(state.stageObject);
            AssetDatabase.SaveAssets();
        }    
    }
}