using ListerSS.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ListerSS
{
    public class Program
    {
        public static Func<MessageReceivedContext, Task> JwtEventHandler = context =>
        {
            var token = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(token) && path.StartsWithSegments("/chat"))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        };
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //builder.WebHost.UseUrls("http://0.0.0.0:8080");

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "MyServer",
                        ValidateAudience = true,
                        ValidAudience = "MyClient",
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKey")),
                        ValidateIssuerSigningKey = true
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = JwtEventHandler
                    };
                });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.UseAuthorization();


            app.MapControllers();
            app.MapHub<ChatHub>("/chat");

            app.Run();
        }
    }
}
