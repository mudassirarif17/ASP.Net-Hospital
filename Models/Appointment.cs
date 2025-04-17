using project.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public string patientName { get; set; }
        public string status { get; set; }
        [ForeignKey("User") ] 
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public DateTime AppointmentDate { get; set; }

    }
}
