using To2dnd;

public abstract class NodeChildBase : NodeDrawableBase
{
    public NodeBase Parent { get; set; }

    public const int IndexOfNothing = -1;

//    public override string ElementName
//    {
//        get
//        {
//            return Parent.ElementName + HierarchyDelimiter + Parent.Children.IndexOf(this);
//        }
//    }

    [TBeforeSerialization]
    public override void Save()
    {
        base.Save();
        //joints.ForEach(j => j.TargetId = j.Target != null ? j.Target.GetHashCode().ToString() : NothingId);
    }

    public virtual bool Connect(int i, NodeBase target)
    {
        return i != IndexOfNothing && joints[i].Connect(target);
    }

    /// <summary>
    /// Which joint is connected to that target.
    /// If fail, returns <code>IndexOfNothing</code>.
    /// </summary>
    /// <param name="target">target to check conection with</param>
    /// <returns><code>IndexOfNothing</code> if no such target at all, otherwise index of corresponding joint</returns>
    public virtual int Connected(NodeBase target)
    {
        for (var i = 0; i < joints.Count; ++i)
        {
            if (Connected(i, target))
            {
                return i;
            }
        }
        return IndexOfNothing;
    }

    public virtual bool Connected(int i, NodeBase target)
    {
        return 
            joints.Count != 0 &&
            i > 0 &&
            i < joints.Count &&
            joints[i].TargetId == target.Id;
    }
}
