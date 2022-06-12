using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mauricoder.EditorPaint.Core;

namespace Mauricoder.EditorPaint
{

    public class EditorPainter : EditorWindow
    {

        #region Private

        private Draw draw;
        private Line currentLine = new Line();
        private bool canCreateNewLine = false;
        private Color selectedColor = Color.white;

        private Color[] avaiableColors = new Color[]
           {
            Color.white,
            Color.red,
            Color.blue,
            Color.green,
            Color.yellow,
            Color.cyan,
           };

        #endregion


        void OnEnable()
        {

            Initialization();

        }

        void Update()
        {

            Repaint();

        }

        void OnGUI()
        {

            GUILayout.BeginHorizontal();

            if (Event.current.button == 0 && Event.current.isMouse == true)
            {

                canCreateNewLine = true;
                currentLine.points.Add(new Vector3(Event.current.mousePosition.x, Event.current.mousePosition.y, 0f));

            }

            if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && canCreateNewLine == true)
            {

                canCreateNewLine = false;
                CreateNewLine();

            }

            if (Event.current.button == 1)
            {

                draw.lines.Clear();
                CreateNewLine();

            }

            if (Event.current.type == EventType.Repaint)
            {
                FreeDraw();
                DrawMouseGuideline();
            }

            GUILayout.EndHorizontal();

            ShowColorMenu();
            ShowSaveMenu();

        }


        #region Private Static Functions

        [MenuItem("Tools/Mauricoder/Editor Painter")]
        public static void ShowWindow()
        {

            EditorPainter editorPainter = GetWindow<EditorPainter>("Editor Painter");
            editorPainter.minSize = new Vector2(800, 600);
            editorPainter.maxSize = new Vector2(1920, 1080);
            editorPainter.position = new Rect(new Vector2(0, 0), new Vector2(1280, 720));

        }

        #endregion

        #region Private Functions

        private void Initialization()
        {

            draw = new Draw();
            currentLine = new Line();
            draw.lines.Add(currentLine);

        }

        private void DrawMouseGuideline()
        {

            GL.Begin(GL.QUADS);
            GL.Color(selectedColor);
            GL.Vertex3(Event.current.mousePosition.x + 6, Event.current.mousePosition.y + 6, 0);
            GL.Vertex3(Event.current.mousePosition.x + 6, Event.current.mousePosition.y - 6, 0);
            GL.Vertex3(Event.current.mousePosition.x - 6, Event.current.mousePosition.y - 6, 0);
            GL.Vertex3(Event.current.mousePosition.x - 6, Event.current.mousePosition.y + 6, 0);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex3(Event.current.mousePosition.x, 1000, 0f);
            GL.Vertex3(Event.current.mousePosition.x, -1000, 0f);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex3(10000, Event.current.mousePosition.y, 0f);
            GL.Vertex3(-10000, Event.current.mousePosition.y, 0f);
            GL.End();

        }

        private void FreeDraw()
        {

            for (int i = 0; i < draw.lines.Count; i++)
            {

                for (int j = 0; j < draw.lines[i].points.Count; j++)
                {

                    GL.Begin(GL.LINES);
                    GL.Color(draw.lines[i].colorLine);
                    GL.Vertex3(draw.lines[i].points[j].x, draw.lines[i].points[j].y, 0f);

                    if (j + 1 < draw.lines[i].points.Count)
                    {

                        GL.Vertex3(draw.lines[i].points[j + 1].x, draw.lines[i].points[j + 1].y, 0f);

                    }

                    GL.End();
                }

            }

        }

        private void CreateNewLine()
        {

            currentLine = new Line();
            currentLine.colorLine = selectedColor;
            draw.lines.Add(currentLine);

        }

        private void ShowColorMenu()
        {

            GUILayout.BeginHorizontal();

            int buttonPosition = 0;
            int buttonHeight = 30;
            int buttonWeight = 30;

            for (int i = 0; i < avaiableColors.Length; i++)
            {

                GUI.backgroundColor = avaiableColors[i];

                if (GUI.Button(new Rect(buttonPosition, 0, buttonWeight, buttonHeight), ""))
                {

                    SelectColor(avaiableColors[i]);

                }

                buttonPosition += 30;

            }

            GUILayout.EndHorizontal();

        }

        private void SelectColor(Color color)
        {

            selectedColor = color;
            currentLine.colorLine = color;

        }

        private void ShowSaveMenu()
        {

            GUILayout.BeginHorizontal();

            if (GUI.Button(new Rect(0, 50, 100, 30), "Save"))
            {

                DrawSaver drawSaver = new DrawSaver();
                drawSaver.Save(draw.lines);

            }

            if (GUI.Button(new Rect(120, 50, 100, 30), "Load"))
            {

                DrawLoader drawLoader = new DrawLoader();
                draw.lines = drawLoader.Load().lines;

            }

            GUILayout.EndHorizontal();

        }

        #endregion

    }
}
