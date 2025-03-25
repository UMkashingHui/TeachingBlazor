using System;
namespace TeachingWebApi.Config
{
	public class TeachingAppDatabaseSettings
	{
		public string BooksCollectionName { get; init; }
		public string UsersCollectionName { get; init; }
		public string RolesCollectionName { get; init; }
		public string ConnectionString { get; init; }
		public string DatabaseName { get; init; }
	}
}

