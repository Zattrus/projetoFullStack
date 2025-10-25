public class FakeEmailService : IEmailService
{
  public async Task NotifyLeadCreatedAsync(Lead lead)
  {
    var path = Path.Combine(AppContext.BaseDirectory, "outbox");
    Directory.CreateDirectory(path);
    var fileName = Path.Combine(path, $"{lead.Id}.txt");
    await File.WriteAllTextAsync(fileName,  
      $"To: vendas@test.com\nSubject: Lead accepted\nBody: Lead {lead.Id} - {lead.ContactFullName} Price: {lead.Price:C}");
  }
}