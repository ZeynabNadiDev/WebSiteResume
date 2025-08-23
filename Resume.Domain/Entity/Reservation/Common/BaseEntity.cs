using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume.Domain.Entity.Reservation.Common;

public abstract class BaseEntity<T>
{
    [Key]
    public T Id { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.Now;

    public bool IsDelete { get; set; } = false;
}