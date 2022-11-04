using Newtonsoft.Json;
using System.Collections.Generic;
using ShowerShow.Models;
using System;
using ShowerShow.Utils;

namespace ShowerShow.DTO
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
