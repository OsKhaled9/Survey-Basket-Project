namespace SurveyBasket.Api.Authentication.Filters;

public class PermissionsRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
