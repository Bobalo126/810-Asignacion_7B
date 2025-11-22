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
            using var client = new FtpClient
            {
                Host = _settings.Server,
                Port = 21,
                Credentials = new NetworkCredential(_settings.User, _settings.Password),
            };
                        
            client.Config.DataConnectionType = FtpDataConnectionType.PASV;
            client.Config.EncryptionMode = FtpEncryptionMode.None;

            client.Connect();

            var status = client.UploadFile(
                localPath: rutaLocal,
                remotePath: nombreRemoto,
                existsMode: FtpRemoteExists.Overwrite
            );

            client.Disconnect();

            if (status != FtpStatus.Success)
            {
                throw new Exception("No se pudo subir el archivo al servidor FTP.");
            }

            return Task.CompletedTask;
        }
    }
}
