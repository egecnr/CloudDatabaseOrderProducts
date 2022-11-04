using Newtonsoft.Json;
using UserAndOrdersFunction.Model;
using System;
using System.Collections.Generic;


namespace UserAndOrdersFunction.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonRequired]
        public string UserName { get; set; }
        [JsonRequired]
        public string Password { get; set; }
        [JsonRequired]
        public string EmailAddress { get; set; }
        [JsonRequired]
        public string FullName { get; set; }

        public DateTime DateOfAccountCreated { get; set; }
    }
}
