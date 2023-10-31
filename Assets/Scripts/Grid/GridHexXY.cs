using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHexXY<TGridObject>
{
    //bool showDebug;
    //private int width;
    //private int height;
    //private float cellSize;
    //private Vector3 originPosition;
    //private TGridObject[,] gridArray;

    //public GridHexXY(int width, int height, float cellSize, Vector3 originPosition, Func<GridHexXY<TGridObject>, int, int, TGridObject> createHexGrid, GameObject parent, bool showDebug)
    //{
    //    this.width = width;
    //    this.height = height;
    //    this.cellSize = cellSize;
    //    this.originPosition = originPosition;
    //    this.showDebug = showDebug;
        
    //    gridArray = new TGridObject[width, height];

    //    for(int x  = 0; x < gridArray.GetLength(0); x++)
    //    {
    //        for(int z = 0 ; z < gridArray.GetLength(1); z++)
    //        {
    //            gridArray[x, z] = createGridObject(this, x, z);
    //        }
    //    }

    //    if(showDebug)
    //    {
    //        TextMesh[,] debugTextArray = new TextMesh[width, height];

    //        for(int x = 0; x< gridArray.GetLength(0); x++)
    //        {
    //            for(int z = 0 ;z < gridArray.GetLength(1); z++)
    //            {
    //                debugTextArray[x, z] = CreateWorldText(parent.transform, gridArray[x,z]?.ToString(), GetWorldPosition(x,z), 5, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
    //                debugTextArray[x, z].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    //                debugTextArray[x, z].transform.eulerAngles = new Vector3(90, 0, 0);
    //                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
    //                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);

    //            }
    //        }

    //        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
    //        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            
    //    }
    //}

    //public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment)
    //{
    //    GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
    //    Transform transform = gameObject.transform;
    //    transform.SetParent(parent, false);
    //    transform.localPosition = localPosition;
    //    TextMesh textMesh = gameObject.GetComponent<TextMesh>();
    //    textMesh.anchor = textAnchor;
    //    textMesh.fontSize = fontSize;
    //    textMesh.color = color;
    //    textMesh.alignment = textAlignment;
    //    textMesh.text = text;
    //    return textMesh;
    //}

    //private Vector3 GetWorldPosition(int x, int y)
    //{
    //    return new Vector3(x, 0, y) * cellSize + originPosition;
    //}

    //public int GetWidth()
    //{
    //    return width;
    //}
    //public int GetHeight()
    //{
    //      return height;
    //}
}

