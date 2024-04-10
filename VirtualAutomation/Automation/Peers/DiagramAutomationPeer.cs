using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Automation.Peers;
using Avalonia.Controls;

namespace VirtualAutomation;

public class DiagramAutomationPeer : ControlAutomationPeer
{
    private List<Entity> _entities;
    private IReadOnlyList<AutomationPeer>? _children;
    private bool _childrenValid;
    
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
