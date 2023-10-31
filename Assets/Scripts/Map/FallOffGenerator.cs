using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FallOffGenerator
{ 
    public static float[,] GenerateFallOffMap(int mapWidth, int mapHeight)
    {
        float[,] map = new float[mapWidth, mapHeight];

        for(int i = 0; i < mapWidth; i++)
        {
            for(int j = 0; j < mapHeight; j++)
            {
                float x = i / (float)mapWidth * 2 - 1;
                float y = j / (float)mapHeight * 2 - 1;

                //Find the largest value of x and z
                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));

                map[i, j] = Evaluate(value);
            }
        }
        return map;
    }

    static float Evaluate(float value)
    {
        float a = 3;
        float b = 2.2f;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
}
