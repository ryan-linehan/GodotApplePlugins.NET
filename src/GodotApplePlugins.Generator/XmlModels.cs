using System.Xml.Serialization;

namespace GodotApplePlugins.Generator;

/// <summary>
/// XML deserialization models matching Godot's doc class.xsd schema.
/// </summary>

[XmlRoot("class")]
public class XmlGdClass
{
    [XmlAttribute("name")]
    public string Name { get; set; } = "";

    [XmlAttribute("inherits")]
    public string Inherits { get; set; } = "";

    [XmlElement("brief_description")]
    public string BriefDescription { get; set; } = "";

    [XmlElement("description")]
    public string Description { get; set; } = "";

    [XmlArray("methods")]
    [XmlArrayItem("method")]
    public List<XmlGdMethod> Methods { get; set; } = new();

    [XmlArray("members")]
    [XmlArrayItem("member")]
    public List<XmlGdMember> Members { get; set; } = new();

    [XmlArray("signals")]
    [XmlArrayItem("signal")]
    public List<XmlGdSignal> Signals { get; set; } = new();

    [XmlArray("constants")]
    [XmlArrayItem("constant")]
    public List<XmlGdConstant> Constants { get; set; } = new();
}

public class XmlGdMethod
{
    [XmlAttribute("name")]
    public string Name { get; set; } = "";

    [XmlAttribute("qualifiers")]
    public string Qualifiers { get; set; } = "";

    [XmlElement("return")]
    public XmlGdReturn? Return { get; set; }

    [XmlElement("param")]
    public List<XmlGdParam> Params { get; set; } = new();

    [XmlElement("description")]
    public string Description { get; set; } = "";
}

public class XmlGdReturn
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "void";
}

public class XmlGdParam
{
    [XmlAttribute("index")]
    public int Index { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; } = "";

    [XmlAttribute("type")]
    public string Type { get; set; } = "";

    [XmlAttribute("default")]
    public string? Default { get; set; }
}

public class XmlGdMember
{
    [XmlAttribute("name")]
    public string Name { get; set; } = "";

    [XmlAttribute("type")]
    public string Type { get; set; } = "";

    [XmlAttribute("setter")]
    public string? Setter { get; set; }

    [XmlAttribute("getter")]
    public string? Getter { get; set; }

    [XmlAttribute("default")]
    public string? Default { get; set; }

    [XmlText]
    public string Description { get; set; } = "";
}

public class XmlGdSignal
{
    [XmlAttribute("name")]
    public string Name { get; set; } = "";

    [XmlElement("param")]
    public List<XmlGdParam> Params { get; set; } = new();

    [XmlElement("description")]
    public string Description { get; set; } = "";
}

public class XmlGdConstant
{
    [XmlAttribute("name")]
    public string Name { get; set; } = "";

    [XmlAttribute("value")]
    public string Value { get; set; } = "";

    [XmlAttribute("enum")]
    public string? Enum { get; set; }

    [XmlText]
    public string Description { get; set; } = "";
}
