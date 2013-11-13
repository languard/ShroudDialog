using UnityEngine;
using UnityEditor;
using System.Collections;


public class DialogBarksWindow : EditorWindow 
{
    static DialogDataMono dialog;
    int focusIndex = -1;
    int activeBark = -1;
    Vector2 scrollPosition;
    bool forceRepaint = false;

    [MenuItem("Dialog/Bark Editor")]
    static void CreateBarkWindow()
    {
        FindDialog();

        DialogBarksWindow window = (DialogBarksWindow)EditorWindow.GetWindow(typeof(DialogBarksWindow));
        window.title = "Barks";
        window.Repaint();
    }

    private static void FindDialog()
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
    }

    void OnInspectorUpdate()
    {
        int newFocus = EditorPrefs.GetInt("focusIndex");
        if (newFocus != focusIndex)
        {
            activeBark = -1;
        }
        Repaint();
    }

    void OnGUI()    
    {
        int newFocus = EditorPrefs.GetInt("focusIndex");
        if (newFocus != focusIndex)
        {
            focusIndex = newFocus;
            activeBark = -1;            
        }
        if (focusIndex == -1)
        {
            GUILayout.Label("No NPC Selected");
            activeBark = -1;
        }
        else
        {
            EditBark();
        }
       
    }



    void EditBark()
    {
        if (dialog == null)
        {
            FindDialog();
        }
        GUILayout.BeginVertical();
        GUILayout.Label("Available Barks");
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(400), GUILayout.Height(200));
        for (int i = 0; i < dialog.dialogData.npcList[focusIndex].barkList.Count; i++)
        {
            if (activeBark == i) GUI.color = Color.green;

            if (GUILayout.Button(dialog.dialogData.npcList[focusIndex].barkList[i].line, "Label"))
            {
                activeBark = i;
                forceRepaint = true;
            }

            GUI.color = Color.white;
        }
        GUILayout.EndScrollView();
        if (activeBark != -1)
        {
            DialogBark curBark = dialog.dialogData.npcList[focusIndex].barkList[activeBark];
            GUILayout.Label("Bark Line");
            curBark.line = EditorGUILayout.TextArea(curBark.line, GUILayout.Width(400), GUILayout.Height(45));
            GUILayout.Label("All token and flag lists are comma seperated");
            GUILayout.Label("Tokens");
            curBark.tokens = EditorGUILayout.TextField(curBark.tokens, GUILayout.Width(400), GUILayout.Height(45));
            GUILayout.Label("Forbidden Flags");
            curBark.blockFlags = EditorGUILayout.TextField(curBark.blockFlags, GUILayout.Width(400), GUILayout.Height(45));
            GUILayout.Label("Required Flags");
            curBark.requireFlags = EditorGUILayout.TextField(curBark.requireFlags, GUILayout.Width(400), GUILayout.Height(45));
            GUILayout.Label("Saved Flags");
            curBark.saveFlags = EditorGUILayout.TextField(curBark.saveFlags, GUILayout.Width(400), GUILayout.Height(45));
        }
        if (GUILayout.Button("Add Bark", GUILayout.Width(400)))
        {
            DialogBark newBark = new DialogBark();
            dialog.dialogData.npcList[focusIndex].barkList.Add(newBark);
        }
        if (GUILayout.Button("Delete Bark", GUILayout.Width(400)))
        {
            if (activeBark >= 0 && activeBark < dialog.dialogData.npcList[focusIndex].barkList.Count)
            {
                dialog.dialogData.npcList[focusIndex].barkList.RemoveAt(activeBark);
                if (activeBark == dialog.dialogData.npcList[focusIndex].barkList.Count)
                {
                    activeBark = dialog.dialogData.npcList[focusIndex].barkList.Count - 1;
                }
                if(activeBark == -1) GUI.FocusControl("");
                Repaint();
            }
        }

        GUILayout.EndVertical();

        if (forceRepaint)
        {
            forceRepaint = false;
            GUI.FocusControl("");
            Repaint();
        }
    }
}
