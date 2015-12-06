using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PropertiesEditorController : MonoBehaviour {

    const string fieldObjectsPrefix = "Field";

    public class MyUIEditField
    {
        public GameObject Parent { get; protected set; }
        public InputField inputField;
        public Text label;

        public MyUIEditField(GameObject parent)
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

    public List<MyUIEditField> fields = new List<MyUIEditField>();

	// Use this for initialization
	void Start () {
        for (var i = 0; i < 5; ++i)
        {
            var t = transform.FindChild(fieldObjectsPrefix + i).gameObject;
            if (t != null)
            {
                var field = new MyUIEditField(t);
                fields.Add(field);
                Debug.Log(field);
            }
        }

        fields.ForEach(x => x.Placeholder = "no element");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
