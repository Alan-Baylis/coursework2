using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class HelperClass
{
    private static readonly Dictionary<string, double> ResistivityTable = new Dictionary<string, double>()
    {
        {"copper", 1.68e-8},
        {"silver", 1.59e-8},
        {"aluminium", 2.65e-8},
        {"graphene", 1e-8}
    };

    public static double GetResistivity(string s)
    {
        return ResistivityTable[s];
    }

    public static void SetResistivity(string s, double newValue)
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