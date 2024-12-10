using System.Net;

public class AuthenticationMiddleware {
    private RequestDelegate _next;
    public readonly string ClientId = Environment.GetEnvironmentVariable("clientId");
    public readonly string ClientSecret = Environment.GetEnvironmentVariable("clientSecret");
    public readonly string AuthEndpoint = Environment.GetEnvironmentVariable("authEndpoint");
    public readonly string TokenEndpoint = Environment.GetEnvironmentVariable("tokenEndpoint");
    public readonly string UserInformationEndpoint = Environment.GetEnvironmentVariable("userInfoEndpoint");
    public readonly string redirect_url = Environment.GetEnvironmentVariable("redirectUrl");

    public AuthenticationMiddleware(RequestDelegate next) {
        if (next == null)
            throw new ArgumentException("Next should not be null");
        _next = next;
    }

    public Task Invoke(HttpContext context) {
        if (!context.Request.Path.Value.StartsWith("/js") && !context.Request.Path.Value.StartsWith("/css")) {
            if (context.Request.Host.Host.Contains("localhost")) // for debugging locally
            {
                context.Items.Add("id", "adamd");
                context.Items.Add("fullname", "Adam Dukovich");
                context.Items.Add("siterole", "admin");
            }
            else if (!context.Request.QueryString.ToString().Contains("code") && !context.Request.Cookies.Any(p => p.Key == "id")) { // Redirect to auth endpoint
                context.Response.Redirect(AuthEndpoint + "?client_id=" + ClientId + "&client_secret=" + ClientSecret + "&redirect_uri=" + redirect_url);
            }
            else if (!context.Request.Cookies.Any(p => p.Key == "id")) { //
                try {
                    string code = context.Request.Query["code"].ToString();
                    string token = getToken(code);
                    UserInfo userinfo = getUserInfo(token);

                    //set user info
                    context.Items.Add("id", userinfo.UserID);
                    context.Items.Add("fullname", userinfo.FirstName + " " + userinfo.LastName);
                    context.Items.Add("siterole", userinfo.SiteRole);

                    //set cookie
                    context.Response.Cookies.Append("WorkRequest", getUniqueID(), new CookieOptions
                    {
                        Secure = true,
                        HttpOnly = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.Now.AddHours(2)
                    });
                }
                catch (Exception e) {
                    context.Response.Redirect("Error");
                }
            }
        }
        return _next.Invoke(context);
    }

    public string getToken(string code) {
        string token = "";
        return token;
    }

    public UserInfo getUserInfo(string token) {
        UserInfo userinfo = new UserInfo();
        userinfo.UserID = "jsomebody";
        userinfo.FirstName = "Joe";
        userinfo.LastName = "Somebody";
        userinfo.SiteRole = "user";
        return userinfo;
    }

    public string getUniqueID() {
        return "KLJK72KJD01";
    }

    public class UserInfo() {
        public string UserID;
        public string SiteRole;
        public string FirstName;
        public string LastName;
    }
}