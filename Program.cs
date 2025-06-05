using AutoRentNet.Data;
using AutoRentNet.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────  SERVICES  ─────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Json: ignoruj cykle referencji (gdyby pełne encje trafiały do JSON-a)
builder.Services.AddControllers()
       .AddJsonOptions(o =>
           o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default")
                 ?? "Data Source=autorent.db"));

// ─────────────────────────  BUILD  ────────────────────────────
var app = builder.Build();

// ──────────────────────  MIDDLEWARE  ──────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ** statyczne pliki z katalogu wwwroot **
app.UseDefaultFiles();   // szuka index.html
app.UseStaticFiles();    // serwuje css/js/img

// ──────────────────  MIGRACJA + SEED  ────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ─────────────────────  ENDPOINTS  ────────────────────────────
var api = app.MapGroup("/api");

// ── pojazdy
api.MapGet("/vehicles", async (AppDbContext db) =>
    await db.Vehicles.OrderBy(v => v.Name).ToListAsync());

// ── rezerwacje – GET
api.MapGet("/reservations", async (AppDbContext db) =>
    await db.Reservations
        .Include(r => r.Vehicle)
        .OrderBy(r => r.PickupDateTime)
        .Select(r => new ReservationReadDto(
            r.Id,
            r.VehicleId,
            r.Vehicle.Name,
            r.PickupLocation,
            r.DropoffLocation,
            r.PickupDateTime,
            r.DropoffDateTime,
            r.CustomerName))
        .ToListAsync());

// ── rezerwacje – POST
api.MapPost("/reservations", async (ReservationDto dto, AppDbContext db) =>
{
    var (ok, error, entity) = await ReservationLogic.CreateAsync(dto, db);
    return ok
        ? Results.Created($"/api/reservations/{entity!.Id}", entity)
        : Results.BadRequest(new { error });
});

app.Run();