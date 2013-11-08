using UnityEngine;
using UnityEditor;
using System.Collections;

public class DialogNPCWindow : EditorWindow 
{

    static DialogDataMono dialog;

    [MenuItem("Dialog/NPC Editor")]
    static void CreateNPCWindow()
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
                Debug.LogError("Failed to find DialogDataMono on the game object!  Fix this!");
            }
        }

        DialogNPCWindow window = (DialogNPCWindow)EditorWindow.GetWindow(typeof(DialogNPCWindow));
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
            for (int i = 0; i < dialog.dialogData.npcList.Count; i++)
            {
                GUILayout.Label(dialog.dialogData.npcList[i].name);
            }
        }
        catch
        {
            GUILayout.Label("Unable to find DialogData");
        }
    }

}
