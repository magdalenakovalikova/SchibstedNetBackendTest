namespace SchibstedBackendTest.Models
{
    public class UserViewModel
    {
        public string username { get; set; }
        public bool IsADMIN { get; set; }
        public bool IsPAGE_1 { get; set; }
        public bool IsPAGE_2 { get; set; }
        public bool IsPAGE_3 { get; set; }
    }

    public class UserPasswordViewModel : UserViewModel
    {
        public string password { get; set; }
    }
}