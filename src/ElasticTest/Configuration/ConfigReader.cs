using System.Text.Json.Nodes;

namespace ElasticTest.Configuration;

//public class MyProgram
//{
//    public async Task Main()
//    {
//        var reader = new ConfigReader("");
//        await reader.OpenFileAsync();

//        var exists = reader.TryGetValue("connectionString", out var connectionString);

//        if (!exists)
//        {
//            // do something
//        }

//        var passwordExists = reader.TryGetValue("Password", out var password);
//        if (!passwordExists)
//        {
//            // do something
//        }

//        // process other code

//    }
//}

public class ConfigReader : IDisposable
{
    private bool disposedValue;
    private readonly string _fileFullPath;
    private JsonNode _jsonNode;

    public ConfigReader(string fileFullPath)
    {
        _fileFullPath = fileFullPath;
    }

    public async Task OpenFileAsync()
    {

        var fullPath = Path.Combine(AppContext.BaseDirectory, _fileFullPath);

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException(fullPath);
        }
        var configString = await File.ReadAllTextAsync(fullPath);
        _jsonNode = JsonNode.Parse(configString);
    }

    public bool TryGetValue(string key, out string value)
    { 
        try
        {
            value = _jsonNode![key]!.ToString();
        }
        catch
        {
            value = null!;
            return false;
        }

        return true;
    }


    #region IDisposble implementation
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~ConfigReader()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}