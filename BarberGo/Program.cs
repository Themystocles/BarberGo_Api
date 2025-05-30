using System.Text;
using BarberGo.Data;
using BarberGo.Interfaces;
using BarberGo.Repositories;
using BarberGo.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Microsoft.AspNetCore.DataProtection;


namespace BarberGo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            var builder = WebApplication.CreateBuilder(args);

            // Configurar Forwarded Headers para Render (HTTPS reverso)
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
            });

            // DbContext PostgreSQL
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Repositórios e serviços (copiar do seu código)
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(GenericRepositoryServices<>));
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<LoginServices>();
            builder.Services.AddScoped<LoginUserRepository>();
            builder.Services.AddScoped<IWeeklySchedule, WeeklyScheduleRepository>();
            builder.Services.AddScoped<ITodaysCustomers, TodaysCustomers>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

            // Swagger com JWT
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

            // Configurar Redis (cache distribuído + Data Protection)
            var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
            var redis = ConnectionMultiplexer.Connect(redisConnectionString);

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "BarberGo_";
            });

            builder.Services.AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

            // Sessão
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".BarberGo.Session";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.Path = "/";
            });

            // Configurar autenticação JWT + Cookies + Google
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
            .AddCookie(options =>
            {
                options.Cookie.Name = "BarberGo.Auth.Cookie";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
                options.LoginPath = "/auth/login";
                options.LogoutPath = "/auth/logout";
                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode == 200)
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
            })
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                googleOptions.CallbackPath = "/auth/signin-google";
                googleOptions.Events = new OAuthEvents
                {
                    OnRemoteFailure = context =>
                    {
                        Console.WriteLine("Erro no login externo: " + context.Failure?.Message);
                        return Task.CompletedTask;
                    }
                };
            });

            // CORS para seu frontend React hospedado
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
                {
                    policy.WithOrigins("https://barbergo-ui.onrender.com")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            app.UseForwardedHeaders();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
                Secure = CookieSecurePolicy.Always
            });

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();

            // Seu middleware de erro personalizado (caso tenha)
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.Run();
        }
    }
}
