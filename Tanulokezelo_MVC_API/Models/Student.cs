namespace Tanulokezelo_MVC_API.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Nev { get; set; }
        public int EletKor { get; set; }
        public double Atlag { get; set; }
        public int OMazonosito { get; set; }
        public int TAJszam {  get; set; }

    }
    public class StudentDTO
    {
        public int OMazonosito { get; set; }
        public string Nev { get; set; }
        public int EletKor { get; set; }
        public double Atlag { get; set; }
    }
}
