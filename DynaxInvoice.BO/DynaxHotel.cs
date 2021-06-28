using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.BO
{
    public class DynaxHotel
    {
        public int? Id { get; set; }
        public int CustomerId { get; set; }
        public string HotelName { get; set; }
        public string HotelType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public int StateId { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Landmark { get; set; }
        public string DistFromLandmark { get; set; }
        public int RatePerNight { get; set; }
        public string CompanyName { get; set; }
        public string StateName { get; set; }
        public string FacilityId { get; set; }
    }
}
