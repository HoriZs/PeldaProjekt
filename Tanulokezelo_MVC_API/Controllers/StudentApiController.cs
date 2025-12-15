using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tanulokezelo_MVC_API.Models;

namespace Tanulokezelo_MVC_API.Controllers
{
    [Route("api/studentapi")]
    [ApiController]
    public class StudentApiController : ControllerBase
    {
        private static List<Student> tanulok = new List<Student> 
        {
            new Student{Id=1,OMazonosito=71610355,Nev="Teszt Elek",
            EletKor=20,Atlag=4.5}
        };
        private static int nextId = 2;
        //GET - egész lista
        [HttpGet]
        public IActionResult GetAll()
        {
            //Console.WriteLine(tanulok[0]);
            return Ok(TanuloListaConverter(tanulok));
            //return Ok(tanulok);
        }
        //GET - Id alapján visszadani egy tanulót
        [HttpGet("{om}")]
        public IActionResult GetById(int om)
        {
            var tanulo = tanulok.FirstOrDefault(x => x.OMazonosito == om);

            if (tanulo == null) return NotFound();

            return Ok(new StudentDTO
            {
               OMazonosito=tanulo.OMazonosito,
               Nev=tanulo.Nev,
               EletKor=tanulo.EletKor,
               Atlag=tanulo.Atlag
            });
        }
        //POST - tanuló hozzáad
        [HttpPost("create")]
        public IActionResult AddStudent([FromBody] StudentDTO s)
        {
            //Id beállítása a nextID statikus változóval
            //Taj szam generálása (OM azonosító / életkor)
            if (s == null) return BadRequest();
            Student tanulo = new Student
            {
                Id= nextId++,
                OMazonosito=s.OMazonosito,
                Nev=s.Nev,
                Atlag=s.Atlag,
                EletKor=s.EletKor,
                TAJszam=s.OMazonosito/s.EletKor
            };           
            tanulok.Add(tanulo);
            return Ok(tanulo);
        }
        //PUT - módosítás
        [HttpPut("update/{om}")]
        public IActionResult UpdateStudent(int om, [FromBody] StudentDTO modositott) 
        { 
            if(modositott==null) return BadRequest();

            var tanulo = tanulok.FirstOrDefault(t=>t.OMazonosito==om);

            if (tanulo==null) return NotFound();
            //DTO -> MODEL frissítés
            tanulo.OMazonosito = modositott.OMazonosito;
            tanulo.EletKor = modositott.EletKor;
            tanulo.Atlag=modositott.Atlag;
            tanulo.Nev=modositott.Nev;
            tanulo.TAJszam = modositott.OMazonosito / modositott.EletKor;

            return Ok(tanulo);
        }

        //DELETE - törlés
        [HttpDelete("delete/{om}")]
        public IActionResult DeleteStudent(int om)
        {
            var tanulo = tanulok.FirstOrDefault(t => t.OMazonosito == om);

            if (tanulo == null) return NotFound();

            tanulok.Remove(tanulo);

            //Törlés sikeres, de nincsen visszetérési adat
            return NoContent();
        }

        public List<StudentDTO> TanuloListaConverter(List<Student> lista) 
        {
            List<StudentDTO> tanuloDTOs= new List<StudentDTO>();
            foreach (var tanulo in lista)
            {
                StudentDTO tanuloDTO = new StudentDTO 
                {
                    OMazonosito = tanulo.OMazonosito,
                    Nev=tanulo.Nev,
                    EletKor=tanulo.EletKor,
                    Atlag=tanulo.Atlag
                };
                tanuloDTOs.Add(tanuloDTO);
            }
            return tanuloDTOs;
        }
    }
}
