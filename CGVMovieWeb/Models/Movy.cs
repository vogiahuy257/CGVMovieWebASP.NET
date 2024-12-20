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
    
    public partial class Movy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Movy()
        {
            this.Showtimes = new HashSet<Showtime>();
        }
    
        public int MovieID { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public Nullable<int> Duration { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> ReleaseDate { get; set; }
        public Nullable<double> Rating { get; set; }
        public string ImageLink { get; set; }
        public Nullable<decimal> TicketPrice { get; set; }
        public string LinkTrailer { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Showtime> Showtimes { get; set; }
    }
}
