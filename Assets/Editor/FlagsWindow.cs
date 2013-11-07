using UnityEngine;
using UnityEditor;
using System.Collections;

public class FlagsWindow : EditorWindow
{

    static DialogDataMono dialog;

    string newGlobalFlag = "";
    string newRegionFlag = "";

    [MenuItem("Dialog/Flags Window")]
    static void ShowFlagsWindow()
    {
        GameObject temp = GameObject.Find("DialogData");
        if (temp == null)
        {
            Debug.LogError("Failed to find dialogData object, creating a new one");
            temp = new GameObject();
            temp.name = "DialogData";
            temp.AddComponent<DialogDataMono>();
            dialog = temp.GetComponent<DialogDataMono>();
        }
        else
        {
            dialog = temp.GetComponent<DialogDataMono>();
            if (dialog == null)
            {
                Debug.LogError("Found dialogData, but failed to get DialogDataMono component.  Fix this!");
            }
        }
        FlagsWindow window = (FlagsWindow)EditorWindow.GetWindow(typeof(FlagsWindow));
        window.Repaint();
    }

    void OnInspectorUpdate()
    {
        try
        {
            dialog = GameObject.Find("DialogData").GetComponent<DialogDataMono>();
            
        }
        catch
        {
            //do nothing
        }
        Repaint();
    }


    void OnGUI()
    {
        try
        {
            if (dialog == null) dialog = GameObject.Find("DialogData").GetComponent<DialogDataMono>();

            GUILayout.BeginVertical();
            GUILayout.Label("Global Flags");
            foreach (DialogFlag flag in dialog.dialogData.globalFlags)
            {
                GUILayout.BeginHorizontal();
                flag.flag = GUILayout.TextField(flag.flag);

                if (GUILayout.Button("X"))
                {

                }
            }
            newGlobalFlag = GUILayout.TextField(newGlobalFlag);
            if (GUILayout.Button("Add Global Flag"))
            {
                if (newGlobalFlag.Trim().Length > 0)
                {
                    newGlobalFlag = newGlobalFlag.Split(' ')[0];
                    DialogFlag newFlag = new DialogFlag(newGlobalFlag);
                    dialog.dialogData.globalFlags.Add(newFlag);
                    newGlobalFlag = "";
                }
            }
            GUILayout.Label("Region Flags");
            foreach (DialogFlag flag in dialog.dialogData.regionFlags)
            {
                flag.flag = GUILayout.TextField(flag.flag);
            }
            newRegionFlag = GUILayout.TextField(newRegionFlag);
            if (GUILayout.Button("Add Region Flag"))
            {
                if (newRegionFlag.Trim().Length > 0)
                {
                    newRegionFlag = newRegionFlag.Split(' ')[0];
                    DialogFlag newFlag = new DialogFlag(newRegionFlag);
                    dialog.dialogData.regionFlags.Add(newFlag);
                    newRegionFlag = "";
                }
            }
            GUILayout.EndVertical();
        }
        catch
        {
            GUILayout.Label("Unable to find DialogData");
        }
    }
}
