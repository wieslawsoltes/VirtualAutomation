using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace VirtualAutomation;

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
        // return _entity.Bounds;
        
        var root = _panel.GetVisualRoot();

        if (root is not Visual rootVisual)
            return default;

        var transform = _panel.TransformToVisual(rootVisual);

        if (!transform.HasValue)
            return default;

        return _entity.Bounds.TransformToAABB(transform.Value);
    }
}
