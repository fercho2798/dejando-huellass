using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Instituto 
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hola que tal mundo");

            Console.WriteLine("preparado");
        }

        
        static void SeedDatabase()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Database.Migrate();

                if (context.Colegioes.Any())
                {
                   
                    return;
                }

                var colegio1 = new Colegio();
                colegio1.Nombre = "Colegio 1";

                var alumno1 = new Alumno();
                alumno1.Nombre = "Paul";
                alumno1.Edad = 999;
                alumno1.Detalles = new AlumnoDetalle() { Becado = true, CategoriaDePago = 1 };

                var alumno2 = new Alumno();
                alumno2.Nombre = "valeria";
                alumno2.Edad = 15;
                alumno2.Detalles = new AlumnoDetalle() { Becado = false, Carrera = "Ingeniería de Software", CategoriaDePago = 1 };


                var alumno3 = new Alumno();
                alumno3.Nombre = "Samanta";
                alumno3.Edad = 25;
                alumno3.Detalles = new AlumnoDetalle() { Becado = true, Carrera = "Licenciatura en Derecho", CategoriaDePago = 2 };


                var direccion1 = new Direccion();
                direccion1.Calle = "Calle 20";
                alumno1.Direccion = direccion1;

                var grado1 = new Grado();
                grado1.Nombre = "Base de datos";

                var grado2 = new Grado();
                grado2.Nombre = "Administracion de empresas";

                var colegio2 = new Colegio();
                colegio2.Nombre = "Colegio 2";

                colegio1.Alumnos.Add(alumno1);
                colegio1.Alumnos.Add(alumno2);

                colegio2.Alumnos.Add(alumno3);

                context.Add(colegio1);
                context.Add(colegio2);
                context.Add(grado1);
                context.Add(grado2);

                context.SaveChanges();

                var AlumnoGrado = new AlumnoGrado();
                AlumnoGrado.Activo = true;
                AlumnoGrado.GradoId = grado1.Id;
                AlumnoGrado.AlumnoId = alumno1.Id;

                var AlumnoGrado2 = new AlumnoGrado();
                AlumnoGrado2.Activo = false;
                AlumnoGrado2.GradoId = grado1.Id;
                AlumnoGrado2.AlumnoId = alumno2.Id;

                context.Add(AlumnoGrado);
                context.Add(AlumnoGrado2);
                context.SaveChanges();
            }
        }

        static void EjemploInsertarAlumno()
        {
            using (var context = new ApplicationDbContext())
            {
                var alumno = new Alumno();
                alumno.Nombre = "Mateo";
                context.Add(alumno);
                context.SaveChanges();
            }
        }

        static void EjemploActualizarAlumnoModeloConectado()
        {
            using (var context = new ApplicationDbContext())
            {
                var alumnos = context.Alumnos.Where(x => x.Nombre == "Anita").ToList();

                alumnos[0].Nombre += " Apellido";

                context.SaveChanges();

            }
        }

        static void EjemploActualizarAlumnoModeloDesconectado(Alumno alumno)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(alumno).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
        }

        static void EjemploRemoverAlumnoModeloConectado()
        {
            using (var context = new ApplicationDbContext())
            {
                var alumno = context.Alumnos.FirstOrDefault();
                context.Remove(context);
                context.SaveChanges();
            }
        }

        static void EjemploRemoverAlumnoModeloDesonectado(Alumno alumno)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(alumno).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                context.SaveChanges();
            }
        }

        static void AgregarModeloUnoAUnoConectado()
        {
            using (var context = new ApplicationDbContext())
            {
             
                var alumno = new Alumno();
                alumno.Nombre = "Maria";
                alumno.Edad = 99;

                var direccion = new Direccion();
                direccion.Calle = "Calle 4";
                alumno.Direccion = direccion;

                context.Add(alumno);
                context.SaveChanges();
            }
        }

        static void AgregarModeloUnoAUnoModeloDesconectado(Direccion direccion)
        {
  
            using (var context = new ApplicationDbContext())
            {
                context.Entry(direccion).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }

        }

        static void TraerDataRelacionada()
        {
        
            using (var context = new ApplicationDbContext())
            {
                var alumnos = context.Alumnos.Include(x => x.Direccion).ToList();
            }
        }

        static void AgregarModeloMuchosAMuchosModeloDesconectado(Alumno alumno)
        {
           
            using (var context = new ApplicationDbContext())
            {
                context.Add(alumno);
                context.SaveChanges();
            }
        }

        static void TraerDataRelacionadaUnoAMuchos()
        {
            using (var context = new ApplicationDbContext())
            {

                var colegioesAlumnos1 = context.Colegioes.Where(x => x.Id == 1).Include(x => x.Alumnos).ToList();
                var colegioesAlumnos = context.Colegioes.Where(x => x.Id == 1)
                    .Select(x => new { Colegio = x, Alumnos = x.Alumnos.Where(e => e.Edad > 18).ToList() }).ToList();

            }
        }

        static void InsertarDataRelacionadaMuchosAMuchos()
        {
            using (var context = new ApplicationDbContext())
            {
                var alumno = context.Alumnos.FirstOrDefault();
                var grado = context.Grados.FirstOrDefault();

                var AlumnoGrado = new AlumnoGrado();

                AlumnoGrado.GradoId = grado.Id;
                AlumnoGrado.AlumnoId = alumno.Id;
                AlumnoGrado.Activo = true;

                context.Add(AlumnoGrado);
                context.SaveChanges();
            }
        }

        static void TraerDataRelacionadaMuchosAMuchos()
        {
            using (var context = new ApplicationDbContext())
            {
                var grado = context.Grados.Where(x => x.Id == 1).Include(x => x.AlumnosGrados)
                    .ThenInclude(y => y.Alumno).FirstOrDefault();
            }
        }

        static void StringInterpolationEnEF2()
        {
            using (var context = new ApplicationDbContext())
            {
                var nombre = "'German' or 1=1";
              
                var alumno = context.Alumnos.FromSql($"select * from Alumnos where Nombre = {nombre}").ToList();
            }
        }

        static void FiltroPorTipo()
        {
            using (var context = new ApplicationDbContext())
            {
               
                var alumnosGrados = context.AlumnosGrados.ToList();
            }
        }

        static void EliminadoSuave()
        {
            
            using (var context = new ApplicationDbContext())
            {
                var alumno = context.Alumnos.FirstOrDefault();
                context.Remove(alumno);
                context.SaveChanges();
            }
        }

        static void EjemploConcurrencyCheck()
        {
            using (var context = new ApplicationDbContext())
            {
                var est = context.Alumnos.FirstOrDefault();
                est.Nombre += " 2";
                est.Edad += 1;
                context.SaveChanges();
            }
        }

        static void FuncionEscalarEnEF()
        {
            using (var context = new ApplicationDbContext())
            {
                var alumnos = context.Alumnos
                    .Where(x => ApplicationDbContext.Cantidad_De_Grados_Activos(x.Id) > 0).ToList();
            }
        }

        static void FuncionalidadTableSplitting()
        {
            using (var context = new ApplicationDbContext())
            {
                var alumnos = context.Alumnos.Include(x => x.Detalles).ToList();
            }
        }
    }

    class Colegio
    {
        public Colegio()
        {
            Alumnos = new List<Alumno>();
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Alumno> Alumnos { get; set; }
    }

    class Alumno
    {
        public int Id { get; set; }
        [ConcurrencyCheck]
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int ColegioId { get; set; }
        public bool EstaEliminado { get; set; }

        private string _Apellido;

        public string Apellido
        {
            get { return _Apellido; }
            set
            {
                _Apellido = value.ToUpper();
            }
        }
        public Direccion Direccion { get; set; }
        public List<AlumnoGrado> AlumnosGrados { get; set; }
        public AlumnoDetalle Detalles { get; set; }
    }

    class AlumnoDetalle
    {
        public int Id { get; set; }
        public bool Becado { get; set; }
        public string Carrera { get; set; }
        public int CategoriaDePago { get; set; }
        public Alumno Alumno { get; set; }
    }

    class Direccion
    {
        public int Id { get; set; }
        public string Calle { get; set; }
        public int AlumnoId { get; set; }
    }

    class Grado
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Nombre { get; set; }
        public List<AlumnoGrado> AlumnosGrados { get; set; }
    }

    class AlumnoGrado
    {
        public int AlumnoId { get; set; }
        public int GradoId { get; set; }
        public bool Activo { get; set; }
        public Alumno Alumno { get; set; }
        public Grado Grado{ get; set; }
    }
}
