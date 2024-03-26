namespace System.Web
{
    // Ref: https://stackoverflow.com/questions/31243068/access-the-current-httpcontext-in-asp-net-core
    public static class HttpContext
    {
        private static Microsoft.AspNetCore.Http.IHttpContextAccessor m_httpContextAccessor;


        public static void Configure(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            m_httpContextAccessor = httpContextAccessor;
        }

        public static Microsoft.AspNetCore.Http.HttpContext Current
        {
            get
            {
                return m_httpContextAccessor.HttpContext;
            }
        }
    }
}
