using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mauricoder.EditorPaint.Core
{
    [Serializable]
    public class Draw : ScriptableObject
    {

        public List<Line> lines = new List<Line>();

        public Draw()
        {
        }

        public Draw(List<Line> lines)
        {
            this.lines = lines;
        }
    }
}
