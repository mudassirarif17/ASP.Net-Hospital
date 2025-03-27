using System.ComponentModel.DataAnnotations;

namespace project.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public string Name { get; set; }

        public ICollection<Doctor> Doctors { get;}
    }
}
