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
    
    public partial class Ticket
    {
        public int TicketID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> ShowtimeID { get; set; }
        public Nullable<int> SeatID { get; set; }
        public Nullable<System.DateTime> PurchaseDate { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<bool> PaymentConfirmation { get; set; }
        public string PaymentMethod { get; set; }
    
        public virtual Seat Seat { get; set; }
        public virtual Showtime Showtime { get; set; }
        public virtual User User { get; set; }
    }
}
