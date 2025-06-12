namespace Endpoints.Interfaces;

public class BadRequestException(string message) : Exception(message);
