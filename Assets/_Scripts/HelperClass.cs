using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public static class HelperClass
{
    public delegate object DoSomethingWithList(List<object> objs);

    public delegate void DoSomethingWithElement(AbstractElement element);

    public delegate bool CheckFunction(AbstractElement element);

    public static Random myRandom = new Random();

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

    public static Cable GetRandomCable()
    {
        return new Cable();
    }

    public static Battery GetRandomBattery()
    {
        return new Battery(myRandom.Next(10, 20), myRandom.Next(1, 3));
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

    public static void DrawConnections(List<Vector3[]> pointPairs)
    {
        foreach (var pair in pointPairs)
        {
            var linesObject = ElectricalCircuit.Instance.LineRenderers[pointPairs.IndexOf(pair)];
            linesObject.SetVertexCount(0);
            linesObject.SetVertexCount(2);
            var startPoint = pair[0];
            var endPoint = pair[1];
            /*const float curveThickness = 1.5f;
            var tangent = Mathf.Clamp((-1)*(startPoint.x - endPoint.x), -100, 100);
            var startTangent = new Vector2(startPoint.x + tangent, startPoint.y);
            var endTangent = new Vector2(endPoint.x - tangent, endPoint.y);*/
            linesObject.SetPosition(0, startPoint);
            linesObject.SetPosition(1, endPoint);
        }
    }

    

    public static int NextInRangeOrFirst(this int a, int begin, int end)
    {
        return a == end - 1 ? begin : a + 1;
    }

    public static bool IsNotNull(this object o)
    {
        return o != null;
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