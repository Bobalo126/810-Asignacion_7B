using _810_Asignacion_7B.Config;
using _810_Asignacion_7B.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuración desde appsettings.json
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Bind de settings
var ftpSettings = new FtpSettings();
builder.Configuration.GetSection("Ftp").Bind(ftpSettings);

var informeSettings = new InformeSettings();
builder.Configuration.GetSection("Informes").Bind(informeSettings);

// Registrar servicios en DI
builder.Services.AddSingleton(ftpSettings);
builder.Services.AddSingleton(informeSettings);

builder.Services.AddScoped<IReportGenerator, ReportGenerator>();
builder.Services.AddScoped<IFtpService, FtpService>();

var app = builder.Build();

// Servir index.html desde wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

// Endpoint que procesa el envío
app.MapPost("/enviar", async (
    IReportGenerator reportGenerator,
    IFtpService ftpService) =>
{
    try
    {
        // 1. Generar el archivo CSV (fecha/hora actual)
        string rutaLocal = await reportGenerator.GenerarInformeAsync();
        string nombreArchivo = Path.GetFileName(rutaLocal);

        // 2. Enviar por FTP
        await ftpService.SubirArchivoAsync(rutaLocal, nombreArchivo);

        string mensaje = $"Se generó y envió por FTP el archivo {nombreArchivo} correctamente.";
        return Results.Content(mensaje, "text/plain; charset=utf-8");
    }
    catch (Exception ex)
    {
        return Results.Content("Error en el proceso: " + ex.Message, "text/plain; charset=utf-8");
    }
});

app.Run();