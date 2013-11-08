using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogNPC 
{
    public string name;
    public string description;
    public List<DialogBark> barkList = new List<DialogBark>();
    public List<DialogFlag> availableFlagList = new List<DialogFlag>();
    public List<DialogFlag> setFlagList = new List<DialogFlag>();

    public DialogNPC()
    {
        name = "NOT SET";
        description = "NOT SET";
    }
}
