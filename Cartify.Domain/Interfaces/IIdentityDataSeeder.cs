namespace Cartify.Domain.Interfaces;

public interface IIdentityDataSeeder
{
    public Task SeedRolesAsync();
    public Task SeedUsersAsync();
}