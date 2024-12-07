using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicPro_MVVM_WPF.Model;

public class UserModel
{   
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("user_id")]
    public int UserId { get; set; }
    
    [Required]
    [Column("login")]
    public string Login { get; set; }
    
    [Required]
    [Column("role")]
    public string Role { get; set; }
    
    [Required]
    [Column("hash_password")]
    public string HashPassword { get; set; }  = string.Empty;
}