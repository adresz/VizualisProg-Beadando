using Model;
using System.ComponentModel.DataAnnotations;

public class User_details
{
    [Key]  // Explicitly specify user_id as the primary key
    public int user_id { get; set; }
    public string email { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string phone_Number { get; set; }
    public string taj_Number { get; set; }
    public int gender { get; set; }

    // Navigation property to Users
    public virtual Users Users { get; set; }
}
