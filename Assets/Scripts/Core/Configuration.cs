using System.Collections.Generic;
using UnityEngine;

public static class Configuration
{
    public static int ExpForLevel(int level)
    {
        if (level == 0)
        {
            return 0;
        }
        else
        {
            return Mathf.RoundToInt(10 * Mathf.Pow(3f / 2f, level - 1));
        }
    }
}
