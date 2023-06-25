using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Go_Saku.Net.Models;

namespace go_saku_cs.Models
{
    public class PhotoUser
    {

        [Key]
        [Column("photo_id")]
        [JsonPropertyName("photo_id")]
        public int PhotoId { get; set; }

        [Column("url_photo")]
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [Column("user_id")]
        [JsonPropertyName("user_id")]
        public Guid UserID { get; set; }

        [ForeignKey("UserID")]
        [JsonPropertyName("user")]
        [JsonIgnore]
        public User User { get; set; }
        [NotMapped]  // Tandai properti 'File' sebagai non-mapped
        public IFormFile File { get; set; }
       

    }
}
