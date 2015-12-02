using UnityEngine;
using System.Collections;

public sealed class BranchEndElement : AbstractElement {
    public const string BranchEndId = "end";
    private static BranchEndElement branchEnd;
    public static BranchEndElement BranchEnd { get { return branchEnd ?? (branchEnd = new BranchEndElement()); } }

    protected BranchEndElement() : base(null)
    {
        Id = BranchEndId;
    }

    public override Rect DragableRect
    {
        get { throw new System.NotImplementedException(); }
    }

    public override void Draw()
    {
        throw new System.NotImplementedException();
    }
}
