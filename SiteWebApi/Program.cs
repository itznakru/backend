
using ItZnak.Infrastruction.Extentions;
using ItZnak.PatentsDtoLibrary.Services;
using Microsoft.AspNetCore.Cors;

namespace ItZnak.SiteWebApi
{
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        /* add my services */
        builder.Services.AddLogService();
        builder.Services.AddConfigService();
        builder.Services.AddTransient<IMongoDbContextService,MongoDbContextService>();

        builder.Services.AddCors();

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

        app.Run();
    }
}
}