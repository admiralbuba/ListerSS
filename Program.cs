using Lister.Persistence.Database;
using Lister.WebApi.Configuration;
using Lister.WebApi.Models.Response;
using Lister.WebApi.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

            ConfigHelper.Authentication = builder.Configuration.GetSection("Authentication").Get<Authentication>();
            ConfigHelper.Logging = builder.Configuration.GetSection("Logging").Get<Logging>();

            builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();
            builder.Services.AddControllers();
            //builder.Services.AddSingleton<IConnectionMultiplexer>(opt => ConnectionMultiplexer.Connect("localhost:6379"));
            builder.Services.AddDbContext<ListerContext>(opt => opt.UseSqlite());
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = ConfigHelper.Authentication.ValidIssuer,
                        ValidAudience = ConfigHelper.Authentication.ValidAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConfigHelper.Authentication.SecretKey)),
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
            builder.Services.AddSignalR(options => options.AddFilter<DataFilter>());
            builder.Services.AddAutoMapper(typeof(Program));

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

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllers();
            app.MapHub<ChatHub>("/chat");

            app.Run();
        }
    }
}
