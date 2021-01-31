namespace Shared.Helpers
{
    public interface IConfigurationReader
    {
        public string GetConnectionString();
        public string GetDatabaseName();
    }
}
