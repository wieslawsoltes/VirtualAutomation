using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace VirtualAutomation;

public class Entity
{
    public Entity(string type, Rect bounds)
    {
        Type = type;
        Bounds = bounds;
    }

    public string Type { get; set; }

    public Rect Bounds { get; set; }
}

public class EntityAutomationPeer : AutomationPeer
{
    private readonly Control _panel;
    private readonly Entity _entity;
    private AutomationPeer? _parent;

    public EntityAutomationPeer(Control panel, Entity entity, AutomationPeer parent)
    {
        _panel = panel;
        _entity = entity;
        _parent = parent;
    }

    protected override IReadOnlyList<AutomationPeer> GetOrCreateChildrenCore()
    {
        return Array.Empty<AutomationPeer>();
    }

    protected override string GetClassNameCore()
    {
        return "Entity";
    }

    protected override AutomationPeer? GetLabeledByCore()
    {
        // ignored
        return null;
    }

    protected override string? GetNameCore()
    {
        return _entity.Type;
    }

    protected override AutomationPeer? GetParentCore()
    {
        return _parent;
    }

    protected override bool HasKeyboardFocusCore()
    {
        // ignored
        return false;
    }

    protected override bool IsContentElementCore()
    {
        // ignored
        return false;
    }

    protected override bool IsControlElementCore()
    {
        // ignored
        return true;
    }

    protected override bool IsEnabledCore()
    {
        // ignored
        return true;
    }

    protected override bool IsKeyboardFocusableCore()
    {
        // ignored
        return false;
    }

    protected override void SetFocusCore()
    {
        // ignored
    }

    protected override bool ShowContextMenuCore()
    {
        // ignored
        return false;
    }

    protected override bool TrySetParent(AutomationPeer? parent)
    {
        _parent = parent;
        return true;
    }

    protected override void BringIntoViewCore()
    {
        // ignored
    }

    protected override string? GetAcceleratorKeyCore()
    {
        // ignored
        return null;
    }

    protected override string? GetAccessKeyCore()
    {
        // ignored
        return null;
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.ListItem;
    }

    protected override string? GetAutomationIdCore()
    {
        // TODO:
        return null;
    }

    protected override Rect GetBoundingRectangleCore()
    {
        // TODO:
        //return _entity.Bounds;
        
        var root = _panel.GetVisualRoot();

        if (root is not Visual rootVisual)
            return default;

        var transform = _panel.TransformToVisual(rootVisual);

        if (!transform.HasValue)
            return default;

        return new Rect(_entity.Bounds.Size).TransformToAABB(transform.Value);
    }

    // Implement other necessary methods and properties, including any pattern support
}

public class DiagramAutomationPeer : ControlAutomationPeer
{
    private List<Entity> _entities;
    
    public DiagramAutomationPeer(Control owner) : base(owner)
    {
        _entities = new List<Entity>
        {
           new Entity("Ellipse", new Rect(0, 0, 100, 100)),
           new Entity("Rectangle", new Rect(200, 0, 70, 70)),
           new Entity("Path", new Rect(0, 200, 50, 50)),
        };
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.List;
    }

    private IReadOnlyList<AutomationPeer>? _children;
    private bool _childrenValid;
    
    //protected override List<AutomationPeer> GetChildrenCore()
    protected override IReadOnlyList<AutomationPeer> GetOrCreateChildrenCore()
    {
        var children = _children ?? Array.Empty<AutomationPeer>();

        if (_childrenValid)
            return children;

        var newChildren = new List<AutomationPeer>();

        foreach (var entity in _entities)
        {
            newChildren.Add(new EntityAutomationPeer(Owner, entity, this));
        }

        _childrenValid = true;
        return _children = newChildren;
    }
}

public class DiagramViewer : Border
{
    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new DiagramAutomationPeer(this);
    }
}

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
}
