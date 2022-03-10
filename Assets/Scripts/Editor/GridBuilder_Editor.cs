using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridBuilder))]
public class GridBuilder_Editor : Editor
{
    GridBuilder gridBuilder;
    public override void OnInspectorGUI()
    {
        gridBuilder = (GridBuilder)target;

        base.OnInspectorGUI();

        if(GUILayout.Button("Generate Grid"))
        {
            DeleteGrid();
            gridBuilder.BuildGrid(29, 26);
        }

        if (GUILayout.Button("Delete Grid"))
        {
            DeleteGrid();
        }

        if (GUILayout.Button("Debug Blocked Grid"))
        {
            //DebugBlockedGrids();
        }

    }

    private void DeleteGrid()
    {
        if (gridBuilder.GridNodes != null)
        {
            foreach (var item in gridBuilder.GridNodes)
            {
                if (item != null)
                    DestroyImmediate(item.gameObject);
            }
        }
    }

    private void DebugBlockedGrids()
    {
        
    }
}
