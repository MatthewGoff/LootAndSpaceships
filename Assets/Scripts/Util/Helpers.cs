using System.Collections.Generic;
using UnityEngine;

public static class Helpers{

	public static float PerlinNoise(float x, float y, float[,] frequencies, float offset)
    {
        float noiseSample = 0f;
        float totalWeight = 0f;
        for (int i = 0; i < frequencies.GetLength(1); i++)
        {
            noiseSample += frequencies[0, i] * Mathf.PerlinNoise(x * frequencies[1, i] + offset, y * frequencies[1, i] + offset);
            totalWeight += frequencies[0, i];
        }
        return noiseSample / totalWeight;
    }

    public static string ToString<T>(IEnumerable<T> collection)
    {
        string returnString = "[";
        foreach (T element in collection)
        {
            returnString += element.ToString() + ", ";
        }
        if (returnString.Length > 1)
        {
            returnString = returnString.Substring(0, returnString.Length - 2);
        }
        returnString =  returnString + "]";
        return returnString;
    }
}
