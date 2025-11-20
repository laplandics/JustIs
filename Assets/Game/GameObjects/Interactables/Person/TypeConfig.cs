using UnityEditor;
using UnityEngine;

public abstract class TypeConfig : ScriptableObject
{
    [SerializeField] protected SpecialGameState unlockState;
    private SpecialGameState _asset;
    private string _path;
    
    public virtual bool IsValid() { return false; }

    public void CreateAsset(string folderPath, string stageNum, string index)
    {
        if (unlockState == null) return;
        if (_path != null) return;
        _path = folderPath + "/" + stageNum + "_" + "GSTypeCnf" + "_" + index + ".asset";
        _asset = CreateInstance(unlockState.GetType()) as SpecialGameState;
        if (_asset == null) return;
        AssetDatabase.CreateAsset(_asset, _path);
        AssetDatabase.SaveAssets();
        unlockState = _asset;
    }

    public void DeleteAsset()
    {
        if (unlockState == null) return;
        AssetDatabase.DeleteAsset(_path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        _path = null;
    }
    
    public virtual void PerformConfiguration(Person person) {}
}