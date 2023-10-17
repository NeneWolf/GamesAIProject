using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    public static float OuterRadius(float hexSize)
    {
        return hexSize;
    }

    public static float InnerRadius(float hexSize)
    {
        return hexSize * 0.866025404f;
    }

    public static Vector3[] GetCorners(float hexSize, HexOrientation orientation)
    {
        Vector3[] corners = new Vector3[6];
        for (int i = 0; i < 6; i++)
        {
            corners[i] = Corner(hexSize, orientation,i);
        }
        return corners;
    }

    public static Vector3 Corner(float hexSize, HexOrientation orientation, int index)
    {
        float angle = 60f * index;
        if (orientation == HexOrientation.PointyTop)
        {
            angle += 30f;
        }
        Vector3 corner = new Vector3(hexSize * Mathf.Cos(angle * Mathf.Deg2Rad), 0f, hexSize * Mathf.Sin(angle * Mathf.Deg2Rad));
        return corner;
    }

    public static Vector3 Center(float hexSize, int x, int z , HexOrientation orientation)
    {
        Vector3 centerPoint;
        if(orientation == HexOrientation.PointyTop)
        {
            centerPoint.x = (x +z * 0.5f - z / 2) * (InnerRadius(hexSize) * 2f);
            centerPoint.y = 0f;
            centerPoint.z = z * (OuterRadius(hexSize) * 1.5f);
        }
        else
        {
            centerPoint.x = x * (OuterRadius(hexSize) * 1.5f);
            centerPoint.y = 0f;
            centerPoint.z = (z + x * 0.5f - x / 2) * (InnerRadius(hexSize) * 2f);
        }

        return centerPoint;
    }
}
