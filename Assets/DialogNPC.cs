using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogNPC 
{
    public string name;
    public string description;
    List<DialogBark> barkList = new List<DialogBark>();
    List<DialogFlag> availableFlagList = new List<DialogFlag>();
    List<DialogFlag> setFlagList = new List<DialogFlag>();
	
}
