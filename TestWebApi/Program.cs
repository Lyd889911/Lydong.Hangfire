var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddLydongHangfire();//!!!

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseLydongHangfire();//!!!
app.UseAuthorization();

app.MapControllers();

app.Run();
