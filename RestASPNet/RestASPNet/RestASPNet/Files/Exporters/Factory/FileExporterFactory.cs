using RestASPNet.Files.Exporters.Contract;
using RestASPNet.Files.Importers.Factory;

namespace RestASPNet.Files.Exporters.Factory
{
    public class FileExporterFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FileExporterFactory> _logger;
        public FileExporterFactory(IServiceProvider serviceProvider, ILogger<FileExporterFactory> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public IFileExporter GetExporter(string acceptHeader)
        {
            if (string.Equals(acceptHeader, MediaTypes.ApplicationXlsx))
            {
                return _serviceProvider.GetService<XlsxExporter>();
            }
            else if (string.Equals(acceptHeader, MediaTypes.ApplicationCsv))
            {
                return _serviceProvider.GetService<CsvExporter>();
            }
            else
            {
                _logger.LogWarning("Unsupported media type requested: {AcceptHeader}", acceptHeader);
                throw new NotSupportedException($"Unsupported media type: {acceptHeader}");
            }
        }
    }
}
