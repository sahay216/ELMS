namespace Route
{
    public class RoleRouteConstraint : IRouteConstraint
    {
        private readonly string _role;

        public RoleRouteConstraint(string role)
        {
            _role = role;
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return httpContext.User.IsInRole(_role);
        }
    }
}