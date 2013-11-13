using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogData
{
    [SerializeField]
    public List<DialogFlag> globalFlags;

    [SerializeField]
    public List<DialogFlag> regionFlags;

    [SerializeField]
    public List<DialogNPC> npcList;

    public DialogData()
    {
        globalFlags = new List<DialogFlag>();
        regionFlags = new List<DialogFlag>();
        npcList = new List<DialogNPC>();
    }
}

[System.Serializable]
public class DialogFlag
{
    public string flag;
    public int id;
    public static int nextID;

    public void SetNextID(int id)
    {
        DialogFlag.nextID = id;
    }

    public DialogFlag(string flag)
    {
        this.flag = flag;
        this.id = nextID;
        nextID += 1;
    }

    public DialogFlag(string flag, int id)
    {
        this.flag = flag;
        this.id = id;
        if (id >= nextID) nextID = id + 1;
    }
}
