using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Go_Saku.Net.Models
{
    public class Badge
    {
        [Key]
        [Column("badge_id")]
        [JsonPropertyName("badge_id")]
        public int? BadgeID { get; set; }

        [Column("badge_name")]
        [JsonPropertyName("badge_name")]
        public string? BadgeName { get; set; }

        [Column("threshold")]
        [JsonPropertyName("threshold")]
        public int? Threshold { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
        public Badge()
        {
            Users = new List<User>(); // Inisialisasi properti Users dengan nilai awal
        }
    }
}
