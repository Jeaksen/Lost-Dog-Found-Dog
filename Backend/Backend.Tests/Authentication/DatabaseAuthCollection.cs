using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Authentication
{
    [CollectionDefinition("Database Auth collection")]
    public class DatabaseAuthCollection : ICollectionFixture<DatabaseAuthFixture>
    {}
}
