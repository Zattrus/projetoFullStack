public record LeadAcceptedDto(
    Guid Id,
    string? ContactFirstName,
    string ContactFullName,
    string? ContactPhone,
    string? ContactEmail,
    string Suburb,
    string Category,
    string Description,
    decimal Price,
    DateTime CreatedAt,
    LeadStatus Status
) : ILeadProjection;