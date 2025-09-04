using Resume.Domain.Entity.InterfaceEntity;
using Resume.Domain.Entity.Reservation.Common;

namespace Resume.Domain.Entity.Reservation;

public class PersonSelectedReservation : BaseEntity<ulong>,IEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }

    public ulong ReservationDateTimeId { get; set; }
    public virtual ReservationDateTime ReservationDateTime { get; set; }
}
