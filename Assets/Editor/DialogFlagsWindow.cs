using UnityEngine;
using UnityEditor;
using System.Collections;

public class DialogFlagsWindow : EditorWindow
{

    static DialogDataMono dialog;

    string newGlobalFlag = "";
    string newRegionFlag = "";

    Vector2 scrollPos;

    [MenuItem("Dialog/Flags Editor")]
    static void ShowFlagsWindow()
    {
        GameObject temp = GameObject.Find("DialogData");
        if (temp == null)
        {
            Debug.LogWarning("DialogData not found, creating a new one");
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
        DialogFlagsWindow window = (DialogFlagsWindow)EditorWindow.GetWindow(typeof(DialogFlagsWindow));
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
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);

            GUILayout.BeginVertical();
           
            GUILayout.Label("Global Flags");
            for (int i=0; i< dialog.dialogData.globalFlags.Count; i++)
            {
                DialogFlag flag = dialog.dialogData.globalFlags[i];
                GUILayout.BeginHorizontal();
                flag.flag = GUILayout.TextField(flag.flag);

                if (GUILayout.Button("X"))
                {
                    dialog.dialogData.globalFlags.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
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
            for (int i=0; i<dialog.dialogData.regionFlags.Count; i++)
            {
                DialogFlag flag = dialog.dialogData.regionFlags[i];
                GUILayout.BeginHorizontal();
                flag.flag = GUILayout.TextField(flag.flag);
                if(GUILayout.Button("X"))
                {
                    dialog.dialogData.regionFlags.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
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

            GUILayout.EndScrollView();

        }
        catch
        {
            GUILayout.Label("Unable to find DialogData");
        }
    }
}
