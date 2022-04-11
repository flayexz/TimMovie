﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using TimMovie.Core.Entities;

namespace TimMovie.Web.ViewModels;

public class RegistrationViewModel
{
    
    [Required(ErrorMessage = "Это обязательно поле")]
    [EmailAddress(ErrorMessage = "Неверный тип почты")]
    [Display(Name = "Почта")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Это обязательно поле")]
    [Display(Name = "Логин")]
    public string UserName { get; set; }
    
    [Required(ErrorMessage = "Это обязательно поле")]
    [Display(Name = "Пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Это обязательно поле")]
    [Display(Name = "Подтверждение пароля")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password),ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }
    
    
    public Country? Country { get; set; }

}