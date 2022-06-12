using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mauricoder.EditorPaint.Core;

namespace Mauricoder.EditorPaint
{

    public class DrawSaver
    {
        public void Save(List<Line> lines)
        {

            List<Line> l = new List<Line>(lines);

            Draw draw = new Draw(l);

            string path = EditorUtility.SaveFilePanelInProject("Save Draw", "Draw", "asset", "Plaese enter file name to save your draw");
            path = Utility.ConvertAbsloutePathToProjectRelativePath(path);

            AssetDatabase.CreateAsset(draw, path);
            AssetDatabase.Refresh();

        }

    }
}
