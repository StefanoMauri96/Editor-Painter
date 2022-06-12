using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class DrawEditor : EditorWindow
{

    private Material material;
    private List<Line> lines = new List<Line>();
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


    [MenuItem("Tool/Free Draw")]
    public static void ShowWindow()
    {

        GetWindow<DrawEditor>("Draw");

    }

    void OnEnable()
    {

        // Find the "Hidden/Internal-Colored" shader, and cache it for use.
        material = new Material(Shader.Find("Hidden/Internal-Colored"));

        lines = new List<Line>();
        lines.Add(currentLine);

    }



    private void Update()
    {

        //Debug.Log(currentTimer);
        Repaint();

    }

    private void OnGUI() 
    {

        GUILayout.BeginHorizontal();      

        if(Event.current.button == 0 && Event.current.isMouse == true)
        {

            canCreateNewLine = true;
            currentLine.points.Add(new Vector3(Event.current.mousePosition.x, Event.current.mousePosition.y, 0f));            

        }

        if(Event.current.type == EventType.MouseUp && Event.current.button == 0 && canCreateNewLine == true)
        {

            canCreateNewLine = false;
            CreateNewLine();

        }

        if(Event.current.button == 1)
        {

            lines.Clear();
            CreateNewLine();

        }

        if(Event.current.type == EventType.Repaint)
        {
            FreeDraw();
            DrawMouseQuad();
        }

        // End our horizontal 
        GUILayout.EndHorizontal();

        ShowColorMenu();
        ShowSaveMenu();

    }


    private void DrawMouseQuad()
    {

        GL.Begin(GL.QUADS);
        GL.Color(Color.yellow);
        GL.Vertex3(Event.current.mousePosition.x + 6, Event.current.mousePosition.y + 6, 0);
        GL.Vertex3(Event.current.mousePosition.x + 6, Event.current.mousePosition.y - 6, 0);
        GL.Vertex3(Event.current.mousePosition.x - 6, Event.current.mousePosition.y - 6, 0);
        GL.Vertex3(Event.current.mousePosition.x - 6, Event.current.mousePosition.y + 6, 0);
        GL.End();

        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        GL.Vertex3(Event.current.mousePosition.x, 1000,0f);
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

        for(int i=0; i<lines.Count; i++)
        {

            for (int j = 0; j < lines[i].points.Count; j++)
            {

                GL.Begin(GL.LINES);
                GL.Color(lines[i].colorLine);
                //GL.Vertex3(lines[i].points[j].x+1, lines[i].points[j].y+1, 0f);
                GL.Vertex3(lines[i].points[j].x, lines[i].points[j].y, 0f);
                //GL.Vertex3(lines[i].points[j].x-1, lines[i].points[j].y-1, 0f);

                if (j + 1 < lines[i].points.Count)
                {

                    //GL.Vertex3(lines[i].points[j + 1].x+1, lines[i].points[j + 1].y+1, 0f);
                    GL.Vertex3(lines[i].points[j + 1].x, lines[i].points[j + 1].y, 0f);
                    //GL.Vertex3(lines[i].points[j + 1].x-1, lines[i].points[j + 1].y-1, 0f);

                }

                GL.End();
            }

        }

    }

    private void DrawCustomMatric(Rect layoutRectangle)
    {

        // If we are currently in the Repaint event, begin to draw a clip of the size of 
        // previously reserved rectangle, and push the current matrix for drawing.
        GUI.BeginClip(layoutRectangle);
        GL.PushMatrix();

        // Clear the current render buffer, setting a new background colour, and set our
        // material for rendering.
        GL.Clear(true, false, Color.black);
        material.SetPass(0);

        // Start drawing in OpenGL Quads, to draw the background canvas. Set the
        // colour black as the current OpenGL drawing colour, and draw a quad covering
        // the dimensions of the layoutRectangle.
        GL.Begin(GL.QUADS);
        GL.Color(Color.black);
        GL.Vertex3(0, 0, 0);
        GL.Vertex3(layoutRectangle.width, 0, 0);
        GL.Vertex3(layoutRectangle.width, layoutRectangle.height, 0);
        GL.Vertex3(0, layoutRectangle.height, 0);
        GL.End();

        // Start drawing in OpenGL Lines, to draw the lines of the grid.
        GL.Begin(GL.LINES);

        // Store measurement values to determine the offset, for scrolling animation,
        // and the line count, for drawing the grid.
        int offset = (Time.frameCount * 2) % 50;
        int count = (int)(layoutRectangle.width / 10) + 20;

        for (int i = 0; i < count; i++)
        {
            // For every line being drawn in the grid, create a colour placeholder; if the
            // current index is divisible by 5, we are at a major segment line; set this
            // colour to a dark grey. If the current index is not divisible by 5, we are
            // at a minor segment line; set this colour to a lighter grey. Set the derived
            // colour as the current OpenGL drawing colour.
            Color lineColour = (i % 5 == 0
                ? new Color(0.5f, 0.5f, 0.5f) : new Color(0.2f, 0.2f, 0.2f));
            GL.Color(lineColour);

            // Derive a new x co-ordinate from the initial index, converting it straight
            // into line positions, and move it back to adjust for the animation offset.
            float x = i * 10 - offset;

            if (x >= 0 && x < layoutRectangle.width)
            {
                // If the current derived x position is within the bounds of the
                // rectangle, draw another vertical line.
                GL.Vertex3(x, 0, 0);
                GL.Vertex3(x, layoutRectangle.height, 0);
            }

            if (i < layoutRectangle.height / 10)
            {
                // Convert the current index value into a y position, and if it is within
                // the bounds of the rectangle, draw another horizontal line.
                GL.Vertex3(0, i * 10, 0);
                GL.Vertex3(layoutRectangle.width, i * 10, 0);
            }
        }

        // End lines drawing.
        GL.End();

        // Pop the current matrix for rendering, and end the drawing clip.
        GL.PopMatrix();
        GUI.EndClip();

    }

    private void CreateNewLine()
    {

        currentLine = new Line();
        currentLine.colorLine = selectedColor;
        lines.Add(currentLine);

    }

    private void ShowColorMenu()
    {

        GUILayout.BeginHorizontal();

        int buttonPosition = 0;
        int buttonHeight = 30;
        int buttonWeight = 30;

        for(int i=0; i< avaiableColors.Length; i++)
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

            SaveDraw();

        }

        if (GUI.Button(new Rect(120, 50, 100, 30), "Load"))
        {

            LoadDraw();

        }

        GUILayout.EndHorizontal();

    }

    private void SaveDraw()
    {

        List<Line> l = new List<Line>(lines);

        Draw draw = new Draw("Test", l);

        string path = EditorUtility.SaveFilePanelInProject("Save Draw", "Draw", "asset", "Plaese enter file name to save your draw");
        path = GetRelativePath(path);

        AssetDatabase.CreateAsset(draw, path);
        AssetDatabase.Refresh();

    }

    private void LoadDraw()
    {

        AssetDatabase.Refresh();

        string path = EditorUtility.OpenFilePanel("Load Draw", "Assets", "asset");
        path = GetRelativePath(path);
        Draw draw = AssetDatabase.LoadAssetAtPath<Draw>(path);

        if(draw == null)
        {

            Debug.Log("Asset null");
            return;

        }

        lines = new List<Line>(draw.lines);

    }

    private string GetRelativePath(string absloutePath)
    {

        Regex rgx = new Regex("Assets");

        string relativePath = "Assets" + rgx.Split(absloutePath)[1];
        return relativePath;

    }

}
