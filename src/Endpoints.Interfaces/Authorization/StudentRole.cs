namespace Endpoints.Interfaces.Authorization;

// To be created only in tests and authentication code.
public readonly struct StudentRole
{
    public required string ExternalStudentId { get; init; }
}
