using Godot;
using GodotApplePlugins.Sharp.Shared;

namespace GodotApplePlugins.Sharp.Authentication;

/// <summary>
/// C# wrapper for the ASAuthorizationAppleIDCredential GDExtension class.
/// Contains the credentials returned from a successful Sign in with Apple.
/// </summary>
public class AppleIdCredential
{
    private readonly GodotObject _instance;

    /// <summary>
    /// Creates a new AppleIdCredential wrapper around the provided GDExtension instance.
    /// </summary>
    /// <param name="instance">The ASAuthorizationAppleIDCredential GDExtension object.</param>
    public AppleIdCredential(GodotObject instance)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    /// Gets the underlying GDExtension object.
    /// </summary>
    public GodotObject Instance => _instance;

    /// <summary>
    /// The unique user identifier. This is stable for this user+app combination.
    /// </summary>
    public string User => _instance.Get(ApplePluginStringNames.User).AsString();

    /// <summary>
    /// The user's email address, if requested and authorized.
    /// May be a private relay address.
    /// </summary>
    public string? Email
    {
        get
        {
            var result = _instance.Get(ApplePluginStringNames.Email);
            return result.VariantType == Variant.Type.Nil || string.IsNullOrEmpty(result.AsString())
                ? null
                : result.AsString();
        }
    }

    /// <summary>
    /// The user's full name components, if requested and authorized.
    /// </summary>
    public PersonNameComponents? FullName
    {
        get
        {
            var result = _instance.Get(ApplePluginStringNames.FullName);
            if (result.VariantType == Variant.Type.Nil)
                return null;

            if (result.Obj is Godot.Collections.Dictionary dict)
            {
                return new PersonNameComponents
                {
                    GivenName = dict.TryGetValue("given_name", out var given) ? given.AsString() : null,
                    FamilyName = dict.TryGetValue("family_name", out var family) ? family.AsString() : null,
                    MiddleName = dict.TryGetValue("middle_name", out var middle) ? middle.AsString() : null,
                    NamePrefix = dict.TryGetValue("name_prefix", out var prefix) ? prefix.AsString() : null,
                    NameSuffix = dict.TryGetValue("name_suffix", out var suffix) ? suffix.AsString() : null,
                    Nickname = dict.TryGetValue("nickname", out var nickname) ? nickname.AsString() : null
                };
            }

            return null;
        }
    }

    /// <summary>
    /// A JSON Web Token (JWT) that securely communicates user identity to your server.
    /// </summary>
    public string? IdentityToken
    {
        get
        {
            var result = _instance.Get(ApplePluginStringNames.IdentityToken);
            return result.VariantType == Variant.Type.Nil || string.IsNullOrEmpty(result.AsString())
                ? null
                : result.AsString();
        }
    }

    /// <summary>
    /// A short-lived token for server-side validation.
    /// </summary>
    public string? AuthorizationCode
    {
        get
        {
            var result = _instance.Get(ApplePluginStringNames.AuthorizationCode);
            return result.VariantType == Variant.Type.Nil || string.IsNullOrEmpty(result.AsString())
                ? null
                : result.AsString();
        }
    }

    /// <summary>
    /// Indicates the likelihood that the user is a real person.
    /// </summary>
    public ASUserDetectionStatus RealUserStatus =>
        (ASUserDetectionStatus)_instance.Get(ApplePluginStringNames.RealUserStatus).AsInt32();
}

/// <summary>
/// Components of a person's name.
/// </summary>
public class PersonNameComponents
{
    /// <summary>The given (first) name.</summary>
    public string? GivenName { get; set; }

    /// <summary>The family (last) name.</summary>
    public string? FamilyName { get; set; }

    /// <summary>The middle name.</summary>
    public string? MiddleName { get; set; }

    /// <summary>Name prefix (e.g., "Dr.", "Mr.").</summary>
    public string? NamePrefix { get; set; }

    /// <summary>Name suffix (e.g., "Jr.", "III").</summary>
    public string? NameSuffix { get; set; }

    /// <summary>The user's nickname.</summary>
    public string? Nickname { get; set; }

    /// <summary>
    /// Gets the formatted full name.
    /// </summary>
    public string FormattedName
    {
        get
        {
            var parts = new List<string>();
            if (!string.IsNullOrEmpty(NamePrefix)) parts.Add(NamePrefix);
            if (!string.IsNullOrEmpty(GivenName)) parts.Add(GivenName);
            if (!string.IsNullOrEmpty(MiddleName)) parts.Add(MiddleName);
            if (!string.IsNullOrEmpty(FamilyName)) parts.Add(FamilyName);
            if (!string.IsNullOrEmpty(NameSuffix)) parts.Add(NameSuffix);
            return string.Join(" ", parts);
        }
    }
}
