using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Draw : ScriptableObject
{
    
    public string name;
    public List<Line> lines = new List<Line>();

    public Draw(string name, List<Line> lines)
    {
        this.name = name;
        this.lines = lines;
    }
}
