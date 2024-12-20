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
    
    public partial class Showtime
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Showtime()
        {
            this.Seats = new HashSet<Seat>();
            this.Tickets = new HashSet<Ticket>();
        }
    
        public int ShowtimeID { get; set; }
        public Nullable<int> MovieID { get; set; }
        public Nullable<System.DateTime> ShowDate { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public string Room { get; set; }
    
        public virtual Movy Movy { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Seat> Seats { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
