using dii.storage;
using dii.storage.cosmos;
using dii.storage.cosmos.Models;
using dii.storage.Models.Interfaces;
using Microsoft.Extensions;
using TodoApp.Adapters;
using TodoApp.Models;
using TodoApp.Models.Interfaces;

public class Startup
{
	private IConfiguration _configuration { get; }
	public Optimizer Optimizer;
	public INoSqlDatabaseConfig NoSqlDatabaseConfig;

	public Startup() {

		_configuration = new ConfigurationBuilder()
            .SetBasePath("C:\\Users\\SEO\\Desktop\\csharp\\todoApp")
            .AddJsonFile("appsettings.json")
            .Build();

		// Register the configuration from your appSettings.{env}.json file.
		NoSqlDatabaseConfig = new CosmosDatabaseConfig();

		if (Optimizer == null) {
			Optimizer = Optimizer.Init(typeof(Todo));
		}

	}

	public void ConfigureServices(IServiceCollection services)
	{

		_configuration.GetSection("Storage:Cosmos").Bind(NoSqlDatabaseConfig);

		// Initialize the DiiCosmosContext with the configuration.
		var context = DiiCosmosContext.Init(NoSqlDatabaseConfig);

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

		// Initialize the tables within the DiiCosmosContext. Depending on your configuration,
		// this may attempt to create any containers that do not exist at startup.
		context.InitTablesAsync(Optimizer.Tables).Wait();

		//// If run from an async method, this is preferable:
		//await context.InitTablesAsync(optimizer.Tables).ConfigureAwait(false);
	}
	
}