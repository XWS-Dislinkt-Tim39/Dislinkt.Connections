using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Dislinkt.Connections.Persistence.Neo4j.Factory;
using Neo4j.Driver;

namespace Dislinkt.Connections.Persistence.Neo4j
{
    public class Neo4jExe
    {
        
        private readonly IDriver _driver;

        public Neo4jExe(string uri, string user, string password)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }


        public async Task CreateFriendship(string person1Name, string person2Name)
        {
            // To learn more about the Cypher syntax, see https://neo4j.com/docs/cypher-manual/current/
            // The Reference Card is also a good resource for keywords https://neo4j.com/docs/cypher-refcard/current/
            var query = @"
                        MERGE (p1:Person { name: $person1Name })
                        MERGE (p2:Person { name: $person2Name })
                        MERGE (p1)-[:KNOWS]->(p2)
                        RETURN p1, p2";

            var session = _driver.AsyncSession();
            try
            {
                // Write transactions allow the driver to handle retries and transient error
                var writeResults = await session.WriteTransactionAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, new { person1Name, person2Name });
                    return (await result.ToListAsync());
                });

                foreach (var result in writeResults)
                {
                    var person1 = result["p1"].As<INode>().Properties["name"];
                    var person2 = result["p2"].As<INode>().Properties["name"];
                    Trace.WriteLine($"Created friendship between: {person1}, {person2}");
                }
            }
            // Capture any errors along with the query and data for traceability
            catch (Neo4jException ex)
            {
                Trace.WriteLine($"{query} - {ex}");
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task FindPerson(string personName)
        {
            var query = @"
        MATCH (p:Person)
        WHERE p.name = $name
        RETURN p.name";

            var session = _driver.AsyncSession();
            try
            {
                var readResults = await session.ReadTransactionAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, new { name = personName });
                    return (await result.ToListAsync());
                });

                foreach (var result in readResults)
                {
                    Trace.WriteLine($"Found person: {result["p.name"].As<string>()}");
                }
            }
            // Capture any errors along with the query and data for traceability
            catch (Neo4jException ex)
            {
                Trace.WriteLine($"{query} - {ex}");
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }


        public static async Task<string> Example()
        {
            // Aura queries use an encrypted connection using the "neo4j+s" protocol
            var uri = "neo4j+s://803184a8.databases.neo4j.io";

            var user = "neo4j";
            var password = "SiwDvvkzyx3TXwBnLCJd7-cilalOTAzMWOszLjoccxg";

            var example = new Neo4jExe(uri, user, password);
            
            await example.CreateFriendship("Alice", "David");
            await example.FindPerson("Alice");

            return "uspeo";
        }
    }
}
