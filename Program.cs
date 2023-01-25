using Lister.Persistence.Database;
using Lister.WebApi.Configuration;
using Lister.WebApi.Models.Response;
using Lister.WebApi.Services;
using Lister.WebApi.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace Lister.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var authOpts = builder.Configuration.GetSection("Authentication").Get<Authentication>();

            builder.Services.AddControllers();
            builder.Services.AddSingleton<IConnectionMultiplexer>(opt => ConnectionMultiplexer.Connect(builder.Configuration["RedisUrl"]));
            builder.Services.AddDbContext<ListerContext>(opt => opt.UseSqlite());
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = authOpts.ValidIssuer,
                        ValidAudience = authOpts.ValidAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authOpts.SecretKey)),
                    };

                    opt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Headers["Authorization"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(token) && path.StartsWithSegments("/chat"))
                            {
                                context.Token = token.ToString().Replace("Bearer ", "");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            builder.Services.AddAuthorization();
            builder.Services.AddSignalR(opt => opt.AddFilter<DataFilter>());
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();
            builder.Services.AddSingleton<IJwtUtils, JwtUtils>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler(err =>
            {
                err.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message//"Internal Server Error."
                        }));
                    }
                });
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<ChatHub>("/chat");

            app.Run();
        }
    }
}
