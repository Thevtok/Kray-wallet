using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Go_Saku.Net.Models
{
    public class User
    {
        [Key]
        [Column("user_id")]
        [JsonPropertyName("user_id")]
        public Guid ID { get; set; }

        [Column("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [Column("username")]
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [Column("email")]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [Column("password")]
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [Column("phone_number")]
        [JsonPropertyName("phone_number")]
        public string? PhoneNumber { get; set; }

        [Column("address")]
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [Column("balance")]
        [JsonPropertyName("balance")]
        public int Balance { get; set; }

        [Column("role")]
        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [Column("point")]
        [JsonPropertyName("point")]
        public int Point { get; set; }

        [Column("token")]
        [JsonPropertyName("token")]
        public string? Token { get; set; }

        [Column("badge_id")]
        [JsonPropertyName("badge_id")]
        
        public int BadgeID { get; set; }

        [ForeignKey("BadgeID")]
        [JsonPropertyName("badge")]
        [JsonIgnore]
        public Badge? Badge { get; set; }  // Relasi dengan model Badge

        [NotMapped]
        [JsonPropertyName("badge_name")]
        public string? BadgeName { get; set; }// Properti untuk mengakses nama badge

        [Column("tx_count")]
        [JsonPropertyName("tx_count")]
        public int? TxCount { get; set; }

        public User()
        {
            ID = Guid.NewGuid();
            
        }
    }
}
