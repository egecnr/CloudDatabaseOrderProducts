using Newtonsoft.Json;
using System.Collections.Generic;
using UserAndOrdersFunction.Models;
using System;
using UserAndOrdersFunction.Utils;

namespace UserAndOrdersFunction.DTO
{
    public class CreateUserDTO
    {
        [JsonRequired]
        public string UserName { get; set; }
        [JsonRequired]
        public string Password { get; set; }
        [JsonRequired]
        public string EmailAddress { get; set; }
        [JsonRequired]
        public string FullName { get; set; }
    }
}
