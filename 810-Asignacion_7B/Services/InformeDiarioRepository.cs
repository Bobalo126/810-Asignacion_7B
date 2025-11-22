using Microsoft.Data.SqlClient;

namespace _810_Asignacion_7B.Services
{
    public interface IInformeDiarioRepository
    {
        Task RegistrarEnvioAsync(string nombreArchivo);
    }
    public class InformeDiarioRepository : IInformeDiarioRepository
    {
        private readonly string _connectionString;

        public InformeDiarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SuperintendenciaDb")
                                ?? throw new InvalidOperationException("Connection string 'SuperintendenciaDb' no encontrada.");
        }

        public async Task RegistrarEnvioAsync(string nombreArchivo)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
            INSERT INTO InformeDiario (NombreArchivo)
            VALUES (@NombreArchivo);";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@NombreArchivo", nombreArchivo);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
