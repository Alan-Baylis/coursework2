using JetBrains.Annotations;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void OnInputModeEvent();

    public delegate void WhenInputModeEvent(ElementController controller); // todo rename

    public delegate void UpdateInputOnInputModeEvent(); // todo rename

    public enum InputMode
    {
        Idle,
        Properties,
        Drag,
        Connect
    }

    public float clickTime = 0.7f;
    public InputMode currentMode;
    private bool isActive;
    public bool isPressed;
    private ElementController newElement;
    private ElementController currentElement;
    private ElementController elementFromWhomToConnect;
    public PropertiesEditorController propertiesEditor;
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
    public ElementController CurrentElement
    {
        get { return currentElement; }
        set
        {
            Debug.LogFormat("Setting CurrentElement to {0}", value);
            currentElement = value;
        }
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
    public event OnInputModeEvent OnChangeToIdleEvent;
    public event OnInputModeEvent OnChangeToPropertiesEvent;
    public event OnInputModeEvent OnChangeToDragEvent;
    public event OnInputModeEvent OnChangeToConnectEvent;
    public event WhenInputModeEvent OnClickWhenIdle;
    public event WhenInputModeEvent OnClickWhenProperties;
    public event WhenInputModeEvent OnClickWhenDrag;
    public event WhenInputModeEvent OnClickWhenConnect;
    public event UpdateInputOnInputModeEvent UpdateInIdle;
    public event UpdateInputOnInputModeEvent UpdateInProperties;
    public event UpdateInputOnInputModeEvent UpdateInDrag;
    public event UpdateInputOnInputModeEvent UpdateInConnect;

    protected void SetMode(InputMode mode)
    {
        Debug.LogFormat("Setting mode to {0}", mode.ToString());
        currentMode = mode;
        switch (currentMode)
        {
            case InputMode.Idle:
                if (OnChangeToIdleEvent != null) OnChangeToIdleEvent();
                break;
            case InputMode.Properties:
                if (OnChangeToPropertiesEvent != null) OnChangeToPropertiesEvent();
                break;
            case InputMode.Connect:
                if (OnChangeToConnectEvent != null) OnChangeToConnectEvent();
                break;
            case InputMode.Drag:
                if (OnChangeToDragEvent != null) OnChangeToDragEvent();
                break;
        }
    }

    // ------------------------------------------------------------------------
    [UsedImplicitly]
    private void Awake()
    {
        Instance = this;
        IsActive = true;
        IsUserControll = true;
        OnChangeToIdleEvent += HandleOnChangeToIdle;
        OnChangeToPropertiesEvent += HandleOnChangeToProperties;
        OnChangeToConnectEvent += HandleOnChangeToConnect;
        OnChangeToDragEvent += HandleOnChangeToDrag;

        OnClickWhenIdle += HandleOnClickWhenIdle;
        OnClickWhenProperties += HandleOnClickWhenProperties;
        OnClickWhenDrag += HandleOnClickWhenDrag;
        OnClickWhenConnect += HandleOnClickWhenConnect;

        UpdateInIdle += HandleUpdateInIdle;
        UpdateInProperties += HandleUpdateInProperties;
        UpdateInDrag += HandleUpdateInDrag;
        UpdateInConnect += HandleUpdateInConnect;
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
                if (UpdateInIdle != null) UpdateInIdle();
                break;
            case InputMode.Properties:
                if (UpdateInProperties != null) UpdateInProperties();
                break;
            case InputMode.Connect:
                if (UpdateInConnect != null) UpdateInConnect();
                break;
            case InputMode.Drag:
                if (UpdateInDrag != null) UpdateInDrag();
                break;
        }
        if (currentMode == InputMode.Idle) return;
        if ((Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Delete)))
        {
            NodeDrawableBase.allDrawableBases.RemoveAll(x => x.ToString() == CurrentElement.ElementName);
            ElectricalCircuit.Instance.realElements.Remove(CurrentElement);
            Destroy(CurrentElement.transform.parent.gameObject);
            SetMode(InputMode.Idle);
            ElectricalCircuit.Instance.ApplyPhysics();
            ElectricalCircuit.Instance.UpdatePointsOfConnections();
        }
        else if(Input.GetKeyUp(KeyCode.Plus) || Input.GetKeyUp(KeyCode.KeypadPlus))
        {
            CurrentElement.transform.parent.localScale *= 1.5f;
            ElectricalCircuit.Instance.UpdatePointsOfConnections();
        }
        else if (Input.GetKeyUp(KeyCode.Minus) || Input.GetKeyUp(KeyCode.KeypadMinus))
        {
            CurrentElement.transform.parent.localScale /= 1.5f;
            ElectricalCircuit.Instance.UpdatePointsOfConnections();
        }
    }

    private void HandleClick(ElementController controller)
    {
        Debug.LogFormat("NewElement {0} null", (controller == null) ? "==" : "!=");
        if (controller != null)
        {
            CurrentElement = controller;
        }
        switch (currentMode)
        {
            case InputMode.Idle:
                if (OnClickWhenIdle != null) OnClickWhenIdle(controller);
                break;
            case InputMode.Properties:
                if (OnClickWhenProperties != null) OnClickWhenProperties(controller);
                break;
            case InputMode.Drag:
                if (OnClickWhenDrag != null) OnClickWhenDrag(controller);
                break;
            case InputMode.Connect:
                if (OnClickWhenConnect != null) OnClickWhenConnect(controller);
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
        PressEscape();
        if (isPressed)
        {
            timer += Time.deltaTime;
        }
    }

    private void PressEscape()
    {
        if (!Input.GetKeyUp(KeyCode.Escape)) return;
        isPressed = false;
        SetMode(InputMode.Idle);
        CurrentElement = null;
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
        if (timer > clickTime && NewElement.IsNotNull() && isPressed && currentMode == InputMode.Properties)
        {
            Debug.Log("It is in drag mode now!!!");
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
        SetMode(InputMode.Properties);
        isPressed = false;
    }

    // ------------------------------------------------------------------------

    #region HandleUpdate

    private void HandleUpdateInConnect()
    {
        if (CurrentElement != null)
        {
            /*HelperClass.DrawConnection(CurrentElement.transform.position,
                Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)));*/
        }
    }

    private void HandleUpdateInDrag()
    {
        if (isPressed)
        {
            CurrentElement.transform.position =
                Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    CurrentElement.transform.position.z));
        }
        else
        {
            Debug.LogError("No current element to drag.");
            SetMode(InputMode.Properties);
        }
    }

    private void HandleUpdateInProperties()
    {
    }

    private void HandleUpdateInIdle()
    {
    }

    #endregion

    #region HandleOnClick

    private void HandleOnClickWhenIdle(ElementController controller)
    {
        if (controller == null) return;

        SetMode(InputMode.Properties);
        CurrentElement = controller;
        isPressed = false;
    }

    private void HandleOnClickWhenProperties(ElementController controller)
    {
        if (controller == null) return;
        if (controller == CurrentElement)
        {
            SetMode(InputMode.Connect);
        }
        CurrentElement = controller;
        isPressed = false;
    }

    private void HandleOnClickWhenDrag(ElementController controller)
    {
        SetMode(InputMode.Properties);
        ElectricalCircuit.Instance.UpdatePointsOfConnections();
    }

    private void HandleOnClickWhenConnect(ElementController controller)
    {
        if (controller == null)
        {
            SetMode(InputMode.Properties);
            isPressed = false;
        }
        else
        {
            if (CurrentElement != null && CurrentElement != elementFromWhomToConnect)
            {
                ElectricalCircuit.Instance.Connect(elementFromWhomToConnect.ElementName, CurrentElement.ElementName);
            }
            SetMode(InputMode.Idle);
        }
    }

    #endregion

    #region HandleOnChange

    private void HandleOnChangeToDrag()
    {
        propertiesEditor.statusText.text = "Dragging";
    }

    private void HandleOnChangeToConnect()
    {
        elementFromWhomToConnect = CurrentElement;
        propertiesEditor.statusText.text = string.Format("Connecting {0}...", CurrentElement.ElementName);
    }

    private void HandleOnChangeToProperties()
    {
        propertiesEditor.SetElectricProperties(
            ElectricalCircuit.Instance.GetElementByController(CurrentElement).Properties);
        propertiesEditor.SetButtonsActive(true);
        propertiesEditor.statusText.text = "Editing properties";
        propertiesEditor.elementName.text = CurrentElement.ElementName;
    }

    private void HandleOnChangeToIdle()
    {
        propertiesEditor.elementName.text = "";
        propertiesEditor.SetButtonsActive(false);
        propertiesEditor.ForEach(x => x.Text = "");
        propertiesEditor.statusText.text = "";
        CurrentElement = null;
        isPressed = false;
    }

    #endregion

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
        if (!Physics.Raycast(ray, out hit, distance)) return null;
        //Debug.Log(hit.collider.gameObject.name);
        var hitObject = hit.collider.gameObject;
        if (!hitObject.GetComponent<ElementController>()) return null;
        Debug.Log("We have the controller!");
        return hitObject.GetComponent<ElementController>();
    }

    #endregion
}