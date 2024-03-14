using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;
using TikTakWebAPI.DAL;

namespace TikTakWebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        builder.Services.AddSingleton<VideoProcess>();

        builder.Services.AddControllers();

        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = long.MaxValue; // Allow very large file uploads
        });

        builder.Services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = long.MaxValue; // if don't set default value is: 30 MB
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        PhysicalFileProvider fileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Videos"));
        string requestPath = "/Videos";

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = fileProvider,
                RequestPath = requestPath,
                EnableDirectoryBrowsing = true,
            });
        }

        //app.UseHttpsRedirection();

        // Set up custom content types - associating file extension to MIME type
        FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();

        // Add new mappings
        provider.Mappings[".m3u8"] = "application/x-msdownload";

        // Enable displaying browser links.
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = fileProvider,
            RequestPath = requestPath,
            ContentTypeProvider = provider,
        });

        app.UseAuthorization();

        app.MapControllers();

        app.UseCors("AllowAllOrigins");

        app.Run();
    }
}
