using ListerSS.Configuration;
using ListerSS.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ListerSS
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
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = ConfigHelper.Authentication.ValidIssuer,
                        ValidAudience = ConfigHelper.Authentication.ValidAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConfigHelper.Authentication.SecretKey)),
                    };

                    options.Events = new JwtBearerEvents
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

            builder.Services.AddSignalR(options => options.AddFilter(new DataFilter()));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllers();
            app.MapHub<ChatHub>("/chat");

            app.Run();
        }
    }
}
