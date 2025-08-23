using Resume.Domain.Entity.Reservation.Common;
using System;
using System.Collections.Generic;

namespace Resume.Domain.Entity.Reservation;

public class ReservationDate : BaseEntity<ulong>
{
    public DateTime Date { get; set; }

    public virtual List<ReservationDateTime> ReservationDateTimes { get; set; } = new();
}
