namespace ElectroDocument.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate next;
        public JwtMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? token = context.Session.GetString("accessToken");

 

            if (context.Request.Path == "/logout")
            {
                // Clear the token from session or wherever it's stored
                context.Session.Remove("accessToken");
                // Optionally invalidate the token on the server-side
                // For example, if it's stored in a database
                // InvalidateToken(token); // Example function to invalidate the token
                // Redirect or send response as necessary
                context.Response.Redirect("/"); // Redirect to home after logout
                return;
            }

            if (token is not null)
            {

                context.Request.Headers.Authorization = "Bearer " + token;
            }

            await next.Invoke(context);
        }


    }
}
