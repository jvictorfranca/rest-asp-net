using RestASPNet.Files.Importers.Contract;

namespace RestASPNet.Files.Importers.Factory
{
    public class FileImporterFactory
    {
        private  readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FileImporterFactory> _logger;
        public FileImporterFactory(IServiceProvider serviceProvider, ILogger<FileImporterFactory> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
    public IFileImporter GetFileImporter(string fileType)
        {
            if(fileType.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("Creating CSVFileImporter for file type: {FileType}", fileType);
                return _serviceProvider.GetRequiredService<CSVFileImporter>();
            }
            else if(fileType.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("Creating ExcelFileImporter for file type: {FileType}", fileType);
                return _serviceProvider.GetRequiredService<XlsxFileImporter>();
            }
            else
            {
                throw new NotSupportedException($"File type '{fileType}' is not supported.");
            }
        }

    }   

}
