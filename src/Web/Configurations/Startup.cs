namespace Web.Configurations;

internal static class Startup
{
    internal static IConfigurationBuilder AddConfigurations(this IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .AddJsonFiles("appsettings")
            .AddJsonFiles("database")
            .Build();
        return configurationBuilder;
    }

    private static IConfigurationBuilder AddJsonFiles(this IConfigurationBuilder configurationBuilder, string fileName)
    {
        string rootPath = Environment.CurrentDirectory;
        return configurationBuilder.AddJsonFile(Path.Combine(rootPath, "Configurations", string.Join(".", fileName, "json")), true, true);
    }
}
