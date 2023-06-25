using Go_Saku.Net.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Bank
{
    [Key]
    [Column("account_id")]
    [JsonPropertyName("account_id")]
    public int AccountId { get; set; }

    [Column("bank_name")]
    [JsonPropertyName("bank_name")]
    public string BankName { get; set; }

    [Column("account_number")]
    [JsonPropertyName("account_number")]
    public string AccountNumber { get; set; }

    [Column("account_holder_name")]
    [JsonPropertyName("account_holder_name")]
    public string AccountHolderName { get; set; }

    [Column("user_id")]
    [JsonPropertyName("user_id")]
    public Guid UserID { get; set; }

    [ForeignKey("UserID")]
    [JsonPropertyName("user")]
    [JsonIgnore]
    public User User { get; set; }  // Relasi dengan model User

    // ...
}
