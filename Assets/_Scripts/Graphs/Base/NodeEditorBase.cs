using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public abstract class NodeEditorBase<TNode, TChild> : EditorWindow where TNode : NodeBase where TChild : NodeChildBase 
{
    public enum WindowState : byte
    {
        None, 
        Connect,
        Drag, 
        ContextMenu
    }

    public WindowState CurrentState { get; protected set; }
    protected virtual AbstractNodeFactory<TNode, TChild> NodeFactory { get; set; }
    protected Vector2 scroll;
    protected Rect contextMenuRect;
    protected bool showContextMenu;
    protected Vector2 mouseOffset;
    protected TNode currentNode;
    protected float windowHeight = 200;
    protected float windowWidth = 200;
    protected List<TNode> nodes = new List<TNode>();
    protected Vector2 contextMenuSize = new Vector2(100, 200);
    protected float freeAreaSize = 500;

    public const int LeftMouseButtonNumber = 0;
    public const int RightMouseButtonNumber = 1;

    protected virtual string CreateNodeButtonText { get { return "Create Node"; } }
    protected virtual string CreateChildButtonText { get { return "Create Out"; } }
    protected virtual string FileExtension { get { return "dat"; } }
    protected virtual string DefaultFilename { get { return "node"; } }

    protected virtual void OnGUI()
    {
        ShowSaveLoadMenu();
        scroll = GUILayout.BeginScrollView(scroll);
        var size = Vector2.zero;
        if (nodes.Count > 0)
        {
            foreach (var node in nodes)
            {
                if (node.FullRect.x > size.x)
                {
                    size.x = node.FullRect.x + freeAreaSize;
                }
                if (node.FullRect.y > size.y)
                {
                    size.y = node.FullRect.y + freeAreaSize;
                }
            }
        }
        else
        {
            size.x = freeAreaSize;
            size.y = freeAreaSize;
        }

        windowWidth = size.x;
        windowHeight = size.y;

        GUILayout.Box("", GUI.skin.label, GUILayout.Height(windowHeight), GUILayout.Width(windowWidth));
        Draw();
        ApplyState();
        Repaint();
        //GUILayout.Label(CurrentState.ToString());
        GUILayout.EndScrollView();
    }

    protected virtual void OnDestroy()
    {
        nodes.ForEach(node =>
        {
            node.Children.ForEach(nodeChild =>
            {
                nodeChild.joints.Clear();
            });
            node.Children.Clear();
        });
        nodes.Clear();
    }

    protected abstract void ShowSaveLoadMenu();

    public abstract byte[] GetBytes();

    public virtual void LoadByBytes(byte[] data)
    {
        nodes = TBinarySerialization.Deserialize<List<TNode>>(data);
        ReconnectChildren();
    }

    protected virtual void Save()
    {
        string path = GetSavePath();
        if (path == null) return;
        if (Directory.Exists(Path.GetDirectoryName(path)) || !File.Exists(path))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.Create(path).Close();
        }
        File.WriteAllBytes(path, GetBytes());
    }

    protected virtual void Load()
    {
        string path = GetLoadPath();
        if (path == null || !File.Exists(path)) return;
        if (!File.Exists(path))
        {
            Debug.LogError("There's no such file \"" + path + "\"!");
        }
        else
        {
            LoadByBytes(File.ReadAllBytes(path));
        }
    }

    public virtual void Draw()
    {
        DrawNodes();
        DrawConnections();
    }

    public virtual void DrawNodes()
    {
        foreach (var node in nodes)
        {
            GUILayout.BeginArea(node.FullRect, GUI.skin.box);

            GUILayout.Label("");

            node.Draw();

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(node.FirstJointRect.x + node.Position.x, node.FirstJointRect.y + node.Position.y, node.FirstJointRect.width, node.FirstJointRect.height), GUI.skin.box);
            DrawNodeActionRectContent();
            GUILayout.EndArea();
            var outsRect = node.GetOutsRect();
            GUILayout.BeginArea(new Rect(node.Position.x + outsRect.x, node.Position.y + outsRect.y + node.Size.y, outsRect.width, outsRect.height), GUI.skin.box);
            foreach (TChild child in node.Children)
            {
                GUILayout.BeginArea(new Rect(child.Position.x, child.Position.y, child.Size.x, child.Size.y));
                child.Draw();
                GUILayout.EndArea();
                foreach (var joint in child.joints)
                {
                    GUILayout.BeginArea(new Rect(outsRect.x + joint.ActionRect.x + child.Position.x, outsRect.y + joint.ActionRect.y + child.Position.y, joint.ActionRect.width, joint.ActionRect.height), GUI.skin.box);
                    DrawNodeChildrenActionRectContent();
                    GUILayout.EndArea();
                }
            }
            GUILayout.EndArea();
        }
    }

    public virtual TNode GetById(string id)
    {
        return id == NodeDrawableBase.NothingId ? null : nodes.FirstOrDefault(node => node.Id == id);
    }


    protected abstract void CreateNode();

    protected abstract void CreateOut(TNode parent);

    protected abstract void RemoveNode(TNode node);

    protected virtual bool Connected(TChild child, TNode node)
    {
        return child.Connected(node) != NodeChildBase.IndexOfNothing;
    }

    protected virtual void ApplyState()
    {
        switch (CurrentState)
        {
            case WindowState.None:
                ApplyNone();
                break;
            case WindowState.Connect:
                ApplyConnect();
                break;
            case WindowState.Drag:
                ApplyDrag();
                break;
            case WindowState.ContextMenu:
                ApplyContextMenu();
                break;
        }
    }

    protected virtual string GetSavePath()
    {
        return EditorUtility.SaveFilePanel("Save", Application.dataPath, DefaultFilename, FileExtension);
    }

    protected virtual string GetLoadPath()
    {
        return EditorUtility.OpenFilePanel("Load", Application.dataPath, FileExtension);
    }

    protected virtual void ReconnectChildren()
    {
        foreach (var nodeOut in nodes.SelectMany(node => node.Children.Cast<TChild>()))
        {
            for (var i = 0; i < nodeOut.joints.Count; ++i)
            {
                nodeOut.Connect(i, GetById(nodeOut.joints[i].TargetId));
            }
        }
    }

    protected virtual void DrawContextMenu()
    {
        GUILayout.BeginArea(contextMenuRect, GUI.skin.box);

        if (GUILayout.Button(CreateNodeButtonText))
        {
            CreateNode();
            CurrentState = WindowState.None;
        }
        

        if (currentNode != null && GUILayout.Button(CreateChildButtonText))
        {
            CreateOut(currentNode);
        }

        GUILayout.EndArea();
    }

    protected virtual void ApplyContextMenu()
    {
        if (contextMenuRect.Contains(Event.current.mousePosition) || !showContextMenu)
        {
            if (!showContextMenu)
            {
                CreateContextMenu();
            }
            DrawContextMenu();
        }
        else
        {
            CurrentState = WindowState.None;
            showContextMenu = false;
            currentNode = null;
        }
    }

    protected virtual void CreateContextMenu()
    {
        contextMenuRect = new Rect(Event.current.mousePosition - contextMenuSize * 0.5f, contextMenuSize);

        if (contextMenuRect.x < 0)
        {
            contextMenuRect.x = 0;
        }
        if (contextMenuRect.y < 0)
        {
            contextMenuRect.y = 0;
        }

        if (contextMenuRect.x + contextMenuSize.x > windowWidth)
        {
            contextMenuRect.x = windowWidth - contextMenuSize.x;
        }
        if (contextMenuRect.y + contextMenuSize.y > windowHeight)
        {
            contextMenuRect.y = windowHeight - contextMenuSize.y;
        }

        showContextMenu = true;
    }

    protected virtual void ApplyNone()
    {
        if (Event.current.type != EventType.MouseDown) return;
        switch (Event.current.button)
        {
            case LeftMouseButtonNumber:
                foreach (var node in nodes)
                {
                    if (node.DragableRect.Contains(Event.current.mousePosition)) 
                    {
                        currentNode = node;
                        mouseOffset = new Vector2(currentNode.DragableRect.x - Event.current.mousePosition.x, currentNode.DragableRect.y - Event.current.mousePosition.y);
                        CurrentState = WindowState.Drag;
                        break;
                    }
                    if (!node.PositionMatchesNodeChild(Event.current.mousePosition)) continue;
                    CurrentState = WindowState.Connect;
                    currentNode = node;
                    break;
                }
                break;
            case RightMouseButtonNumber:
                OpenContextMenu();
                break;
        }
    }

    protected virtual void OpenContextMenu()
    {
        CurrentState = WindowState.ContextMenu;

        foreach (var node in nodes.Where(node => new Rect(node.Position, node.Size).Contains(Event.current.mousePosition)))
        {
            currentNode = node;
            break;
        }
    }

    protected virtual void ApplyConnect()
    {
        if (currentNode != null)
        {
            CustomEditorHelper.DrawConnection(currentNode.BezierStartPosition, Event.current.mousePosition);
            if (Event.current.type != EventType.MouseDown) return;
            switch (Event.current.button)
            {
                case 0:
                    var isConnected = false;
                    foreach (var node in nodes)
                    {
                        if (!node.FullRect.Contains(Event.current.mousePosition)) continue;
                        currentNode.CurrentJointPoint.Connect(node);
                        isConnected = true;
                        break;
                    }
                    if (!isConnected)
                    {
                        currentNode.CurrentJointPoint.Connect(null);
                    }
                    CurrentState = WindowState.None;
                    break;
                case 1:
                    CurrentState = WindowState.ContextMenu;
                    break;
            }
        }
        else
        {
            CurrentState = WindowState.None;
        }
    }

    protected virtual void DrawConnections()
    {
        foreach (var node in nodes)
        {
            node.DrawConnections();
        }
    }

    protected virtual void ApplyDrag()
    {
        if (Event.current.type == EventType.MouseDrag && currentNode != null)
        {
            currentNode.Position = new Vector2(Event.current.mousePosition.x + mouseOffset.x, Event.current.mousePosition.y + mouseOffset.y);
        }

        if (Event.current.type != EventType.MouseUp) return;
        currentNode = null;
        CurrentState = WindowState.None;
    }

    protected virtual void DrawNodeActionRectContent()
    {
        
    }

    protected virtual void DrawNodeChildrenActionRectContent()
    {

    }

    protected virtual List<TChild> GetAllChildrenOfAllNodes()
    {
        var childrenOfAllNodes = new List<TChild>();
        nodes.ForEach(node => childrenOfAllNodes.AddRange(node.Children.Cast<TChild>()));
        return childrenOfAllNodes;
    }
}
