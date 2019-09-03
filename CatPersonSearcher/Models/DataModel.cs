using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatPersonSearcher
{
    // Person Model
    [Table("PersonData")]
    public partial class PersonData
    {
        public long ID { get; set; }

        [StringLength(255)]
        public string Firstname { get; set; } = "";

        [StringLength(255)]
        public string Lastname { get; set; } = "";

        public virtual AddressData AddressData { get; set; }
        public virtual DetailsData DetailsData { get; set; }
    }

    // Address Model
    [Table("AddressData")]
    public partial class AddressData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; } = 0;

        [StringLength(100)]
        public string Phone { get; set; } = "";

        [StringLength(255)]
        public string Street { get; set; } = "";

        [StringLength(255)]
        public string City { get; set; } = "";

        [StringLength(50)]
        public string State { get; set; } = "";

        [StringLength(10)]
        public string Zipcode { get; set; } = "";

        public virtual PersonData PersonData { get; set; }
    }

    // Details Model
    [Table("DetailsData")]
    public partial class DetailsData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; } = 0;

        [StringLength(10)]
        public string Age { get; set; } = "";

        [StringLength(255)]
        public string Interests { get; set; } = "";

        [StringLength(255)]
        public string PhotoURL { get; set; } = "";

        public virtual PersonData PersonData { get; set; }
    }

    [Table("TableCount")]
    public partial class TableCount
    {
        public long ID { get; set; } = 0;
        public long? PersonCount { get; set; } = 0;
    }
}
