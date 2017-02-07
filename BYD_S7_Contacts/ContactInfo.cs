namespace BYD_S7_Contacts
{
    public class ContactInfo
    {
        public ContactInfo()
        {
            this.Group = string.Empty;
        }

        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string PhoneType { get; set; }
        public string PhoneValue { get; set; }
        public string Group { get; set; }
    }
}
