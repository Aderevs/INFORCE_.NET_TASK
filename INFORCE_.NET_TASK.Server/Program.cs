
using INFORCE_.NET_TASK.Server.DbLogic;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace INFORCE_.NET_TASK.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSingleton(provider =>
            {
                var connectionString = builder.Configuration["ConnectionString"];
                return new UrlShortenerContext(connectionString);
            });
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ModelProfile>();
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.Events = new CookieAuthenticationEvents
                   {
                       OnRedirectToLogin = context =>
                       {
                           // Suppress the redirect and return 401 status code instead
                           context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                           return Task.CompletedTask;
                       }
                   };
               });
            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.UseAuthorization();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
