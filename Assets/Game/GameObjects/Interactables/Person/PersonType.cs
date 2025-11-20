using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "[StageNum]PT", menuName = "GameData/PersonType")]
public class PersonType : ScriptableObject
{
    public StageNum stageNum;
    public string path;
    public TypeConfig[] personTypeConfigs;

    [Button]
    private void CreateConfigAssets()
    {
        var index = 0;
        foreach (var config in personTypeConfigs) { config.CreateAsset(path, stageNum.ToString(), index.ToString()); index++; }
    }

    [Button]
    private void DeleteConfigAssets()
    {
        foreach (var config in personTypeConfigs) { config.DeleteAsset(); }
    }
}