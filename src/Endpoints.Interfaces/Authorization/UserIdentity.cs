namespace Endpoints.Interfaces.Authorization;

public readonly struct UserIdentity
{
    public StudentRole? AsStudent { get; init; }
    public TutorRole? AsTutor { get; init; }
    public required string ExternalTenantId { get; init; }
}

public static class UserIdentityExtensions
{
    public static StudentRole RequireStudent(this UserIdentity identity)
    {
        return identity.AsStudent ?? throw new AuthException();
    }

    public static TutorRole RequireTutor(this UserIdentity identity)
    {
        return identity.AsTutor ?? throw new AuthException();
    }
}
