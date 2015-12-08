using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum ElementMode
    {
        Idle,
        Properties,
        Drag,
        Connect
    }

    public float clickTime = 0.3f;
    public ElementMode currentMode;
    private bool isActive;
    public bool isPressed;
    public float timer;
    private ElementController newElement;

    private ElementController NewElement
    {
        get { return newElement; }
        set
        {
            Debug.LogFormat("Setting newElement to {0}", value);
            newElement = value;
        }
    }

    public static InputManager Instance { get; private set; }
    public ElementController CurrentElement { get; protected set; }

    protected void SetMode(ElementMode mode)
    {
        Debug.LogFormat("Setting mode to {0}", mode.ToString());
        currentMode = mode;
    }

    public bool IsActive
    {
        get { return isActive; }
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
    // ------------------------------------------------------------------------
    [UsedImplicitly]
    private void Awake()
    {
        Instance = this;
        IsActive = true;
        IsUserControll = true;
        SetMode(ElementMode.Idle);
    }

    [UsedImplicitly]
    private void Update()
    {
        switch (currentMode)
        {
            case ElementMode.Idle:
                break;
            case ElementMode.Properties:

                break;
            case ElementMode.Connect:
                if (CurrentElement != null)
                {
                    HelperClass.DrawConnection(CurrentElement.outPoint.position,
                        Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)));
                }
                break;
            case ElementMode.Drag:
                if (isPressed)
                    CurrentElement.transform.position =
                        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CurrentElement.transform.position.z));
                else
                {
                    SetMode(ElementMode.Properties);
                }
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
        if (Input.GetMouseButtonDown(0))
        {
            isPressed = true;
            timer = 0;
            NewElement = GetObjectFromCursor();
        }
        if (timer > clickTime && NewElement != null && isPressed && currentMode == ElementMode.Properties)
        {
            SetMode(ElementMode.Drag);
        }
        if (!Input.GetMouseButtonUp(0) && !Input.GetMouseButtonUp(2))
        {
            return;
        }
        HandleClick(NewElement);
        isPressed = false;
    }

    // ------------------------------------------------------------------------
    public void PressRightButton()
    {
        if (!Input.GetMouseButtonUp(1)) return;
        Debug.Log("Pressed RMB");
        // put all objects down
    }

    // ------------------------------------------------------------------------
    private void HandleClick(ElementController objectBase)
    {
        Debug.LogFormat("NewElement == null --> {0}", objectBase == null);
        //Handle the press on an object
        switch (currentMode)
        {
            case ElementMode.Idle:
                if (objectBase != null)
                {
                    SetMode(ElementMode.Properties);
                    CurrentElement = objectBase;
                }
                break;
            case ElementMode.Properties:
                if (objectBase == null)
                {
                    SetMode(ElementMode.Idle);
                    CurrentElement = null;
                }
                else
                {
                    if (objectBase == CurrentElement)
                    {
                        SetMode(ElementMode.Connect);
                        CurrentElement = objectBase;
                    }
                    CurrentElement = objectBase;
                }
                break;
            case ElementMode.Drag:
                /*if (isPressed)
                    CurrentElement.transform.position =
                        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4));*/
                if (objectBase == null)
                {
                    SetMode(ElementMode.Idle);
                    CurrentElement = null;
                }
                else
                {
                    SetMode(ElementMode.Properties);
                }
                break;
            case ElementMode.Connect:
                if (objectBase == null)
                {
                    SetMode(ElementMode.Properties);
                }
                else
                {
                    if (CurrentElement != null && CurrentElement != objectBase)
                    {
                        ElectricalCircuit.Instance.Connect(CurrentElement.Id, objectBase.Id);
                    }
                    currentMode = ElementMode.Idle;
                    CurrentElement = null;
                }
                break;
            /*default:
                currentMode = ElementMode.Idle;
            break;*/
        }
    }

    #region Getting objects from scene

    private ElementController GetObjectFromCursor()
    {
        var obj = Raycast(Input.mousePosition, float.MaxValue);
        if (obj == null)
        {
            Debug.LogWarning("No Element controller under the cursor");
            return null;
        }
        Debug.LogFormat("Raycast got {0}", obj.name);

        return obj;
    }

    public ElementController Raycast(Vector3 target, float distance)
    {
        var ray = Camera.main.ScreenPointToRay(target);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance))
        {
            Debug.Log(hit.collider.gameObject.name);
            var hitObject = hit.collider.gameObject;
            if(hitObject.GetComponent<ElementController>())
            {
                Debug.Log("We have the controller");
                return hitObject.GetComponent<ElementController>();
            }
        }
        return null;
    }

    #endregion

}