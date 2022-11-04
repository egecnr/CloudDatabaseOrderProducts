using Newtonsoft.Json;
using ShowerShow.Model;
using System;
using System.Collections.Generic;


namespace ShowerShow.Models
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
