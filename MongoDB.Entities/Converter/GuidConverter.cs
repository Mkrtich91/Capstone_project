using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Entities.Converter
{
    public static class GuidConverter
    {
        public static Guid ConvertToGuid(int orderId)
        {
            byte[] bytes = new byte[16];
            byte[] orderIdBytes = BitConverter.GetBytes(orderId);
            Array.Copy(orderIdBytes, bytes, orderIdBytes.Length);
            return new Guid(bytes);
        }
    }

}
