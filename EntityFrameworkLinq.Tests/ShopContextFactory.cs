using System.Collections.ObjectModel;
using System.Data.Common;
using System.Reflection;
using System.Resources;
using EntityFrameworkLinq.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkLinq.Tests
{
    public sealed class ShopContextFactory : IDisposable
    {
        private readonly SqliteConnection connection;

        public ShopContextFactory()
        {
            this.connection = CreateConnection();
            InitializeDatabase(this.connection);
        }

        public ShopContext CreateContext()
        {
            return new ShopContext(this.CreateOptions());
        }

        public DbContextOptions<ShopContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<ShopContext>()
                .UseSqlite(this.connection)
                .Options;
        }

        public IReadOnlyList<T> ReadEntities<T>(string commandText, Func<DbDataReader, T> builder)
        {
            var list = new List<T>();

            using (var command = new SqliteCommand(commandText, this.connection))
            {
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var entity = builder(reader);
                    list.Add(entity);
                }
            }

            return new ReadOnlyCollection<T>(list);
        }

        public void Dispose()
        {
            this.connection.Dispose();
        }

        private static TextReader GetResourceReader()
        {
            const string resourceManifestName = "EntityFrameworkLinq.Tests.Properties.Resources.resources";
            const string resourceName = "InitializationScript";

            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceManifestName);
            ResourceSet set = new ResourceSet(resourceStream!);
            string? @string = set.GetString(resourceName);

            return new StringReader(@string!);
        }

        [Obsolete("This method is only for testing purposes.")]
        private static TextReader GetStringReader()
        {
            string script = string.Empty;

            return new StringReader(script);
        }

        private static void InitializeDatabase(SqliteConnection connection)
        {
            TextReader stringReader = GetResourceReader();

            using (var transaction = connection.BeginTransaction())
            {
                string? line;
                while ((line = stringReader.ReadLine()) is not null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    using (var command = new SqliteCommand(line, connection, transaction))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
        }

        private static SqliteConnection CreateConnection()
        {
            var sqliteConnection = new SqliteConnection("DataSource=:memory:");
            sqliteConnection.Open();

            return sqliteConnection;
        }
    }
}
