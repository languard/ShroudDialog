using UnityEngine;
using UnityEditor;
using System.Collections;

public class DialogNPCWindow : EditorWindow 
{

    static DialogDataMono dialog;
    Vector2 scrollPositionNPC;
    Vector2 scrollPositionFlags;
    int focusIndex = 0;

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
        EditorStyles.textField.wordWrap = true;
        try
        {
            GUILayout.BeginArea(new Rect(3, 3, 550, 700));
            GUILayout.BeginVertical();
    
            GUILayout.Label("NPC List");

            GUILayout.BeginHorizontal();
            scrollPositionNPC = GUILayout.BeginScrollView(scrollPositionNPC, GUILayout.Width(200), GUILayout.Height(400));

            for (int i = 0; i < dialog.dialogData.npcList.Count; i++)
            {
                if (focusIndex == i) GUI.color = Color.green;
                GUI.SetNextControlName("NPC_" + i.ToString());
                dialog.dialogData.npcList[i].name = EditorGUILayout.TextField(dialog.dialogData.npcList[i].name);
                GUI.color = Color.white;
            }
            GUILayout.EndScrollView();

            GUILayout.BeginVertical();
            GUILayout.Label("Description");
            string[] curControl = GUI.GetNameOfFocusedControl().Split('_');
            

            if (curControl.Length == 2 && curControl[0] == "NPC")
            {
                focusIndex = int.Parse(curControl[1]);
                EditorPrefs.SetInt("focusIndex", focusIndex);
                GUI.SetNextControlName("i_g_nore");
                dialog.dialogData.npcList[focusIndex].description = EditorGUILayout.TextField(dialog.dialogData.npcList[focusIndex].description, GUILayout.Width(330), GUILayout.Height(200));
            }
            else if (curControl.Length == 3 && focusIndex >= 0 && focusIndex < dialog.dialogData.npcList.Count)
            {
                GUI.SetNextControlName("i_g_nore");
                dialog.dialogData.npcList[focusIndex].description = EditorGUILayout.TextField(dialog.dialogData.npcList[focusIndex].description, GUILayout.Width(330), GUILayout.Height(200));
            }
            else
            {
                GUI.SetNextControlName("i_g_nore");
                focusIndex = -1;
                EditorPrefs.SetInt("focusIndex", focusIndex);
                GUILayout.TextField("No NPC Selected", GUILayout.Width(230), GUILayout.Height(200));
            }
            GUILayout.Label("Flags");
            scrollPositionFlags = GUILayout.BeginScrollView(scrollPositionFlags);
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Add NPC"))
            {
                DialogNPC newNPC = new DialogNPC();
                dialog.dialogData.npcList.Add(newNPC);
            }
            if (GUILayout.Button("Delete NPC"))
            {
                if (focusIndex >= 0 && focusIndex < dialog.dialogData.npcList.Count)
                {                    
                    dialog.dialogData.npcList.RemoveAt(focusIndex);
                    focusIndex = -1;
                    GUI.FocusControl("");
                    EditorPrefs.SetInt("focusIndex", focusIndex);                    
                }
                Repaint();
            }
            GUILayout.EndArea();
       
        }
        catch
        {
            GUILayout.Label("Unable to find DialogData");
        }
    }

}
