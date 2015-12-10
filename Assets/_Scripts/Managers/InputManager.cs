using JetBrains.Annotations;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void ChangeInputModeEvent();

    public enum InputMode
    {
        Idle,
        Properties,
        Drag,
        Connect
    }

    public float clickTime = 0.3f;
    public InputMode currentMode;
    private bool isActive;
    public bool isPressed;
    private ElementController newElement;
    public float timer;

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
    public PropertiesEditorController propertiesEditor;

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
    public event ChangeInputModeEvent OnIdleEvent = Instance.HandleOnIdle;
    public event ChangeInputModeEvent OnPropertiesEvent = Instance.HandleOnProperties;
    public event ChangeInputModeEvent OnDragEvent = Instance.HandleOnDrag;
    public event ChangeInputModeEvent OnConnectEvent = Instance.HandleOnConnect;

    protected void SetMode(InputMode mode)
    {
        Debug.LogFormat("Setting mode to {0}", mode.ToString());
        currentMode = mode;
        switch (currentMode)
        {
            case InputMode.Idle:
                OnIdleEvent();
                break;
            case InputMode.Properties:
                OnPropertiesEvent();
                break;
            case InputMode.Connect:
                OnConnectEvent();
                break;
            case InputMode.Drag:
                OnDragEvent();
                break;
        }
    }

    private void HandleOnDrag()
    {
    }

    private void HandleOnConnect()
    {
    }

    private void HandleOnProperties()
    {
        propertiesEditor.SetElectricProperties(ElectricalCircuit.Instance.GetElementByController(CurrentElement).Properties);
        propertiesEditor.SetActive(true);
    }

    private void HandleOnIdle()
    {
        propertiesEditor.ForEach(x => x.Text = "");
        propertiesEditor.SetActive(false);
    }

    // ------------------------------------------------------------------------
    [UsedImplicitly]
    private void Awake()
    {
        Instance = this;
        IsActive = true;
        IsUserControll = true;
        OnIdleEvent += HandleOnIdle;
        OnPropertiesEvent += HandleOnProperties;
        OnConnectEvent += HandleOnConnect;
        OnDragEvent += HandleOnDrag;
    }

    [UsedImplicitly]
    private void Start()
    {
        SetMode(InputMode.Idle);
    }

    [UsedImplicitly]
    private void Update()
    {
        switch (currentMode)
        {
            case InputMode.Idle:
                break;
            case InputMode.Properties:
                break;
            case InputMode.Connect:
                if (CurrentElement != null)
                {
                    HelperClass.DrawConnection(CurrentElement.outPoint.position,
                        Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)));
                }
                break;
            case InputMode.Drag:
                if (isPressed)
                {
                    CurrentElement.transform.position =
                        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                            CurrentElement.transform.position.z));
                }
                else
                {
                    SetMode(InputMode.Properties);
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
        if (timer > clickTime && NewElement != null && isPressed && currentMode == InputMode.Properties)
        {
            SetMode(InputMode.Drag);
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
            case InputMode.Idle:
                if (objectBase != null)
                {
                    SetMode(InputMode.Properties);
                    CurrentElement = objectBase;
                }
                break;
            case InputMode.Properties:
                if (objectBase == null)
                {
                    SetMode(InputMode.Idle);
                    CurrentElement = null;
                }
                else
                {
                    if (objectBase == CurrentElement)
                    {
                        SetMode(InputMode.Connect);
                        CurrentElement = objectBase;
                    }
                    CurrentElement = objectBase;
                }
                break;
            case InputMode.Drag:
                if (objectBase == null)
                {
                    SetMode(InputMode.Idle);
                    CurrentElement = null;
                }
                else
                {
                    SetMode(InputMode.Properties);
                }
                break;
            case InputMode.Connect:
                if (objectBase == null)
                {
                    SetMode(InputMode.Properties);
                }
                else
                {
                    if (CurrentElement != null && CurrentElement != objectBase)
                    {
                        ElectricalCircuit.Instance.Connect(CurrentElement.Id, objectBase.Id);
                    }
                    currentMode = InputMode.Idle;
                    CurrentElement = null;
                }
                break;
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
        // ReSharper disable once InvertIf
        if (Physics.Raycast(ray, out hit, distance))
        {
            Debug.Log(hit.collider.gameObject.name);
            var hitObject = hit.collider.gameObject;
            // ReSharper disable once InvertIf
            if (hitObject.GetComponent<ElementController>())
            {
                Debug.Log("We have the controller");
                return hitObject.GetComponent<ElementController>();
            }
        }
        return null;
    }

    #endregion
}