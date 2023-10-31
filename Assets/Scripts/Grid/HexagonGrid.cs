using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HexagonGrid<TGridObject>
{
    //public LayerMask unwalkableMask;
    //public Transform hexagonPrefab;
    //public int width;
    //public int height;
    //public float sizeHex;
    //public TGridObject[,] gridArray;
    //Vector3 originPosition;

    //public HexagonGrid(int width, int height,float sizeHex,Vector3 originPosition,Func<HexagonGrid<TGridObject>, int, int, TGridObject> createGridObject, GameObject parent)
    //{
    //    this.width = width;
    //    this.height = height;
    //    this.sizeHex = sizeHex;
    //    this.originPosition = originPosition;
    //    // TO CHANGE LATER
    //    //sizeHex = 3f;

    //    hexagonPrefab = parent.gameObject.transform;

    //    this.gridArray = new TGridObject[width, height];

    //    for (int x = 0; x < gridArray.GetLength(0); x++)
    //    {
    //        for (int z = 0; z < gridArray.GetLength(1); z++)
    //        {
    //            gridArray[x, z] = createGridObject(this,x,z);
    //        }
    //    }


    //    //Visual Display of the Grid
    //    for (int x = 0; x < gridArray.GetLength(0); x++)
    //    {
    //        for (int z = 0; z < gridArray.GetLength(1); z++)
    //        {
    //            CreateWorldText(hexagonPrefab,gridArray[x,z].ToString(), GetWorldPosition(x,z) + new Vector3(sizeHex,sizeHex, sizeHex) * 0.5f,10,Color.white,TextAnchor.MiddleCenter,TextAlignment.Center);
    //            Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
    //            Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);

    //        }
    //    }

    //    Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
    //    Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

    //}

    //public static TextMesh CreateWorldText(Transform parent, string text,Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor,TextAlignment textAlignment)
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
    //    return new Vector3(x,0,y) * sizeHex + originPosition;
    //}

    //public void GetXZ(Vector3 worldPosition, out int x, out int z)
    //{
    //    x = Mathf.FloorToInt((worldPosition - originPosition).x / sizeHex);
    //    z = Mathf.FloorToInt((worldPosition - originPosition).z / sizeHex);
    //}

    //public void SetGridObject(int x, int y, TGridObject value)
    //{
    //    if(x >= 0 && y >= 0 && x < width && y < height)
    //        gridArray[x, y] = value;
            
    //}

    //public void SetGridObject(Vector3 worldPosition, TGridObject value)
    //{
    //    int x, z;
    //    GetXZ(worldPosition, out x, out z);
    //    SetGridObject(x, z, value);
    //}

    //public TGridObject GetGridObject(int x, int y)
    //{
    //    if (x >= 0 && y >= 0 && x < width && y < height)
    //        return gridArray[x, y];
    //    else
    //        return default(TGridObject);
    //}

    //public TGridObject GetGridObject(Vector3 worldPosition)
    //{
    //    int x, z;

    //    GetXZ(worldPosition, out x, out z);
    //    return GetGridObject(x, z);
    //}
}
