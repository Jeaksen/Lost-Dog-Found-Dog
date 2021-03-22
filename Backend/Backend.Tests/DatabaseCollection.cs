using Xunit;

namespace Backend.Tests.Authentication
{
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture> {}
}
