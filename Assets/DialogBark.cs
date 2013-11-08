using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogBark 
{

    public string line;
    public List<DialogToken> tokens = new List<DialogToken>();
    public List<DialogFlag> blockFlags = new List<DialogFlag>();
    public List<DialogFlag> requireFlags = new List<DialogFlag>();
    public List<DialogFlag> setFlags = new List<DialogFlag>();
    


}
