namespace RAWANiAspireOrchestrationApp.ApiService.Presentation
{
    public static class ApiRoutes
    {
        public const string BaseRoute = "api/v{version:apiVersion}/[controller]";

        public static class UserProfileRouts
        {
            public const string IdRoute = "{userProfileId}";
        } 
    }

}
