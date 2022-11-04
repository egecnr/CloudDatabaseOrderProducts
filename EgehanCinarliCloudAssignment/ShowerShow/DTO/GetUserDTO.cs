using Newtonsoft.Json;
using ShowerShow.Model;
using ShowerShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.DTO
{
    public class GetUserDTO
    {
        [JsonRequired]
        public string UserName { get; set; }
        [JsonRequired]
        public string EmailAddress { get; set; }
        [JsonRequired]
        public string FullName { get; set; }

        public DateTime DateOfAccountCreated { get; set; }

    }
}
