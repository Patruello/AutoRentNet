using AutoRentNet.Data;
using AutoRentNet.Dtos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default")
                  ?? "Data Source=autorent.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();   // Tworzy DB. + seed
}

var api = app.MapGroup("/api");

api.MapGet("/vehicles", async (AppDbContext db) =>
    await db.Vehicles.OrderBy(v => v.Name).ToListAsync());

api.MapGet("/reservations", async (AppDbContext db) =>
    await db.Reservations
        .Include(r => r.Vehicle)
        .Select(r => new ReservationReadDto(
            r.Id,
            r.VehicleId,
            r.Vehicle.Name,
            r.StartDate,
            r.EndDate,
            r.CustomerName))
        .ToListAsync());

api.MapPost("/reservations", async (ReservationDto dto, AppDbContext db) =>
{
    var (ok, error, entity) = await ReservationLogic.CreateAsync(dto, db);

    return ok
        ? Results.Created($"/api/reservations/{entity!.Id}", entity)
        : Results.BadRequest(new { error });
});

app.Run();

