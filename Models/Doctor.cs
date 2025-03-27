using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<Appointment> Appointments { get; set;}

    }
}
