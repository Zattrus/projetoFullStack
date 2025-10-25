public class Lead
{
  public Guid Id { get; set; }
  public string Category { get; set; } = default!;
  public string Suburb { get; set; } = default!;
  public string Description { get; set; } = default!;
  public decimal Price { get; set; }
  public DateTime CreatedAt { get; set; }
  public LeadStatus Status { get; set; } = LeadStatus.New;


  public string ContactFirstName { get; set; } = default!;
  public string? ContactLastName { get; set; } = default!;
  public string? ContactEmail { get; set; } = default!;
  public string? ContactPhone { get; set; } = default!;


  public string ContactFullName => string.IsNullOrWhiteSpace(ContactLastName) 
      ? ContactFirstName 
      : $"{ContactFirstName} {ContactLastName}";
}