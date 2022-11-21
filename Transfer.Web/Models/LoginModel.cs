using System.ComponentModel.DataAnnotations;

namespace Transfer.Web.Models;

public sealed class LoginModel
{
    [Required]
    [Display(Name = "Username")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberLogin { get; set; } = true;

    public string ReturnUrl { get; set; }
}