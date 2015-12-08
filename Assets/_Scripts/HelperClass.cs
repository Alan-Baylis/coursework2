using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public static class HelperClass
{
    public delegate object DoSomethingWithList(List<object> objs);

    public delegate void DoSomethingWithElement(AbstractElement element);

    public delegate bool CheckFunction(AbstractElement element);

    public static string GetReadableList<T>(this List<T> list)
    {
        var str = list.Aggregate("", (current, variable) => current + (variable.ToString() + ", "));

        if (str.EndsWith(", "))
            str = str.Substring(0, str.Length - 2);

        return "[" + str + "]";
    }

    /// <summary>
    ///     Connects element from list to another element from that list.
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
        return new Battery(random.Next(10, 20), random.Next(1, 3));
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

    public static double GetParalellResistance(List<AbstractElement> elements)
    {
        return GetParallelResistance((from element in elements select element.Properties.Resistance).ToList<double>());
    }

    public static double GetParallelResistance(List<double> numbers)
    {
        return numbers.Product()/numbers.Sum();
    }

    public static double Product(this List<double> numbers)
    {
        var r = 1.0;

        numbers.ForEach(x => r *= x);

        return r;
    }

    public static double Sum(this List<double> numbers)
    {
        var r = 0.0;

        numbers.ForEach(x => r += x);

        return r;
    }

    public static object GetPropertiesOfChain(AbstractElement beginning, string meta, DoSomethingWithList func=null)
    {
        if (func == null)
            func = objectsList => (from element in objectsList select (double) (element)).ToList().Sum();
        switch (meta)
        {
            case "resistance":
            {
                var objects = new List<object>();
                for (var i = beginning; i != null; i = i.NextElement)
                {
                    objects.Add(i.Properties.Resistance);
                }
                return func(objects);
            }
            case "current":
            {
                var objects = new List<object>();
                for (var i = beginning; i != null; i = i.NextElement)
                {
                    objects.Add(i.Properties.Current);
                }
                return func(objects);
            }
            default:
                return null;
        }
    }

    public static void DoWithChain(AbstractElement beginning, DoSomethingWithElement func, CheckFunction isNotTheEnd=null)
    {
        if (isNotTheEnd == null)
            isNotTheEnd = x => x != null;
        for (var i = beginning; isNotTheEnd(i); i = i.NextElement)
        {
            func(i);
        }
    }

    public static void DrawConnection(Vector2 startPoint, Vector2 endPoint)
    {
        const float curveThickness = 1.5f;
        var tangent = Mathf.Clamp((-1)*(startPoint.x - endPoint.x), -100, 100);
        var startTangent = new Vector2(startPoint.x + tangent, startPoint.y);
        var endTangent = new Vector2(endPoint.x - tangent, endPoint.y);
        Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, new Color(0f, 0.1f, 0.4f, 0.6f), null,
            curveThickness);
    }


    #region resistivity table

    private static readonly Dictionary<string, double> ResistivityTable = new Dictionary<string, double>
    {
        {"copper", 1.68e-8},
        {"silver", 1.59e-8},
        {"aluminium", 2.65e-8},
        {"graphene", 1e-8},
        {"test_one", 1.0}
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
}