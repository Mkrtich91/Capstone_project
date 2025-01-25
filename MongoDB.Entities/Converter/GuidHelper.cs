using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Entities.Converter
{
    public static class GuidHelper
    {
        public static Guid ToGuid(string value)
        {
            if (Guid.TryParse(value, out Guid result))
            {
                return result;
            }

            return Guid.Empty;
        }
    }
}
