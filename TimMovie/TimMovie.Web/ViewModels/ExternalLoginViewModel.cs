using System.ComponentModel.DataAnnotations;

namespace TimMovie.Web.ViewModels;

public class ExternalLoginViewModel
{
    [Required]
    public string UserName { get; set; }
        
    [Required]
    public string ReturnUrl { get; set; }
}