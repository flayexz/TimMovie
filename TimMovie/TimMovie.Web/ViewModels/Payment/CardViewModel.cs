using System.ComponentModel.DataAnnotations;
namespace TimMovie.Web.ViewModels.Payment;

public class CardViewModel
{
    public CardViewModel()
    {
        
    }
    
    [Display(Name = "Номер карты")]
    [Required(ErrorMessage = "это обязательное поле")]
    [MinLength(13, ErrorMessage = "неверная длина карты")]
    [MaxLength(16, ErrorMessage = "неверная длина карты")]
    public string CardNumber { get; set; } = null!;

    [Display(Name = "CVV/CVC2/CVV2")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "это обязательное поле")]
    [MinLength(3,ErrorMessage = "неверная длина cvv")]
    [MaxLength(7,ErrorMessage = "неверная длина cvv")]
    public string CCID { get; set; } = null!;

    [Display(Name = "Месяц")]
    [Required(ErrorMessage = "это обязательное поле")]
    [Range(1,31,ErrorMessage = "недопустимое значение для месяца")]
    public int ExpirationMonth { get; set; }
    
    [Display(Name = "Год")]
    [Required(ErrorMessage = "это обязательное поле")]
    [Range(2022,int.MaxValue,ErrorMessage = "срок годности вашей карты истек")]
    public int ExpirationYear { get; set; }

    [Display(Name = "Имя держателя")]
    [Required(ErrorMessage = "это обязательное поле")]
    public string NameOnCard { get; set; } = null!;
}