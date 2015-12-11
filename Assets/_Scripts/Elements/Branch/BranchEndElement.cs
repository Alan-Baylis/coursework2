using System;
using UnityEngine;

public sealed class BranchEndElement : AbstractElement {
    public const string BranchEndId = "-100";
    private readonly ElectricProperties invalidProperties = ElectricProperties.CreateFromUR(0, 1);
    private static BranchEndElement branchEnd;
    public static BranchEndElement BranchEnd { get { return new BranchEndElement(); } }

    public override ElectricProperties Properties
    {
        get { return invalidProperties; }
        protected set { }
    }

    private BranchEndElement() : base(null)
    {
        //ElementName = BranchEndId;
    }

    public override Rect DragableRect
    {
        get { throw new NotImplementedException(); }
    }

    public override void Draw()
    {
        throw new NotImplementedException();
    }

    public override string Id
    {
        get { return BranchEndId; }
        set {  }
    }
}
