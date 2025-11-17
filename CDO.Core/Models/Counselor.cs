namespace CDO.Core.Models;

public record class Counselor(
    int id,
    string name,
    string? email,
    string? phone,
    string? fax,
    string? notes,
    string? secretaryName,
    string? secretaryEmail
    );
