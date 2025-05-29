using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BarberGo.Data;
using BarberGo.Interfaces;
using BarberGo.Repositories;
using BarberGo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication; 
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Npgsql.EntityFrameworkCore.PostgreSQL;



namespace BarberGo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            var builder = WebApplication.CreateBuilder(args);
            
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
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;  // cookies
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;              // só para login externo
            })
                .AddCookie()
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
            

          .AddGoogle(googleOptions =>
          {
              googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
              googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
              googleOptions.CallbackPath = "/signin-google";

              googleOptions.Events.OnRedirectToAuthorizationEndpoint = context =>
              {
                  var env = builder.Environment;

                  // Corrige o domínio na URL de redirecionamento com base no ambiente
                  var redirectUri = context.RedirectUri;

                  if (env.IsDevelopment())
                  {
                      redirectUri = redirectUri.Replace("https://barbergo-api.onrender.com", "https://localhost:7032");
                  }

                  context.Response.Redirect(redirectUri);
                  return Task.CompletedTask;
              };
          });

            // Configuração do CORS
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.WithOrigins("https://barbergo-ui.onrender.com", "http://localhost:5173") 
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
            app.UseHttpsRedirection();

          

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
