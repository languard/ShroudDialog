using UnityEngine;
using UnityEditor;
using System.Collections;

public class DialogNPCWindow : EditorWindow 
{

    static DialogDataMono dialog;
    Vector2 scrollPositionNPC;
    Vector2 scrollPositionFlags;
    int focusIndex = 0;
    int barkIndex = 0;
    string saveName = "DialogFile";

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
            FindDialog();
        }
        Repaint();
    }

    void OnGUI()
    {
        EditorStyles.textField.wordWrap = true;
        string[] curControl = null;
        try
        {
            GUILayout.BeginArea(new Rect(3, 3, 450, 700));
            GUILayout.BeginVertical();
    
            GUILayout.Label("NPC List");

            GUILayout.BeginHorizontal();
            scrollPositionNPC = GUILayout.BeginScrollView(scrollPositionNPC, GUILayout.Width(200), GUILayout.Height(250));

            for (int i = 0; i < dialog.dialogData.npcList.Count; i++)
            {
                if (focusIndex == i) GUI.color = Color.green;
				string conName = "NPC_" + i.ToString();
                GUI.SetNextControlName(conName);
                dialog.dialogData.npcList[i].name = EditorGUILayout.TextField(dialog.dialogData.npcList[i].name);
                GUI.color = Color.white;
            }
            GUILayout.EndScrollView();

            
            GUILayout.BeginVertical();
            GUILayout.Label("Description");
            curControl = GUI.GetNameOfFocusedControl().Split('_');
            

            if (curControl.Length == 2)
            {
                int temp = int.Parse(curControl[1]);
                if (temp >= 0 && temp < dialog.dialogData.npcList.Count)
                {
                    //if (temp != focusIndex)
                    //{
                    //    Debug.LogWarning("Old: " + focusIndex + "  New: " + temp);
                    //    Debug.LogWarning(dialog.dialogData.npcList.Count);
                    //}
                    focusIndex = temp;
                    EditorPrefs.SetInt("focusIndex", focusIndex);
                    GUI.SetNextControlName("i_g_nore");                    
                }
                dialog.dialogData.npcList[focusIndex].description = EditorGUILayout.TextField(dialog.dialogData.npcList[focusIndex].description, GUILayout.Width(230), GUILayout.Height(200));
            }
            else if (curControl.Length == 3 && focusIndex >= 0 && focusIndex < dialog.dialogData.npcList.Count)
            {
                GUI.SetNextControlName("i_g_nore");                
                dialog.dialogData.npcList[focusIndex].description = EditorGUILayout.TextField(dialog.dialogData.npcList[focusIndex].description, GUILayout.Width(230), GUILayout.Height(200));
            }
            else
            {
				focusIndex = -1;
				EditorPrefs.SetInt("focusIndex", focusIndex);
                GUI.SetNextControlName("i_g_nore");                                
                GUILayout.TextField("No NPC Selected", GUILayout.Width(230), GUILayout.Height(200));
            }
            
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            if (GUILayout.Button("Add NPC"))
            {
                DialogNPC newNPC = new DialogNPC();
                dialog.dialogData.npcList.Add(newNPC);
                GUI.FocusControl("");
            }
            if (GUILayout.Button("Delete NPC"))
            {
                if (focusIndex >= 0 && focusIndex < dialog.dialogData.npcList.Count)
                {                                        
                    dialog.dialogData.npcList.RemoveAt(focusIndex);
                    if (focusIndex == dialog.dialogData.npcList.Count) focusIndex = dialog.dialogData.npcList.Count - 1;
                    //GUI.FocusControl("");
                    if (focusIndex == -1) GUI.FocusControl("");
                    EditorPrefs.SetInt("focusIndex", focusIndex);                    
                }
                Repaint();
            }
            saveName = GUILayout.TextField(saveName);
            if (GUILayout.Button("Save Data"))
            {
                System.Text.StringBuilder allData = new System.Text.StringBuilder();

                #region oldMethod
                //foreach (DialogNPC npc in dialog.dialogData.npcList)
                //{
                //    allData.AppendLine("[NPC]");
                //    allData.AppendLine("[NAME]");
                //    allData.AppendLine(npc.name);
                //    allData.AppendLine("[DESC]");
                //    allData.AppendLine(npc.description);
                //    allData.AppendLine("[ALLBARKS]");
                //    foreach (DialogBark bark in npc.barkList)
                //    {
                //        allData.AppendLine("[BARK]");
                //        allData.AppendLine("[LINE]");
                //        allData.AppendLine(bark.line);
                //        allData.AppendLine("[TOKENS]");
                //        allData.AppendLine(bark.tokens);
                //        allData.AppendLine("[REQUIREDFLAGS]");
                //        allData.AppendLine(bark.requireFlags);
                //        allData.AppendLine("[FORBIDENFLAGS]");
                //        allData.AppendLine(bark.blockFlags);
                //        allData.AppendLine("[SAVEFLAGS]");
                //        allData.AppendLine(bark.saveFlags);
                //        allData.AppendLine("[ENDBARK]");
                //    }
                //    allData.AppendLine("[ENDNPC]");
                //}
                #endregion

                allData.AppendLine("NPC,Bark,Tokens,Required,Forbiden,Save");

                foreach (DialogNPC npc in dialog.dialogData.npcList)
                {
                    foreach (DialogBark bark in npc.barkList)
                    {
                        allData.AppendLine(System.String.Format(
                            "{0},{1},{2},{3},{4},{5}",
                            npc.name,
                            bark.line,
                            bark.tokens,
                            bark.requireFlags,
                            bark.blockFlags,
                            bark.saveFlags
                            ));
                    }
                }

                string assetName = "";

                if (saveName.Length < 1) assetName = "/DialogFile.csv";
                else assetName = System.String.Format("{0}/{1}.csv", Application.dataPath, saveName);

                Debug.Log(assetName);
                System.IO.File.WriteAllText(assetName, allData.ToString());
                AssetDatabase.Refresh();
            }
            GUILayout.Button("Load Data");
            GUILayout.EndArea();
            
       
        }
        catch(System.Exception e)
        {
            GUILayout.Label("Error occured during OnGUI, clearing control focus.  You should not see this, if you do something is really wrong.");
            Debug.LogError(e.Message);
            Debug.LogError("If the controls look normal, this error has been handled.");
            GUI.FocusControl("");
            focusIndex = -1;
            EditorPrefs.SetInt("focusIndex", focusIndex);
            FindDialog();
        }
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

}
