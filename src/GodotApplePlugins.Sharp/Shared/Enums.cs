namespace GodotApplePlugins.Sharp.Shared;

/// <summary>
/// Status codes returned by StoreKit operations.
/// </summary>
public enum StoreKitStatus
{
    /// <summary>The operation completed successfully.</summary>
    Success = 0,
    /// <summary>The user cancelled the operation.</summary>
    UserCancelled = 1,
    /// <summary>The operation is pending (e.g., awaiting parental approval).</summary>
    Pending = 2,
    /// <summary>The user is not authorized to make purchases.</summary>
    NotAuthorized = 3,
    /// <summary>The product is not available in the current storefront.</summary>
    NotAvailable = 4,
    /// <summary>A network error occurred.</summary>
    NetworkError = 5,
    /// <summary>An unknown error occurred.</summary>
    Unknown = 6
}

/// <summary>
/// Verification error codes for StoreKit transactions.
/// </summary>
public enum StoreKitVerificationError
{
    /// <summary>No error.</summary>
    None = 0,
    /// <summary>The signature is invalid.</summary>
    InvalidSignature = 1,
    /// <summary>The chain is missing.</summary>
    MissingChain = 2,
    /// <summary>The certificate chain is invalid.</summary>
    InvalidCertificateChain = 3,
    /// <summary>The device verification failed.</summary>
    InvalidDeviceVerification = 4,
    /// <summary>The environment mismatch.</summary>
    InvalidEnvironment = 5,
    /// <summary>Unknown error.</summary>
    Unknown = 6
}

/// <summary>
/// Location options for the Game Center access point.
/// </summary>
public enum GKAccessPointLocation
{
    /// <summary>Top-left corner.</summary>
    TopLeading = 0,
    /// <summary>Top-right corner.</summary>
    TopTrailing = 1,
    /// <summary>Bottom-left corner.</summary>
    BottomLeading = 2,
    /// <summary>Bottom-right corner.</summary>
    BottomTrailing = 3
}

/// <summary>
/// Time scope for leaderboard queries.
/// </summary>
public enum GKLeaderboardTimeScope
{
    /// <summary>Today only.</summary>
    Today = 0,
    /// <summary>This week.</summary>
    Week = 1,
    /// <summary>All time.</summary>
    AllTime = 2
}

/// <summary>
/// Player scope for leaderboard queries.
/// </summary>
public enum GKLeaderboardPlayerScope
{
    /// <summary>Global leaderboard.</summary>
    Global = 0,
    /// <summary>Friends only.</summary>
    FriendsOnly = 1
}

/// <summary>
/// Leaderboard type.
/// </summary>
public enum GKLeaderboardType
{
    /// <summary>Classic leaderboard.</summary>
    Classic = 0,
    /// <summary>Recurring leaderboard.</summary>
    Recurring = 1,
    /// <summary>Unknown type.</summary>
    Unknown = 2
}

/// <summary>
/// Photo size for loading player photos.
/// </summary>
public enum GKPlayerPhotoSize
{
    /// <summary>Small photo.</summary>
    Small = 0,
    /// <summary>Normal photo.</summary>
    Normal = 1
}

/// <summary>
/// Real user status for Sign in with Apple.
/// </summary>
public enum ASUserDetectionStatus
{
    /// <summary>Status is unsupported.</summary>
    Unsupported = 0,
    /// <summary>Status is unknown.</summary>
    Unknown = 1,
    /// <summary>User is likely a real person.</summary>
    LikelyReal = 2
}

/// <summary>
/// Authorization scopes for Sign in with Apple.
/// </summary>
public static class ASAuthorizationScopes
{
    /// <summary>Request access to the user's email.</summary>
    public const string Email = "email";

    /// <summary>Request access to the user's full name.</summary>
    public const string FullName = "full_name";
}
