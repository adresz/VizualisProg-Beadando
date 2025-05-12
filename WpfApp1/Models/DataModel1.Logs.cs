using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Logs
    {
        public int log_id { get; set; }
        public int user_id { get; set; }

        [Column("datetime")]  // Adatbázis mező neve
        public DateTime date { get; set; }

        public string Action { get; set; }
        public int? involved_user { get; set; }

        public virtual Users Users { get; set; }
        public virtual Users InvolvedUser { get; set; }
    }
}
