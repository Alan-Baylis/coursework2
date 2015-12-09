using System;
using UnityEngine;
using System.Collections.Generic;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager Instance { get; protected set; }
    [Serializable]
    public class StringGameObject
    {
        public string name = "";
        public GameObject prefab = null;
    }

    public List<StringGameObject> entries = new List<StringGameObject>();

    void Awake()
    {
        Instance = this;
    }
}
