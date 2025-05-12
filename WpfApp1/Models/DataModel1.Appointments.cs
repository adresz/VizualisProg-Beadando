using Model;

public class Appointments
{
    public int appointment_id { get; set; }  // This is the primary key
    public DateTime date { get; set; }
    public int doctor_id { get; set; }
    public int user_id { get; set; }

    public virtual Users Doctor { get; set; }
    public virtual Users User { get; set; }
}
