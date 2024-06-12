namespace API_Start.Dtos
{
    partial class UserForLoginConfirmation
    {
        byte[] PasswordHash { get; set; }   
        byte[] PasswordSalt { get; set; }  

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