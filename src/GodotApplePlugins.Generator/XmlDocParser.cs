using System.Xml.Linq;

namespace GodotApplePlugins.Generator;

/// <summary>
/// Parses Godot XML documentation files into GdClass models.
/// </summary>
public static class XmlDocParser
{
    /// <summary>
    /// Parses all XML files in a directory.
    /// </summary>
    public static List<GdClass> ParseDirectory(string path)
    {
        var classes = new List<GdClass>();

        foreach (var file in Directory.GetFiles(path, "*.xml"))
        {
            try
            {
                var gdClass = ParseFile(file);
                if (gdClass != null)
                {
                    classes.Add(gdClass);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error parsing {file}: {ex.Message}");
            }
        }

        return classes;
    }

    /// <summary>
    /// Parses a single XML documentation file.
    /// </summary>
    public static GdClass? ParseFile(string path)
    {
        var doc = XDocument.Load(path);
        var classElement = doc.Root;

        if (classElement?.Name != "class")
            return null;

        var gdClass = new GdClass
        {
            Name = classElement.Attribute("name")?.Value ?? "",
            Inherits = classElement.Attribute("inherits")?.Value ?? "RefCounted",
            Description = GetDescription(classElement.Element("brief_description"))
                         ?? GetDescription(classElement.Element("description"))
                         ?? ""
        };

        // Parse methods
        var methodsElement = classElement.Element("methods");
        if (methodsElement != null)
        {
            foreach (var method in methodsElement.Elements("method"))
            {
                gdClass.Methods.Add(ParseMethod(method));
            }
        }

        // Parse members/properties
        var membersElement = classElement.Element("members");
        if (membersElement != null)
        {
            foreach (var member in membersElement.Elements("member"))
            {
                gdClass.Properties.Add(ParseProperty(member));
            }
        }

        // Parse signals
        var signalsElement = classElement.Element("signals");
        if (signalsElement != null)
        {
            foreach (var signal in signalsElement.Elements("signal"))
            {
                gdClass.Signals.Add(ParseSignal(signal));
            }
        }

        // Parse constants
        var constantsElement = classElement.Element("constants");
        if (constantsElement != null)
        {
            foreach (var constant in constantsElement.Elements("constant"))
            {
                gdClass.Constants.Add(ParseConstant(constant));
            }
        }

        return gdClass;
    }

    private static GdMethod ParseMethod(XElement element)
    {
        var method = new GdMethod
        {
            Name = element.Attribute("name")?.Value ?? "",
            Description = GetDescription(element.Element("description")) ?? "",
            IsStatic = element.Attribute("qualifiers")?.Value?.Contains("static") ?? false
        };

        // Parse return type
        var returnElement = element.Element("return");
        if (returnElement != null)
        {
            method.ReturnType = returnElement.Attribute("type")?.Value ?? "void";
        }

        // Parse parameters
        foreach (var param in element.Elements("param"))
        {
            method.Parameters.Add(new GdParameter
            {
                Index = int.Parse(param.Attribute("index")?.Value ?? "0"),
                Name = param.Attribute("name")?.Value ?? "",
                Type = param.Attribute("type")?.Value ?? "Variant",
                DefaultValue = param.Attribute("default")?.Value
            });
        }

        // Sort parameters by index
        method.Parameters = method.Parameters.OrderBy(p => p.Index).ToList();

        return method;
    }

    private static GdProperty ParseProperty(XElement element)
    {
        return new GdProperty
        {
            Name = element.Attribute("name")?.Value ?? "",
            Type = element.Attribute("type")?.Value ?? "Variant",
            Description = element.Value?.Trim() ?? "",
            DefaultValue = element.Attribute("default")?.Value,
            Getter = element.Attribute("getter")?.Value,
            Setter = element.Attribute("setter")?.Value,
            HasSetter = element.Attribute("setter") != null
        };
    }

    private static GdSignal ParseSignal(XElement element)
    {
        var signal = new GdSignal
        {
            Name = element.Attribute("name")?.Value ?? "",
            Description = GetDescription(element.Element("description")) ?? ""
        };

        foreach (var param in element.Elements("param"))
        {
            signal.Parameters.Add(new GdParameter
            {
                Index = int.Parse(param.Attribute("index")?.Value ?? "0"),
                Name = param.Attribute("name")?.Value ?? "",
                Type = param.Attribute("type")?.Value ?? "Variant"
            });
        }

        signal.Parameters = signal.Parameters.OrderBy(p => p.Index).ToList();

        return signal;
    }

    private static GdConstant ParseConstant(XElement element)
    {
        return new GdConstant
        {
            Name = element.Attribute("name")?.Value ?? "",
            Value = element.Attribute("value")?.Value ?? "0",
            Description = element.Value?.Trim() ?? "",
            EnumName = element.Attribute("enum")?.Value
        };
    }

    private static string? GetDescription(XElement? element)
    {
        var text = element?.Value?.Trim();
        if (string.IsNullOrEmpty(text))
            return null;

        return CleanDescription(text);
    }

    private static string CleanDescription(string description)
    {
        // Remove Godot BBCode-like tags
        var cleaned = description
            .Replace("[code]", "'")
            .Replace("[/code]", "'")
            .Replace("[b]", "")
            .Replace("[/b]", "")
            .Replace("[i]", "")
            .Replace("[/i]", "");

        // Remove [signal name], [method name] etc references - keep the name
        cleaned = System.Text.RegularExpressions.Regex.Replace(
            cleaned,
            @"\[(signal|method|enum|param|member|constant)\s+(\w+)\]",
            "$2");

        // Remove [ClassName] references - keep the name
        cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, @"\[(\w+)\]", "$1");

        // Normalize whitespace
        cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, @"\s+", " ");

        return cleaned.Trim();
    }
}
