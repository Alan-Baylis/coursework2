using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;

public class InputManager : MonoBehaviour
{

    public enum ElementMode
    {
        Idle, Drag, Connect
    }

    public ElementMode currentMode;
    public static InputManager Instance { get; private set; }
    public ElementController CurrentElement { get; protected set; }

    public bool IsActive
    {
        get
        { return isActive; }
        set
        {
            if (value && LockerCount == 0)
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }
        }
    }
    public bool IsUserControll { get; set; }

    public int LockerCount { get; private set; }
    public float clickTime = 0.5f;

    private bool isActive;

    public float timer;
    public bool isPressed;

    // ------------------------------------------------------------------------
    [UsedImplicitly]
    private void Awake()
    {
        Instance = this;
        IsActive = true;
        IsUserControll = true;
        currentMode = ElementMode.Idle;
    }

    [UsedImplicitly]
    private void Update()
    {
        switch (currentMode)
        {
            case ElementMode.Connect:
                HelperClass.DrawConnection(CurrentElement.OutPoint.position, Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)));
                break;
            case ElementMode.Drag:
                if(isPressed)
                    CurrentElement.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4));
                break;
        }
    }

    // ------------------------------------------------------------------------
    [UsedImplicitly]
    private void LateUpdate()
    {
        if (!IsUserControll || !IsActive) return;
        PressLeftButton();
        PressRightButton();
        if (isPressed)
        {
            timer += Time.deltaTime;
        }
    }

    // ------------------------------------------------------------------------
    public void AddLocker()
    {
        LockerCount++;
        IsActive = false;
    }

    // ------------------------------------------------------------------------
    public void RemoveLocker()
    {
        if (LockerCount > 0)
        {
            LockerCount--;
        }
        if (LockerCount == 0)
        {
            IsActive = true;
        }
    }

    // ------------------------------------------------------------------------
    public void PressLeftButton()
    {
        ElementController objectBase = null;
        if (Input.GetMouseButtonDown(0))
        {
            isPressed = true;
            timer = 0;
            objectBase = GetObjectFromCursor();
        }
        // obtain object
        currentMode = (objectBase != null) ? timer > clickTime ? ElementMode.Drag : ElementMode.Connect : ElementMode.Idle;
        if (!Input.GetMouseButtonUp(0) && !Input.GetMouseButtonUp(2)) return;
        
        ParseObject(objectBase);
        isPressed = false;
    }

    private ElementController GetObjectFromCursor()
    {
        var obj = Raycast(Input.mousePosition, float.MaxValue);
        if (obj == null)
        {
            Debug.Log("No Element controller under the cursor");
            return null;
        }
        var parent = obj.transform.parent;
        ElementController ec = null;
        try
        {
            ec = parent.GetComponentInChildren<ElementController>();
        }
        catch (Exception)
        {
            Debug.LogWarningFormat("Cannot find element controller in object called {0}", obj.name);
        }
        return ec;
    }

    // ------------------------------------------------------------------------
    public void PressRightButton()
    {
        if (!Input.GetMouseButtonUp(1)) return;
        Debug.Log("Pressed RMB");
        // put all objects down
    }

    // ------------------------------------------------------------------------
    private void ParseObject(ElementController objectBase)
    {
        CurrentElement = objectBase;
        //Handle the press on an object
        switch (currentMode)
        {
            case ElementMode.Drag:
                if(isPressed)
                    CurrentElement.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4));
                break;
            case ElementMode.Connect:
                if (CurrentElement != null && CurrentElement != objectBase)
                {
                    // Connect to found element
                    ElectricalCircuit.Instance.Connect(CurrentElement.Id, objectBase.Id);
                }
                currentMode = ElementMode.Idle;
                CurrentElement = null;
                break;
        }
    }

    public GameObject Raycast(Vector3 target, float distance)
    {
        var ray = Camera.main.ScreenPointToRay(target);
        /*RaycastHit hit;
        var t = Physics.Raycast(ray, out hit, distance) ? null : hit.collider.gameObject;*/
        return (from i in ElectricalCircuit.Instance.RealElements let objRect = i.GetComponent<Collider>().bounds where objRect.IntersectRay(ray) select i.gameObject).FirstOrDefault();
        //var raycastedObject = hit.collider.gameObject;
    }
}