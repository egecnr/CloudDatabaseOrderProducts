using AutoMapper;
using UserAndOrdersFunction.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAndOrdersFunction.Utils
{
    public class AutoMapperUtil
    {
        public static Mapper ReturnMapper(MapperConfiguration config)
        {
            return new Mapper(config);
        }
    }
}
