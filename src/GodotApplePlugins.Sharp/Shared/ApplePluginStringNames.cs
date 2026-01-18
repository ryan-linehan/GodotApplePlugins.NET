using Godot;

namespace GodotApplePlugins.Sharp.Shared;

/// <summary>
/// Contains all StringName constants used for calling methods and connecting signals
/// on the GodotApplePlugins GDExtension classes.
/// </summary>
public static class ApplePluginStringNames
{
    #region GameCenterManager

    /// <summary>Method: authenticate()</summary>
    public static readonly StringName Authenticate = new("authenticate");

    /// <summary>Property getter: get_local_player()</summary>
    public static readonly StringName GetLocalPlayer = new("get_local_player");

    /// <summary>Property getter: get_access_point()</summary>
    public static readonly StringName GetAccessPoint = new("get_access_point");

    /// <summary>Signal: authentication_result(status: bool)</summary>
    public static readonly StringName AuthenticationResultSignal = new("authentication_result");

    /// <summary>Signal: authentication_error(message: String)</summary>
    public static readonly StringName AuthenticationErrorSignal = new("authentication_error");

    #endregion

    #region GKLocalPlayer

    /// <summary>Method: load_friends()</summary>
    public static readonly StringName LoadFriends = new("load_friends");

    /// <summary>Method: load_recent_friends()</summary>
    public static readonly StringName LoadRecentFriends = new("load_recent_friends");

    /// <summary>Method: load_challengeable_friends()</summary>
    public static readonly StringName LoadChallengeableFriends = new("load_challengeable_friends");

    /// <summary>Method: fetch_saved_games(callback)</summary>
    public static readonly StringName FetchSavedGames = new("fetch_saved_games");

    /// <summary>Method: save_game_data(data, name)</summary>
    public static readonly StringName SaveGameData = new("save_game_data");

    /// <summary>Method: delete_saved_games(name)</summary>
    public static readonly StringName DeleteSavedGames = new("delete_saved_games");

    /// <summary>Method: fetch_items_for_identity_verification_signature()</summary>
    public static readonly StringName FetchItemsForIdentityVerificationSignature = new("fetch_items_for_identity_verification_signature");

    /// <summary>Property getter: is_authenticated</summary>
    public static readonly StringName IsAuthenticated = new("is_authenticated");

    /// <summary>Property getter: is_underage</summary>
    public static readonly StringName IsUnderage = new("is_underage");

    /// <summary>Property getter: is_multiplayer_gaming_restricted</summary>
    public static readonly StringName IsMultiplayerGamingRestricted = new("is_multiplayer_gaming_restricted");

    /// <summary>Property getter: is_personalized_communication_restricted</summary>
    public static readonly StringName IsPersonalizedCommunicationRestricted = new("is_personalized_communication_restricted");

    #endregion

    #region GKAccessPoint

    /// <summary>Method: trigger(done)</summary>
    public static readonly StringName Trigger = new("trigger");

    /// <summary>Method: trigger_with_state(state, done)</summary>
    public static readonly StringName TriggerWithState = new("trigger_with_state");

    /// <summary>Method: trigger_with_achievement(achievementID, done)</summary>
    public static readonly StringName TriggerWithAchievement = new("trigger_with_achievement");

    /// <summary>Method: trigger_with_leaderboard(leaderboardID, playerScope, timeScope, done)</summary>
    public static readonly StringName TriggerWithLeaderboard = new("trigger_with_leaderboard");

    /// <summary>Method: trigger_with_leaderboard_set(leaderboardSetID, done)</summary>
    public static readonly StringName TriggerWithLeaderboardSet = new("trigger_with_leaderboard_set");

    /// <summary>Method: trigger_with_player(player, done)</summary>
    public static readonly StringName TriggerWithPlayer = new("trigger_with_player");

    /// <summary>Property: active</summary>
    public static readonly StringName Active = new("active");

    /// <summary>Property: location</summary>
    public static readonly StringName Location = new("location");

    /// <summary>Property: show_highlights</summary>
    public static readonly StringName ShowHighlights = new("show_highlights");

    /// <summary>Property getter: visible</summary>
    public static readonly StringName Visible = new("visible");

    /// <summary>Property getter: is_presenting_game_center</summary>
    public static readonly StringName IsPresentingGameCenter = new("is_presenting_game_center");

    /// <summary>Property getter: frame_in_screen_coordinates</summary>
    public static readonly StringName FrameInScreenCoordinates = new("frame_in_screen_coordinates");

    #endregion

    #region GKAchievement

    /// <summary>Static method: load_achievements(callback)</summary>
    public static readonly StringName LoadAchievements = new("load_achievements");

    /// <summary>Static method: report_achievement(achievements, callback)</summary>
    public static readonly StringName ReportAchievement = new("report_achievement");

    /// <summary>Static method: reset_achievements(callback)</summary>
    public static readonly StringName ResetAchievements = new("reset_achievements");

    /// <summary>Property: identifier</summary>
    public static readonly StringName Identifier = new("identifier");

    /// <summary>Property: percent_complete</summary>
    public static readonly StringName PercentComplete = new("percent_complete");

    /// <summary>Property: shows_completion_banner</summary>
    public static readonly StringName ShowsCompletionBanner = new("shows_completion_banner");

    /// <summary>Property getter: is_completed</summary>
    public static readonly StringName IsCompleted = new("is_completed");

    /// <summary>Property getter: last_reported_date</summary>
    public static readonly StringName LastReportedDate = new("last_reported_date");

    /// <summary>Property getter: player</summary>
    public static readonly StringName Player = new("player");

    #endregion

    #region GKLeaderboard

    /// <summary>Static method: load_leaderboards(ids, callback)</summary>
    public static readonly StringName LoadLeaderboards = new("load_leaderboards");

    /// <summary>Method: submit_score(score, context, player)</summary>
    public static readonly StringName SubmitScore = new("submit_score");

    /// <summary>Method: load_entries(players, timeScope, callback)</summary>
    public static readonly StringName LoadEntries = new("load_entries");

    /// <summary>Method: load_local_player_entries(playerScope, timeScope, start, length, callback)</summary>
    public static readonly StringName LoadLocalPlayerEntries = new("load_local_player_entries");

    /// <summary>Method: load_image(callback)</summary>
    public static readonly StringName LoadImage = new("load_image");

    /// <summary>Property getter: title</summary>
    public static readonly StringName Title = new("title");

    /// <summary>Property getter: group_identifier</summary>
    public static readonly StringName GroupIdentifier = new("group_identifier");

    /// <summary>Property getter: type</summary>
    public static readonly StringName Type = new("type");

    /// <summary>Property getter: start_date</summary>
    public static readonly StringName StartDate = new("start_date");

    /// <summary>Property getter: next_start_date</summary>
    public static readonly StringName NextStartDate = new("next_start_date");

    /// <summary>Property getter: duration</summary>
    public static readonly StringName Duration = new("duration");

    #endregion

    #region StoreKitManager

    /// <summary>Method: request_products(productIds)</summary>
    public static readonly StringName RequestProducts = new("request_products");

    /// <summary>Method: purchase(product)</summary>
    public static readonly StringName Purchase = new("purchase");

    /// <summary>Method: purchase_with_options(product, options)</summary>
    public static readonly StringName PurchaseWithOptions = new("purchase_with_options");

    /// <summary>Method: restore_purchases()</summary>
    public static readonly StringName RestorePurchases = new("restore_purchases");

    /// <summary>Method: fetch_current_entitlements()</summary>
    public static readonly StringName FetchCurrentEntitlements = new("fetch_current_entitlements");

    /// <summary>Signal: products_request_completed(products, status)</summary>
    public static readonly StringName ProductsRequestCompletedSignal = new("products_request_completed");

    /// <summary>Signal: purchase_completed(transaction, status, message)</summary>
    public static readonly StringName PurchaseCompletedSignal = new("purchase_completed");

    /// <summary>Signal: purchase_intent(product)</summary>
    public static readonly StringName PurchaseIntentSignal = new("purchase_intent");

    /// <summary>Signal: restore_completed(status, message)</summary>
    public static readonly StringName RestoreCompletedSignal = new("restore_completed");

    /// <summary>Signal: transaction_updated(transaction)</summary>
    public static readonly StringName TransactionUpdatedSignal = new("transaction_updated");

    /// <summary>Signal: unverified_transaction_updated(transaction, verification_error)</summary>
    public static readonly StringName UnverifiedTransactionUpdatedSignal = new("unverified_transaction_updated");

    /// <summary>Signal: supscription_update(status)</summary>
    public static readonly StringName SubscriptionUpdateSignal = new("supscription_update");

    #endregion

    #region StoreProduct

    /// <summary>Property getter: product_id</summary>
    public static readonly StringName ProductId = new("product_id");

    /// <summary>Property getter: display_name</summary>
    public static readonly StringName DisplayName = new("display_name");

    /// <summary>Property getter: description_value</summary>
    public static readonly StringName DescriptionValue = new("description_value");

    /// <summary>Property getter: price</summary>
    public static readonly StringName Price = new("price");

    /// <summary>Property getter: display_price</summary>
    public static readonly StringName DisplayPrice = new("display_price");

    /// <summary>Property getter: is_family_shareable</summary>
    public static readonly StringName IsFamilyShareable = new("is_family_shareable");

    /// <summary>Property getter: json_representation</summary>
    public static readonly StringName JsonRepresentation = new("json_representation");

    #endregion

    #region StoreTransaction

    /// <summary>Property getter: transaction_id</summary>
    public static readonly StringName TransactionId = new("transaction_id");

    /// <summary>Property getter: original_transaction_id</summary>
    public static readonly StringName OriginalTransactionId = new("original_transaction_id");

    /// <summary>Property getter: product_id (reuse from StoreProduct)</summary>
    // ProductId is already defined above

    /// <summary>Property getter: purchase_date</summary>
    public static readonly StringName PurchaseDate = new("purchase_date");

    /// <summary>Property getter: expiration_date</summary>
    public static readonly StringName ExpirationDate = new("expiration_date");

    #endregion

    #region ASAuthorizationController

    /// <summary>Method: signin()</summary>
    public static readonly StringName Signin = new("signin");

    /// <summary>Method: signin_with_scopes(scopes)</summary>
    public static readonly StringName SigninWithScopes = new("signin_with_scopes");

    /// <summary>Signal: authorization_completed(credential)</summary>
    public static readonly StringName AuthorizationCompletedSignal = new("authorization_completed");

    /// <summary>Signal: authorization_failed(message)</summary>
    public static readonly StringName AuthorizationFailedSignal = new("authorization_failed");

    #endregion

    #region ASAuthorizationAppleIDCredential

    /// <summary>Property getter: user</summary>
    public static readonly StringName User = new("user");

    /// <summary>Property getter: email</summary>
    public static readonly StringName Email = new("email");

    /// <summary>Property getter: full_name</summary>
    public static readonly StringName FullName = new("full_name");

    /// <summary>Property getter: identity_token</summary>
    public static readonly StringName IdentityToken = new("identity_token");

    /// <summary>Property getter: authorization_code</summary>
    public static readonly StringName AuthorizationCode = new("authorization_code");

    /// <summary>Property getter: real_user_status</summary>
    public static readonly StringName RealUserStatus = new("real_user_status");

    #endregion

    #region GKPlayer

    /// <summary>Property getter: game_player_id</summary>
    public static readonly StringName GamePlayerId = new("game_player_id");

    /// <summary>Property getter: team_player_id</summary>
    public static readonly StringName TeamPlayerId = new("team_player_id");

    /// <summary>Property getter: display_name (reuse from StoreProduct)</summary>
    // DisplayName is already defined above

    /// <summary>Property getter: alias</summary>
    public static readonly StringName Alias = new("alias");

    /// <summary>Property getter: is_invited_to_game</summary>
    public static readonly StringName IsInvitedToGame = new("is_invited_to_game");

    /// <summary>Method: load_photo(size, callback)</summary>
    public static readonly StringName LoadPhoto = new("load_photo");

    #endregion
}
