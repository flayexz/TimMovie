﻿using System.ComponentModel.DataAnnotations;
using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.Core.Entities;

public class Subscribe : BaseEntity
{
    [Required]
    [MaxLength(70)]
    public string Name { get; set; }
    
    [Required]
    public decimal Price { get; set; }

    public string? Description { get; set; }
    
    public List<Film> Films { get; set; }
}