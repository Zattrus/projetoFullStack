public record LeadListItemDto(
    Guid Id,
    string? ContactFirstName,
    string? ContactFullName,
    string? ContactPhone,
    string? ContactEmail,
    string Category,
    string Suburb,
    string Description,
    decimal Price,
    DateTime CreatedAt,
    LeadStatus Status
) : ILeadProjection;