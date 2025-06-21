using Authentication.Domain.Entities;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Authentication.Infrastructure.ORM.Context;
using Authentication.Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Authentication.Infrastructure.Repository;
public class UserIdentityRepository : BaseRepository<User>, IUserIdentityRepository
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UserIdentityRepository(ApplicationContext context,
                                  UserManager<User> userManager,
                                  SignInManager<User> signInManager)
        : base(context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<bool> HaveInTheDatabaseAsync(Expression<Func<User, bool>> where) => await DbSetContext.AnyAsync(where);

    public async Task<User?> FindByPredicateWithSelectorAsync(Expression<Func<User, bool>> predicate,
                                                                      Expression<Func<User, User>>? selector = null,
                                                                      bool asNoTracking = false)
    {
        IQueryable<User> query = DbSetContext;

        if (asNoTracking)
            query = query.AsNoTracking();

        if (selector is not null)
            query = query.Select(selector);

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<IList<string>> FindAllRolesAsync(User account) =>
        await _userManager.GetRolesAsync(account);

    public async Task<IdentityResult> SaveAsync(User entity) =>
        await _userManager.CreateAsync(entity, entity.PasswordHash!);

    public async Task<IdentityResult> UpdateAsync(User account) =>
        await _userManager.UpdateAsync(account);

    public async Task<string> GenerateTokenToChangePasswordAsync(User entity) =>
        await _userManager.GeneratePasswordResetTokenAsync(entity);

    public async Task<IdentityResult> ChangePasswordAsync(User entity, string currentPassword, string newPassword)
    {
        DetachedObject(entity);

        return await _userManager.ChangePasswordAsync(entity, currentPassword, newPassword);
    }

    public async Task<SignInResult> PasswordSignInAsync(string login, string password) =>
        await _signInManager.PasswordSignInAsync(login, password, false, true);

    public async Task UserSignOutAsync() => await _signInManager.SignOutAsync();
}

