namespace Route
{
    public class AuthenticatedRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return httpContext.User.Identity.IsAuthenticated;
        }
    }
}
