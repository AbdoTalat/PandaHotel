using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Entities
{
    public class MasterDataItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short Value { get; set; }
        public bool IsActive { get; set; }
        public int MasterDataTypeId { get; set; }
        public MasterDataType? MasterDataType { get; set; }
    }
}
