using System;

public interface ILeadProjection
{
    Guid Id { get; }
    string? ContactFirstName { get; }
    string? ContactFullName { get; }
    string? ContactPhone { get; }
    string? ContactEmail { get; }
    string Suburb { get; }
    string Category { get; }
    string Description { get; }
    decimal Price { get; }
    DateTime CreatedAt { get; }
    LeadStatus Status { get; }
}
