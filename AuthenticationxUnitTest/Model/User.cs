﻿

using AuthenticationAPIApp.Contracts;

namespace AuthenticationxUnitTest.Model
{
    public class User : IUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Scope { get; set; }

    }
}
