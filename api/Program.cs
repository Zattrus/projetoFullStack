using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<LeadDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddScoped<IEmailService, FakeEmailService>();

builder.Services.AddControllers().AddJsonOptions(o => {
    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o => o.AddPolicy("spa", p => p
    .AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5173")));
    
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<GetLeadsHandler>());

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LeadDbContext>();
    db.Database.Migrate();

    if (!db.Leads.Any(l => l.Status == LeadStatus.New))
    {
        db.Leads.AddRange(
            new Lead {
                Id = Guid.NewGuid(),
                Category = "Plumbing",
                Suburb = "Hilversum",
                Description = "Fix sink",
                Price = 450m,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                Status = LeadStatus.New,
                ContactFirstName = "Ana",
                ContactLastName = "Souza",
                ContactPhone = "+31 6 1111 2222",
                ContactEmail = "ana@test.com"
            },
            new Lead {
                Id = Guid.NewGuid(),
                Category = "Electrical",
                Suburb = "Utrecht",
                Description = "Replace switchboard",
                Price = 650m,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                Status = LeadStatus.New,
                ContactFirstName = "Mark",
                ContactLastName = "Jansen",
                ContactPhone = "+31 6 2222 3333",
                ContactEmail = "mark@test.com"
            }
        );
        db.SaveChanges();
        Console.WriteLine(">>> Seed de leads NEW inserido com sucesso.");
    }
    else
    {
        Console.WriteLine(">>> Seed ignorado: já existem leads.");
    }
}

app.UseSwagger(); 
app.UseSwaggerUI();

app.UseCors("spa");
app.MapControllers();

app.Run();
