using System;

namespace restapi.Types
{
    public class AuthContract
    {
        public string AccessToken { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime StartTime { get; set; }
    }

}