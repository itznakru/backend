
using System.Net;
using ItZnak.Infrastruction.Extentions;
using ItZnak.PatentsDtoLibrary.Services;
using Microsoft.AspNetCore.Cors;
using ItZnak.Infrastruction.Web.Middleware;

namespace ItZnak.SiteWebApi
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            const int HTTP_PORT = 5020;
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /* Add my services */
            builder.Services.AddLogService();
            builder.Services.AddConfigService();
            builder.Services.AddTransient<IMongoDbContextService, MongoDbContextService>();

            /* Config port and add access politic.  */
            builder.WebHost.ConfigureKestrel(options => options.Listen(IPAddress.Any, HTTP_PORT));
            /* Add cors !SET POLITIC */
            builder.Services.AddCors();

            /* APP PIPILINE */
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            /* GLOBAL EXCEPTION WRAPPER */
            app.UseGlobalExceptionMdl();

            app.Run();
        }
    }
}