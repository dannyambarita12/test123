using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseData : ScriptableObject
{
    public string Id => id;

    [TitleGroup("Base")]
    [ShowInInspector, ReadOnly, PropertyOrder(-7)]
    private string id;

    [TitleGroup("Base")]
    [OnValueChanged(nameof(RenameFileData)), PropertyOrder(-5)]
    public string Name;

#if UNITY_EDITOR
    [ButtonGroup("Base/Generate ID")]
    public void GenerateId(string id = "")
    {
        if(id != string.Empty)
        {
            this.id = id;
            return;
        }

        System.Random rnd = new System.Random();
        int myRandomNo = rnd.Next(1000, 9999);
        id = $"{RandomStringGenerator.GenerateRandomString(4)}-{myRandomNo}";
    }

    protected void RenameFileData()
    {
        this.name = Name;
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
    }
#endif
}
