using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BarberGo.Data;
using BarberGo.Interfaces;
using BarberGo.Repositories;
using BarberGo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication;

namespace BarberGo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            var builder = WebApplication.CreateBuilder(args);

            // Proxy para Render
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
            });

            // Configuração do DbContext
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configuração dos serviços
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(GenericRepositoryServices<>));
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<LoginServices>();
            builder.Services.AddScoped<LoginUserRepository>();
            builder.Services.AddScoped<IWeeklySchedule, WeeklyScheduleRepository>();
            builder.Services.AddScoped<ITodaysCustomers, TodaysCustomers>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<ICreateUserAdmin, CreateUserAdmin>();
            builder.Services.AddScoped<CreateUserAdminServices>();


            // Configuração do Swagger com JWT
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Autenticação JWT usando Bearer. Exemplo: 'Bearer seu_token_aqui'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Configuração da autenticação JWT + Google
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;



            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            })
            .AddCookie()
            .AddCookie("External")
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                googleOptions.CallbackPath = "/signin-google";
                
                googleOptions.Scope.Clear();
                googleOptions.Scope.Add("openid");
                googleOptions.Scope.Add("profile");
                googleOptions.Scope.Add("email");

                googleOptions.ClaimActions.MapJsonKey("picture", "picture", "url");
            });

            // Configuração do CORS
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173", "https://barbergo-ui.onrender.com") // Permite requisições do React
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            // Adicionar serviços ao container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configurar o pipeline HTTP
           
                app.UseSwagger();
                app.UseSwaggerUI();
            

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins);
            app.UseForwardedHeaders();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}