using System;
using System.ComponentModel.DataAnnotations;

namespace aspNetAuth.Models
{
    public class User
    {
        [Key]public int Id { get; set; }
        [Required]public string UserName { get; set; }
        [Required][DataType(DataType.Password)]public string Password { get; set; }
    }
}
