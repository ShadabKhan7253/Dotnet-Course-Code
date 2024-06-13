namespace API_Start.Dtos
{
    public partial class UserForLoginConfirmation
    {
        public byte[] PasswordHash { get; set; }   
        public byte[] PasswordSalt { get; set; }  

        public UserForLoginConfirmation() 
        {
            if (PasswordHash == null)
            {
                PasswordHash = new byte[0];
            }
            if (PasswordSalt == null)
            {
                PasswordSalt = new byte[0];
            }
        } 
    }
}