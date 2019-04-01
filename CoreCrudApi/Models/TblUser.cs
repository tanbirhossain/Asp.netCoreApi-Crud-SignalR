using System;
using System.Collections.Generic;

namespace CoreCrudApi.Models
{
    public partial class TblUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
