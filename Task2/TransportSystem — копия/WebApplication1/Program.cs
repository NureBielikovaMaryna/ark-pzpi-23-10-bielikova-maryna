using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;   
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// בה +
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=transport.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// אגעמ 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); 
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();