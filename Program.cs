using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Configuration;
using WebApiClientes.Data;
using WebApiClientes.MappingClass;
using WebApiClientes.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<myDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var mapping = Mapping.RegisterMaps().CreateMapper();//estas 3 
builder.Services.AddSingleton(mapping);             //lineas hacen posible 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());//utilizar el mapping

builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();//este service me permite
                                                                      //utilizar los repositorios
                                                                      //e interface para clientes
builder.Services.AddScoped<IUserRepositorio, UserRepositorio>();//este service me permite utilzar
                                                                //utilizar los repositorios
                                                                //e interface para users

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters //estas 
                    {                                                                 //lineas 
                        ValidateIssuerSigningKey = true,                              //hacen
                        IssuerSigningKey = new SymmetricSecurityKey(                  //posible
                            System.Text.Encoding.ASCII.GetBytes(                           //permiten
                                builder.Configuration.GetSection("AppSettings:Token").Value)),  //manejar
                        ValidateIssuer = false,                        // la 
                        ValidateAudience = false                       // autentication

                    };//
                });//

builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<SecurityRequirementsOperationFilter>();

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Authorization Standar, Usar Bearer.  Ejemplo \"bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    }); 
});
builder.Services.AddCors();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();//esta linea permite utilizar autorizacion

app.UseAuthorization();

app.UseCors(x => x.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                    ) ;

app.MapControllers();

app.Run();
