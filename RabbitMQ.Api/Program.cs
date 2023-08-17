#region

using RabbitMq.Infra.CrossCutting;

#endregion

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.ConfigureDependencyInjection(builder.Configuration);
    
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();