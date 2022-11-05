using Newtonsoft.Json;
using UserAndOrdersFunction.Model;
using UserAndOrdersFunction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAndOrdersFunction.DTO
{
    public class GetUserDTO
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [JsonRequired]
        public string UserName { get; set; }
        [JsonRequired]
        public string EmailAddress { get; set; }
        [JsonRequired]
        public string FullName { get; set; }

        public DateTime DateOfAccountCreated { get; set; }

    }
}
