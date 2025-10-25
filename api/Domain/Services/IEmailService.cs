public interface IEmailService
{
  Task NotifyLeadCreatedAsync(Lead lead);
}