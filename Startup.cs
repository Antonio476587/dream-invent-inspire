using dii.storage;
using dii.storage.cosmos;
using dii.storage.cosmos.Models;
using dii.storage.Models.Interfaces;
using Microsoft.Extensions;

public class Startup
{
	private IConfiguration _configuration { get; }

	public Startup() {
		_configuration = new ConfigurationBuilder()
            .SetBasePath("C:\\Users\\SEO\\Desktop\\csharp\\todoApp")
            .AddJsonFile("appsettings.json")
            .Build();
	}

	public void ConfigureServices(IServiceCollection services)
	{
		// Register the configuration from your appSettings.{env}.json file.
		INoSqlDatabaseConfig dbConfig = new CosmosDatabaseConfig();
		_configuration.GetSection("Storage:Cosmos").Bind(dbConfig);

		// Initialize the DiiCosmosContext with the configuration.
		var context = DiiCosmosContext.Init(dbConfig);

		// Ensure the database exists. Make sure to wait for the result.
		var dbExistsTask = context.DoesDatabaseExistAsync();
		dbExistsTask.Wait();

		//// If run from an async method, this is preferable:
		//var dbExistsTask = await context.DoesDatabaseExistAsync().ConfigureAwait(false);

		// Usually an exception is desired if the database does not exist and cannot be created.
		if (!dbExistsTask.Result)
		{
			throw new ApplicationException("CosmosDB database does not exist and failed to be created.");
		}

		// Initialize the Optimizer. With this overload, true means the Optimizer will attempt
		// to find all entities that implement the IDiiEntity interface and initalize them. An
		// exception will be thrown if any of the entities fail to initialize. This exception 
		// can be disabled with changing the second boolean input to true, however this is not
		// recommended.
		var optimizer = Optimizer.Init(true, false);

		// Initialize the tables within the DiiCosmosContext. Depending on your configuration,
		// this may attempt to create any containers that do not exist at startup.
		context.InitTablesAsync(optimizer.Tables).Wait();

		//// If run from an async method, this is preferable:
		//await context.InitTablesAsync(optimizer.Tables).ConfigureAwait(false);
	}
	
}