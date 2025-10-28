using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using tasksAction.Custom;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Se agrega clase de Utilidades =====================================================================

builder.Services.AddSingleton<Utilities>();
//====================================================================================================
// Configuracion de JWT ==============================================================================

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;    // No vamos a convertir Info Multimedia
    config.SaveToken = true;                // Posibilidad de Guardar Token
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!))
    };
});
//====================================================================================================
// Add services to the container.===========================================
builder.Services.AddCors(option =>
    option.AddPolicy("RulesCors", app =>
        {
            app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    )
);
//==========================================================================

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("RulesCors");

app.UseAuthentication();    // Indicamos a App utilice nuestra Autenticación

app.UseAuthorization();

app.MapControllers();

app.Run();

