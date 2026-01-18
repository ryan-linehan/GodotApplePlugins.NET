namespace GodotApplePlugins.Generator;

/// <summary>
/// Represents a parsed GDExtension class from XML documentation.
/// </summary>
public class GdClass
{
    public string Name { get; set; } = "";
    public string Inherits { get; set; } = "";
    public string Description { get; set; } = "";
    public List<GdMethod> Methods { get; set; } = new();
    public List<GdProperty> Properties { get; set; } = new();
    public List<GdSignal> Signals { get; set; } = new();
    public List<GdConstant> Constants { get; set; } = new();
}

/// <summary>
/// Represents a method in a GDExtension class.
/// </summary>
public class GdMethod
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ReturnType { get; set; } = "void";
    public List<GdParameter> Parameters { get; set; } = new();
    public bool IsStatic { get; set; }
}

/// <summary>
/// Represents a property/member in a GDExtension class.
/// </summary>
public class GdProperty
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public string Description { get; set; } = "";
    public string? DefaultValue { get; set; }
    public bool HasGetter { get; set; } = true;
    public bool HasSetter { get; set; }
    public string? Getter { get; set; }
    public string? Setter { get; set; }
}

/// <summary>
/// Represents a signal in a GDExtension class.
/// </summary>
public class GdSignal
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<GdParameter> Parameters { get; set; } = new();
}

/// <summary>
/// Represents a method/signal parameter.
/// </summary>
public class GdParameter
{
    public int Index { get; set; }
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public string? DefaultValue { get; set; }
}

/// <summary>
/// Represents a constant or enum value.
/// </summary>
public class GdConstant
{
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";
    public string Description { get; set; } = "";
    public string? EnumName { get; set; }
}
