using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrySqlDepency
{
    [Table("TriggerMessageEvent")]
    public class TriggerMessageEvent
    {
        [Key]
        public long Id { get; set; }

        public string TableName { get; set; }

        public long TableRowKey { get; set; }

        public string Action { get; set; }

        public bool? IsError { get; set; }

        [Column("StateMessage")]
        [StringLength(255)]
        public string Message { get; set; }

        public string Appoint { get; set; }
    }
}
