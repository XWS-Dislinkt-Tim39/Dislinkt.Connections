using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dislinkt.Connections.Persistence.Neo4j.Entities;
using Dislinkt.Connections.Persistence.Neo4j.Factory;
using Neo4j.Driver;

namespace Dislinkt.Connections.Persistence.Neo4j.Common
{
    public class Neo4jDbContext
    {
        private readonly IDatabaseFactory _databaseFactory;
        public Neo4jDbContext(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public async Task<T> FindByIdAsync<T>(Guid id) where T : BaseEntity
        {
            var query = @"
                MATCH (u:User)
                WHERE u.Id = $id
                RETURN u";

            IAsyncSession session = _databaseFactory.Create().AsyncSession();
            try
            {
                var readResult = await session.ReadTransactionAsync(async tx =>
                    {
                        IResultCursor result = await tx.RunAsync(query, new { id = id });
                        return result.SingleAsync().As<T>();
                    }
                );

                return readResult;
            }
            catch (Neo4jException ex)
            {
                Trace.WriteLine($"{query} - {ex}");
                throw;
            }
        }

        public async Task CreateAsync<T>(T t, string EntityType) where T : BaseEntity
        {
            var type = typeof(T);
            var types = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            var query = $"CREATE(:{EntityType} {{Id: \"{t.Id}\", ";

            foreach (var field in types)
            {
                query += field.Name.Substring(1, Math.Max(field.Name.IndexOf('>') - 1, 0)) + @": """;
                query += field.GetValue(t);
                query += @""", ";   
            }

            query = query.Remove(query.Length - 2, 1);
            query += "})";


            IAsyncSession session = _databaseFactory.Create().AsyncSession();
            try
            {
                await session.RunAsync(query);
            }
            catch (Neo4jException ex)
            {
                Trace.WriteLine($"{query} - {ex}");
                throw;
            }
        }
         

        public async Task DeleteByIdAsync<T>(Guid id) where T : BaseEntity
        {
            var query = $"MATCH(n {{Id: \"{id}\"}})" +
                        $"DETACH DELETE n";
            IAsyncSession session = _databaseFactory.Create().AsyncSession();
            try
            {
                await session.RunAsync(query);
            }
            catch (Neo4jException ex)
            {
                Trace.WriteLine($"{query} - {ex}");
                throw;
            }
        }

        public async Task UpdateAsync<T>(Guid id, T t) where T : BaseEntity
        {

        }

        public async Task CreateConnectionAsync(Guid sourceId, Guid targetId, string connectionName)
        {
            var query = $"MATCH (t1), (t2) " +
                        $"WHERE t1.Id = \"{sourceId}\" AND t2.Id = \"{targetId}\" " +
                        $"CREATE (t1)-[:{connectionName}]->(t2)";
            IAsyncSession session = _databaseFactory.Create().AsyncSession();
            try
            {
                await session.RunAsync(query);
            }
            catch (Neo4jException ex)
            {
                Trace.WriteLine($"{query} - {ex}");
                throw;
            }
        }

        public async Task RemoveConnectionAsync(Guid sourceId, Guid targetId, string connectionName)
        {
            var query = $"MATCH (t1) - [connection:{connectionName}] -> (t2) " +
                        $"WHERE t1.Id = \"{sourceId}\" AND t2.Id = \"{targetId}\" " +
                        "DELETE connection";
            IAsyncSession session = _databaseFactory.Create().AsyncSession();
            try
            {
                await session.RunAsync(query);
            }
            catch (Neo4jException ex)
            {
                Trace.WriteLine($"{query} - {ex}");
                throw;
            }
        }

        public async Task<IReadOnlyList<Guid>> GetFollowing(Guid sourceId)
        {
            var query = "MATCH (n)-[:FOLLOWS]->(m) " +
                        $"WHERE n.Id = \"{sourceId}\" " +
                        "RETURN m.Id as Id";
            IAsyncSession session = _databaseFactory.Create().AsyncSession();
            try
            {
                List<IRecord> readResult = await session.ReadTransactionAsync(async tx =>
                    {
                        IResultCursor result = await tx.RunAsync(query);
                        return await result.ToListAsync();
                    }
                );

                return readResult.Count == 0 ? null : readResult.Select(record => Guid.Parse(record["Id"].ToString() ?? string.Empty)).ToList();
            }
            catch (Neo4jException ex)
            {
                Trace.WriteLine($"{query} - {ex}");
                throw;
            }
        }

    }
}
