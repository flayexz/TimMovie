﻿using System.ComponentModel.DataAnnotations;

namespace TimMovie.Core.Entities;

public class Notification
{
    public Guid Id { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    public List<User> Users { get; set; }
    
    public DateTime? Date { get; set; }

}