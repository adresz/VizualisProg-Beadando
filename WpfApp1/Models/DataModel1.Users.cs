using Model;
using System.ComponentModel.DataAnnotations;

public class Users
{
    [Key]  // Explicitly define user_id as the primary key
    public int user_id { get; set; }

    public string username { get; set; }
    public string password { get; set; }
    public int access_id { get; set; }
    public DateTime created_at { get; set; }
    public bool is_banned { get; set; }
    public string ban_reason { get; set; }

    // Navigation properties
    public virtual User_details User_details { get; set; }  // One-to-one relation with User_details
    public virtual ICollection<Logs> Logs { get; set; }      // One-to-many relation with Logs
    public virtual ICollection<Appointments> Appointments { get; set; } // One-to-many relation with Appointments
}
