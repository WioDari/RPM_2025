namespace DemoExam.Models
{
    public class User
    {
        public int Id { get; set; }           
        public string Login { get; set; }         
        public string Password { get; set; }     
        public string FullName { get; set; }      
        public string Email { get; set; }    
        public int RoleId { get; set; }      
        public int SubscriptionId { get; set; }   
        public string Playlists { get; set; }      
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLogin { get; set; }
        public virtual UserRole Role { get; set; }
        public virtual SubscriptionType Subscription { get; set; }
        public virtual ICollection<Playlist> PlaylistsNav { get; set; } = new List<Playlist>();
    }
}