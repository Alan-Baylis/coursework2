using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ContextMenuManager : MonoBehaviour
{
    public GameObject contextMenuPanel;
    public bool activeOnStart;

    public bool ContextMenuActive
    {
        get { return contextMenuPanel.activeSelf; }
        set
        {
            contextMenuPanel.SetActive(value);
        }
    }

    public static ContextMenuManager Instance { get; set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ContextMenuActive = activeOnStart;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ContextMenuActive = !ContextMenuActive;
        }
    }

    protected GameObject GetButtonGameObjectByIndex(int index)
    {
        return transform.FindChild("Button" + index).gameObject;
    }

    /*public void SetButton(int i, Button button)
    {
        GetButtonGameObjectByIndex(i).
    }*/
}
