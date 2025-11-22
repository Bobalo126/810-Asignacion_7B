using _810_Asignacion_7B.Config;

namespace _810_Asignacion_7B.Services
{
    public interface IReportGenerator
    {
        Task<string> GenerarInformeAsync();
    }

    public class ReportGenerator : IReportGenerator
    {
        private readonly InformeSettings _settings;

        public ReportGenerator(InformeSettings settings)
        {
            _settings = settings;
        }

        public async Task<string> GenerarInformeAsync()
        {
            Directory.CreateDirectory(_settings.CarpetaLocal);

            DateTime ahora = DateTime.Now;

            string nombreArchivo = $"InformeDiario_{ahora:yyyy-MM-dd_HH-mm}.csv";
            string rutaCompleta = Path.Combine(_settings.CarpetaLocal, nombreArchivo);

            var datosEjemplo = new List<string>
            {
                "Fecha;Cuenta;Descripcion;Monto",
                $"{ahora:yyyy-MM-dd};100-001;Depósito nómina;150000.00",
                $"{ahora:yyyy-MM-dd};200-345;Pago proveedor;-25000.50",
                $"{ahora:yyyy-MM-dd};300-777;Comisión bancaria;-1500.00"
            };

            await File.WriteAllLinesAsync(rutaCompleta, datosEjemplo);

            return rutaCompleta;
        }
    }
}