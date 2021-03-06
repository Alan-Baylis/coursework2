﻿using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public class PropertiesEditorController : MonoBehaviour {

    const string FieldObjectsPrefix = "Field";

    [Serializable]
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

    public Text statusText;
    public Text elementName;

    // Use this for initialization
    [UsedImplicitly]
    void Start () {
        if(fields.Count != 0) return;
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
	}

    public void SetButtonsActive(bool value)
    {
        ForEach(x =>
        {
            x.inputField.interactable = value;
        });
    }

    public void ForEach(Action <MyUiEditField> action)
    {
        fields.ForEach(action);
    }

    public void SetElectricProperties(ElectricProperties props)
    {
        fields[0].Text = Math.Round(props.Amperage, 4).ToString();
        fields[1].Text = Math.Round(props.Current, 4).ToString();
        fields[2].Text = Math.Round(props.Resistance, 4).ToString();
    }

    public void ChangeAmperage()
    {
        var amperage = fields[0].Text;
        var element = ElectricalCircuit.Instance.GetElementByController(InputManager.Instance.CurrentElement);
        element.Properties.SetIR(Convert.ToDouble(amperage), element.Properties.Resistance);
        ElectricalCircuit.Instance.ApplyPhysics();
        SetElectricProperties(element.Properties);
    }

    public void ChangeCurrent()
    {
        var current = fields[1].Text;
        var element = ElectricalCircuit.Instance.GetElementByController(InputManager.Instance.CurrentElement);
        element.Properties.SetUR(Convert.ToDouble(current), element.Properties.Resistance);
        ElectricalCircuit.Instance.ApplyPhysics();
        SetElectricProperties(element.Properties);
    }

    public void ChangeResistance()
    {
        var resistance = fields[2].Text;
        var element = ElectricalCircuit.Instance.GetElementByController(InputManager.Instance.CurrentElement);
        element.Properties.SetIR(element.Properties.Amperage, Convert.ToDouble(resistance));
        ElectricalCircuit.Instance.ApplyPhysics();
        SetElectricProperties(element.Properties);
    }
}
