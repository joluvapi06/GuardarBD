using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuardarBD.Models
{
    public class Empleado
    {
        public Int32 id_empleado { get; set; }
        public String nombre { get; set; }
        public String pApellido { get; set; }
        public String sApellid { get; set; }
        public String genero { get; set; }
        public String telefono { get; set; }
        public Int32 edad { get; set; }
        public DateTime fecha_nacimiento { get; set; }
        public DateTime fecha_completa { get; set; }
    }
}