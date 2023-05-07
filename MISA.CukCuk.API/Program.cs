using Microsoft.AspNetCore.Mvc;
using MISA.Testamis.BL;
using MISA.Testamis.Common.Database;
using MISA.Testamis.Common.Entitis;
using MISA.Testamis.DL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
builder.Services.AddScoped<IEmployeeDL, EmployeeDL>();
builder.Services.AddScoped<IEmployeeBL, EmployeeBL>();
builder.Services.AddScoped<IMissionallowanceDL, MissionallowanceDL>();
builder.Services.AddScoped<IMissionallowanceBL, MissionallowanceBL>();
builder.Services.AddScoped<IDatabase, Database>();


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.Converters.Add(new DateTimeHandler());
}
        ); 
builder.Services.AddCors(p => p.AddPolicy("MyCors", build =>
        {
            build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
        }));


DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString("SqlConnection");

builder.Services.AddCors(p => p.AddPolicy("corspolicy", build => { 
    build.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader();
}));

// enable single domain
// multiple domain
// any demain


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("corspolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
