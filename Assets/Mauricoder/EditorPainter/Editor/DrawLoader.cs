using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mauricoder.EditorPaint.Core;

namespace Mauricoder.EditorPaint
{
    public class DrawLoader
    {
        public Draw Load()
        {

            AssetDatabase.Refresh();

            string path = EditorUtility.OpenFilePanel("Load Draw", "Assets", "asset");
            path = Utility.ConvertAbsloutePathToProjectRelativePath(path);
            Draw draw = AssetDatabase.LoadAssetAtPath<Draw>(path);

            if (draw == null)
            {

                Debug.Log("Asset null");
                return null;

            }

            return draw;

        }

    }
}
