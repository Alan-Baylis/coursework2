using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public class PropertiesEditorController : MonoBehaviour {

    const string FieldObjectsPrefix = "Field";

    public class MyUiEditField
    {
        public Transform Parent { get; protected set; }
        public InputField inputField;
        public Text label;

        public MyUiEditField(Transform parent)
        {
            Parent = parent;
            inputField = parent.GetComponentInChildren<InputField>();
            parent.transform.FindChild("FieldLabel").GetComponent<Text>();
        }

        public string Text
        {
            get
            {
                return inputField.text;
            }
            set
            {
                inputField.text = value;
            }
        }

        public string LabelText
        {
            get
            {
                return label.text;
            }
            set
            {
                label.text = value;
            }
        }

        public string Placeholder
        {
            get
            {
                return inputField.placeholder.GetComponent<Text>().text;
            }
            set
            {
                inputField.placeholder.GetComponent<Text>().text = value;
            }
        }

        public override string ToString()
        {
            return Parent.name;
        }
    }

    public List<MyUiEditField> fields = new List<MyUiEditField>();

	// Use this for initialization
    [UsedImplicitly]
    void Start () {
        for (var i = 0; i < 5; ++i)
        {
            Transform gameObjectOfFieldParent;
            try
            {
                gameObjectOfFieldParent = transform.FindChild(FieldObjectsPrefix + i);
            }
            catch (NullReferenceException)
            {
                Debug.LogErrorFormat("The object {0} has no {1} child", transform.gameObject.name, FieldObjectsPrefix+i);
                continue;
            }
            if (gameObjectOfFieldParent == null) continue;
            var field = new MyUiEditField(gameObjectOfFieldParent);
            fields.Add(field);
            Debug.LogFormat("{0} obtained", field);
        }
        ResetPlaceholders();
	}

    public void ResetPlaceholders(string s = "no element")
    {
        fields.ForEach(x => x.Placeholder = s);
    }
	
	// Update is called once per frame
    [UsedImplicitly]
    void Update () {
	
	}
}
