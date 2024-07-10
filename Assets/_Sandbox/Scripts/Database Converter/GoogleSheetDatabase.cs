using Cathei.BakingSheet;
using Cathei.BakingSheet.Unity;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class GoogleSheetDatabase
{
    [MenuItem("Database/Import Cards From Google Sheet")]
    public static async void ConvertFromGoogle()
    {
        string googleSheetId = "1junNYSfrEBTdP-kV5rbFDFHpefKp_cq9isS55pJbFRM";
        string googleCredential = File.ReadAllText("Assets/_credentials/anigma-ruina-bdd7bf4962ea.json");
        var googleConverter = new GoogleSheetConverter(googleSheetId, googleCredential);

        var sheetContainer = new DataSheetContainer(UnityLogger.Default);
        
        await sheetContainer.Bake(googleConverter);

        var exporter = new ScriptableObjectSheetExporter("Assets/_Productions/Database/Cards/Data");

        await sheetContainer.Store(exporter);

        AssetDatabase.Refresh();

        Debug.Log("Google sheet converted.");
    }
}

