//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CGVMovieWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Seat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Seat()
        {
            this.Tickets = new HashSet<Ticket>();
        }
    
        public int SeatID { get; set; }
        public string Room { get; set; }
        public string SeatNumber { get; set; }
        public Nullable<bool> IsAvailable { get; set; }
        public Nullable<int> ShowtimeID { get; set; }
    
        public virtual Showtime Showtime { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}