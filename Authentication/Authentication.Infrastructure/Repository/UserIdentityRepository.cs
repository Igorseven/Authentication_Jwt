using Authentication.Domain.Entities;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Authentication.Infrastructure.ORM.Context;
using Authentication.Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Authentication.Infrastructure.Repository;
public class UserIdentityRepository : BaseRepository<UserIdentity>, IUserIdentityRepository
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly SignInManager<UserIdentity> _signInManager;

    public UserIdentityRepository(ApplicationContext context,
                                  UserManager<UserIdentity> userManager,
                                  SignInManager<UserIdentity> signInManager)
        : base(context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<bool> HaveInTheDatabaseAsync(Expression<Func<UserIdentity, bool>> where) => await _dbSetContext.AnyAsync(where);

    public async Task<UserIdentity?> FindByPredicateWithSelectorAsync(Expression<Func<UserIdentity, bool>> predicate,
                                                                      Expression<Func<UserIdentity, UserIdentity>>? selector = null,
                                                                      bool asNoTracking = false)
    {
        IQueryable<UserIdentity> query = _dbSetContext;

        if (asNoTracking)
            query = query.AsNoTracking();

        if (selector is not null)
            query = query.Select(selector);

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<IList<string>> FindAllRolesAsync(UserIdentity accountIdentity) =>
        await _userManager.GetRolesAsync(accountIdentity);

    public async Task<IdentityResult> SaveAsync(UserIdentity entity) =>
        await _userManager.CreateAsync(entity, entity.PasswordHash!);

    public async Task<IdentityResult> UpdateAsync(UserIdentity accountIdentity)
    {
        if (accountIdentity.PasswordHash!.Length <= 20)
        {
            var hashed = _userManager.PasswordHasher.HashPassword(accountIdentity, accountIdentity.PasswordHash);
            accountIdentity.PasswordHash = hashed;
        }

        return await _userManager.UpdateAsync(accountIdentity);
    }

    public async Task<string> GenerateTokenToChangePasswordAsync(UserIdentity entity) =>
        await _userManager.GeneratePasswordResetTokenAsync(entity);

    public async Task<IdentityResult> ResetPasswordAsync(UserIdentity entity, string password)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(entity);
        return await _userManager.ResetPasswordAsync(entity, token, password);
    }

    public async Task<SignInResult> PasswordSignInAsync(string login, string password) =>
        await _signInManager.PasswordSignInAsync(login, password, false, false);
}

