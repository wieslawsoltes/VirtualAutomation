using Avalonia;

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
