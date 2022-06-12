using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Mauricoder.EditorPaint
{
    public static class Utility
    {

        public static string ConvertAbsloutePathToProjectRelativePath(string absloutePath)
        {

            try
            {

                Regex rgx = new Regex("Assets");

                string relativePath = "Assets" + rgx.Split(absloutePath)[1];
                return relativePath;

            }
            catch(Exception ex)
            {

                Debug.Log("Path error");

                return null;

            }

        }

        public static bool CheckStringIsValid(string value)
        {

            if (string.IsNullOrEmpty(value) == true || string.IsNullOrWhiteSpace(value) == true)
                return false;
            else
                return true;

        }

    }
}
