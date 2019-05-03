using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuardarBD.Models
{
    public class Empleados
    {
        public Int32 empleado_id { get; set; }
        public String nombre { get; set; }
        public String email { get; set; }
        public String imagen { get; set; }
    }
}