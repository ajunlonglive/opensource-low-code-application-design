namespace Presentation.ViewModels
{
    public class DCAppUserViewModel : BaseViewModel
    {
        public DCAppUserViewModel(string id) : base(id)
        {
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}