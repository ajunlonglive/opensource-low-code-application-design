namespace Domain.Entities
{
    public class DCAppInternalUser : DCAppUser
    {
        public DCAppInternalUser(string username) : base(username)
        {
            IsInternal = true;

        }

        public DCAppInternalUser()
        {
            IsInternal = true;
        }
    }
}