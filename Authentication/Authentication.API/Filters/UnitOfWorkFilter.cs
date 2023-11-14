using Authentication.API.Extensions;
using Authentication.Domain.Interfaces.OthersContracts;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Authentication.API.Filters;

public sealed class UnitOfWorkFilter : ActionFilterAttribute
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationHandler _notification;

    public UnitOfWorkFilter(IUnitOfWork unitOfWork, INotificationHandler notificationHandler)
    {
        _unitOfWork = unitOfWork;
        _notification = notificationHandler;
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (ExternalMethodExtension.IsMethodGet(context)) return;
        
        var routeData = context.HttpContext.GetRouteData();
        var routeName = routeData.Values["action"]?.ToString();
        const string authRouteName = "CreateAccessToken";

        if (routeName is not null && routeName.Equals(authRouteName, StringComparison.InvariantCulture))
            LonginMethod(context);
        else
            OthersMethods(context);

        base.OnActionExecuted(context);
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (ExternalMethodExtension.IsMethodGet(context)) return;

        _unitOfWork.BeginTransaction();

        base.OnActionExecuting(context);
    }

    private void LonginMethod(ActionExecutedContext context)
    {
        if (context.Exception is null)
            _unitOfWork.CommitTransaction();
        else
            _unitOfWork.RollbackTransaction();
    }

    private void OthersMethods(ActionExecutedContext context)
    {
        if (context.Exception is null && context.ModelState.IsValid && !_notification.HasNotification())
            _unitOfWork.CommitTransaction();
        else
            _unitOfWork.RollbackTransaction();
    }
}

