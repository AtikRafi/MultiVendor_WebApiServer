using System;
using System.Security.Claims;

public static class ClaimsExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        // Try to get the claim (standard Identity claim or custom "UserId")
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? user.FindFirst("UserId")?.Value;

        // Throw exception if claim is not found
        if (string.IsNullOrEmpty(userId))
            throw new Exception("UserId not found in claims");

        return userId;
    }
}
