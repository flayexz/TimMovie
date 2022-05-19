using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimMovie.Core.Entities.Enums;
using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.Core.Entities;

public class UserStatus : BaseEntity
{
    public Guid UserForeignKey { get; set; }
    
    public User User { get; set; }
    
    public UserStatusEnum UserStatusEnum { get; set; }
    
    public DateTime DateLastChange { get; set; }
}