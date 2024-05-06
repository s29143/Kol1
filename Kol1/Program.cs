using Kol1.Medicaments;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<IMedicamentService, MedicamentService>();
builder.Services.AddScoped<IMedicamentRepository, MedicamentRepository>();
builder.Services.AddScoped<IPatientsService, PatientsService>();
builder.Services.AddScoped<IPatientsRepository, PatientsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();