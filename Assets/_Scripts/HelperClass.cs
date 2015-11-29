using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class HelperClass
{
    private static readonly Dictionary<string, float> ResistivityTable = new Dictionary<string, float>()
    {
        {"copper", 1.68e-8f},
        {"silver", 1.59e-8f},
        {"aluminium", 2.65e-8f},
        {"graphene", 1e-8f}
    };

    public static float GetResistivity(string s)
    {
        return ResistivityTable[s];
    }

    public static void SetResistivity(string s, float newValue)
    {
        ResistivityTable[s] = newValue;
    }

    public static string GetReadableList<T>(this List<T> list)
    {
        var str = list.Aggregate("", (current, variable) => current + (variable.ToString() + ", "));

        if (str.EndsWith(", "))
            str = str.Substring(0, str.Length - 2);

        return "[" + str + "]";
    }
}