namespace UserManagement.API.Entities
{
    public class Address
    {
        public string City { get; set; } = string.Empty;
        public string StreetNumber { get; set; } = string.Empty;
        public string StreetName { get; set; } = string.Empty;
        public string ZipCode {  get; set; } = string.Empty;
        public GeoLocation GeoLocation { get; set; } = new GeoLocation();
    }
}
