using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using TeachingWebApi.Utils.Data;
using TeachingWebApi.Models.Identity;
using TeachingWebApi.Utils.Constants;

namespace TeachingWebApi.Data.Seeder
{
    public class DatabaseSeeder
    {
        private readonly MongoDbContext _dbContext;
        private readonly UserManager<TeachingBlazorUser> _userManager;
        private readonly RoleManager<TeachingBlazorRole> _roleManager;
        public DatabaseSeeder(
            MongoDbContext dbContext,
            UserManager<TeachingBlazorUser> userManager,
            RoleManager<TeachingBlazorRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            AddAdministrator();
            AddBasicUser();
            InitializeCollection();
        }

        public async void InitializeCollection()
        {
            await CreateCollection("books");
        }

        private void AddAdministrator()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var adminRole = new TeachingBlazorRole(RoleConstants.AdministratorRole, "Administrator role with full permissions");
                var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                }
                // Check if User Exists
                var superUser = new TeachingBlazorUser
                {
                    FirstName = "SuperUser",
                    LastName = "SuperUser",
                    Email = "SuperUser@163.com",
                    UserName = "SuperUser",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedOn = DateTime.Now,
                    IsActive = true
                };
                var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
                if (superUserInDb == null)
                {
                    await _userManager.CreateAsync(superUser, UserConstants.DefaultPassword);
                    var result = await _userManager.AddToRoleAsync(superUser, RoleConstants.AdministratorRole);
                    // UpdateClaimsAsync(superUser.Id.ToString());
                }
            }).GetAwaiter().GetResult();
        }

        private void AddBasicUser()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var basicRole = new TeachingBlazorRole(RoleConstants.BasicRole, "Basic role with default permissions");
                var basicRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
                if (basicRoleInDb == null)
                {
                    await _roleManager.CreateAsync(basicRole);
                }
                //Check if User Exists
                var basicUser = new TeachingBlazorUser
                {
                    FirstName = "BasicUser",
                    LastName = "BasicUser",
                    Email = "basicuser@163.com",
                    UserName = "BasicUser",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedOn = DateTime.Now,
                    IsActive = true
                };
                var basicUserInDb = await _userManager.FindByEmailAsync(basicUser.Email);
                if (basicUserInDb == null)
                {
                    await _userManager.CreateAsync(basicUser, UserConstants.DefaultPassword);
                    await _userManager.AddToRoleAsync(basicUser, RoleConstants.BasicRole);
                }
            }).GetAwaiter().GetResult();
        }

        private async Task CreateCollection(string collectionName)
        {
            if (!CollectionExists(_dbContext._db, collectionName))
            {
                await _dbContext._db.CreateCollectionAsync(collectionName, new CreateCollectionOptions
                {
                    Capped = false
                });
                Console.WriteLine("Collection created!");
            }
        }

        private bool CollectionExists(IMongoDatabase database, string collectionName)
        {
            return database.ListCollectionNames().ToList().Contains(collectionName);
        }
    }
}