﻿using System.ComponentModel.DataAnnotations;

namespace TimMovie.Core.Entities;

public class Country
{
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public List<User> Users { get; set; }
    public List<Film> Films { get; set; }
}