using System;
using System.Collections.Generic;
using System.Linq;

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
        return new Cable(ElectricProperties.CreateFromUR(0, random.Next(1, 15)));
    }

    public static Battery GetRandomBattery(Random random)
    {
        return new Battery(ElectricProperties.CreateFromUR(random.Next(10, 20), random.Next(1, 3)));
    }
}