using Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Authentication.Domain.Interfaces.RepositoryContracts;

public interface IUserRepository : IDisposable
{
    Task<string> GenerateTokenToChangePasswordAsync(User account);
    Task<IdentityResult> SaveAsync(User account);
    Task<IdentityResult> UpdateAsync(User account);
    Task<SignInResult> PasswordSignInAsync(string login, string password);
    Task<IdentityResult> ChangePasswordAsync(User entity, string currentPassword, string newPassword);
    Task UserSignOutAsync();
    Task<IList<string>> FindAllRolesAsync(User account);
    Task<bool> HaveInTheDatabaseAsync(Expression<Func<User, bool>> where);

    Task<User?> FindByPredicateWithSelectorAsync(
        Expression<Func<User, bool>> predicate,
        Expression<Func<User, User>>? selector = null,
        bool asNoTracking = false);
}