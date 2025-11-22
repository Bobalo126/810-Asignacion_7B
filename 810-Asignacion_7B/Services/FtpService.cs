using _810_Asignacion_7B.Config;
using FluentFTP;
using System.Net;

namespace _810_Asignacion_7B.Services
{
    public interface IFtpService
    {
        Task SubirArchivoAsync(string rutaLocal, string nombreRemoto);
    }
    public class FtpService : IFtpService
    {
        private readonly FtpSettings _settings;

        public FtpService(FtpSettings settings)
        {
            _settings = settings;
        }

        public Task SubirArchivoAsync(string rutaLocal, string nombreRemoto)
        {
            // Crear cliente FTP
            using var client = new FtpClient
            {
                Host = _settings.Server, // "localhost"
                Port = 21,
                Credentials = new NetworkCredential(_settings.User, _settings.Password),
            };

            // Modo pasivo (lo que FileZilla espera por defecto)
            client.Config.DataConnectionType = FtpDataConnectionType.PASV;
            client.Config.EncryptionMode = FtpEncryptionMode.None;

            // 1) Conectar (SIN await, es síncrono)
            client.Connect();

            // 2) Subir archivo (también síncrono)
            var status = client.UploadFile(
                localPath: rutaLocal,
                remotePath: nombreRemoto,            // ej: InformeDiario_20251121.csv
                existsMode: FtpRemoteExists.Overwrite
            );

            // 3) Desconectar
            client.Disconnect();

            // 4) Validar resultado
            if (status != FtpStatus.Success)
            {
                throw new Exception("No se pudo subir el archivo al servidor FTP.");
            }

            // El método debe devolver un Task porque la interfaz lo pide
            return Task.CompletedTask;
        }
    }
}
