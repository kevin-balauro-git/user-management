using UserManagement.API.Entities;

namespace UserManagement.API.Dto
{
    public class AddressDto
    {
        public string City { get; set; } = string.Empty;
        public string StreetNumber { get; set; } = string.Empty;
        public string StreetName { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public GeoLocationDto GeoLocation { get; set; }
    }
}
