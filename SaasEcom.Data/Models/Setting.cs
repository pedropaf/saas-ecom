using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaasEcom.Data.Models
{
    public class Setting
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(100)]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
