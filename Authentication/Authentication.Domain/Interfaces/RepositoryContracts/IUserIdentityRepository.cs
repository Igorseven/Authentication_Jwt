using Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Authentication.Domain.Interfaces.RepositoryContracts;
public interface IUserIdentityRepository : IDisposable
{
    Task<string> GenerateTokenToChangePasswordAsync(UserIdentity accountIdentity);
    Task<IdentityResult> SaveAsync(UserIdentity accountIdentity);
    Task<IdentityResult> UpdateAsync(UserIdentity accountIdentity);
    Task<SignInResult> PasswordSignInAsync(string login, string password);
    Task<IdentityResult> ChangePasswordAsync(UserIdentity entity, string currentPassword, string newPassword);
    Task<IList<string>> FindAllRolesAsync(UserIdentity accountIdentity);
    Task<bool> HaveInTheDatabaseAsync(Expression<Func<UserIdentity, bool>> where);
    Task<UserIdentity?> FindByPredicateWithSelectorAsync(Expression<Func<UserIdentity, bool>> predicate,
                                                         Expression<Func<UserIdentity, UserIdentity>>? selector = null,
                                                         bool asNoTracking = false);

}
