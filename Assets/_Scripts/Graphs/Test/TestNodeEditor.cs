using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TestNodeEditor : NodeEditorBase<NodeImpl, NodeChildImpl> {
    [MenuItem ("Tools/Custom/NodeEditor", false, 0)]
    public static void Init()
    {
        TestNodeEditor window = (TestNodeEditor)GetWindow(typeof(TestNodeEditor));
        window.titleContent = new GUIContent("Node Editor");
        window.minSize = new Vector2(1120, 400);
        window.CurrentState = WindowState.None;
        window.NodeFactory = new TestAbstractNodeFactory();
    }

    protected override void CreateNode()
    {
        nodes.Add(NodeFactory.CreateNewNode(contextMenuRect.x, contextMenuRect.y));
    }

    protected override void CreateOut(NodeImpl parent)
    {
        parent.AddChild(NodeFactory.CreateNewChild(parent));
    }

    public override byte[] GetBytes()
    {
        return TBinarySerialization.Serialize(nodes);
    }

    protected override void RemoveNode(NodeImpl node)
    {
        foreach (var child in node.Children)
        {
            child.Parent = null;
        }
        node.Children.Clear();
    }

    protected override void ShowSaveLoadMenu()
    {
        GUILayout.BeginHorizontal(GUILayout.Width(300));
        if (GUILayout.Button("Save"))
        {
            Save();
        }
        if (GUILayout.Button("Load"))
        {
            Load();
        }
        GUILayout.EndHorizontal();
    }

    protected override void DrawNodeActionRectContent()
    {
        GUILayout.Label("->");
    }

    protected override void DrawNodeChildrenActionRectContent()
    {
        GUILayout.Label("->");
    }
}
