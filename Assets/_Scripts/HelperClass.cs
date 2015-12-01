using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class HelperClass
{
    #region resistivity table
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
    #endregion

    public static string GetReadableList<T>(this List<T> list)
    {
        var str = list.Aggregate("", (current, variable) => current + (variable.ToString() + ", "));

        if (str.EndsWith(", "))
            str = str.Substring(0, str.Length - 2);

        return "[" + str + "]";
    }

    /// <summary>
    /// Connects element from list to another element from that list.
    /// </summary>
    /// <param name="all">list</param>
    /// <param name="ind1">index of element which you connect</param>
    /// <param name="ind2">index of element to which you connect</param>
    public static void Join(IList<AbstractElement> all, int ind1, int ind2)
    {
        all[ind1].Connect(all[ind2]);
    }

    public static Cable GetRandomCable(Random random)
    {
        return new Cable("copper", random.Next(1, 5), random.Next(1, 10));
    }

    public static Battery GetRandomBattery(Random random)
    {
        return new Battery(ElectricProperties.CreateFromUR(random.Next(10, 20), random.Next(1, 3)));
    }

    public static string IdRefinition(string id, List<string> ids)
    {
        if (!ids.Contains(id))
            return id;
        var r = new Regex(@"\(\d+\)");
        var m = r.Match(id);
        if (m.Success)
        {
            var oldValue = m.Groups[m.Groups.Count - 1].ToString();
            var numberInParenthesis = Convert.ToInt32(oldValue.Substring(1, oldValue.Length - 1));
            id = id.Replace(oldValue, string.Format("({0})", numberInParenthesis + 1));
        }
        else
        {
            id += " (1)";
        }

        return id;
    }
}