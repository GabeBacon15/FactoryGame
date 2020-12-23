using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{

    public static float[,] generatePerlinNoise(int width, int height, float scale, float posx, float posy, int seed)
    {
        System.Random rand = new System.Random(seed);
        float offsetX = rand.Next(-10000, 10000) + posx;
        float offsetY = rand.Next(-10000, 10000) + posy;

        float[,] noise = new float[width, height];

        if (scale == 0)
            scale = 0.00001f;

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                float pointX = (x / scale) + offsetX;
                float pointY = (y / scale) + offsetY;

                noise[x, y] = Mathf.PerlinNoise(pointX, pointY);
            }
        }

        return noise;
    }

}
