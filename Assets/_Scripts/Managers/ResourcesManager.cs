using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager Instance { get; protected set; }
    public struct Entry
    {
        public string name;
        public GameObject prefab;
    }
    public List<Entry> entries = new List<Entry>();

    void Awake()
    {
        Instance = this;
    }
}
