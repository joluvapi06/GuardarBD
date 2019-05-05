using GuardarBD.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace GuardarBD.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Guardar(String nombre, String email, HttpPostedFileBase imagen)
        {
            //[Convertir imagen en Base64]
            string imgBase64 = string.Empty;            
            BinaryReader dr = new System.IO.BinaryReader(imagen.InputStream);
            byte[] imgBytes = new byte[imagen.ContentLength];
            imgBytes = dr.ReadBytes(imagen.ContentLength);
            imgBase64 = "data:" + imagen.ContentType + ";base64," + Convert.ToBase64String(imgBytes);

            //[Conexion a la Base de Datos]
            SqlConnection con = new SqlConnection(@"Data Source=PC-JOSE\DBCURSO; Initial Catalog=CursoDB; User ID=sa; Password=qwerty456");
            SqlCommand cmd = new SqlCommand(@"INSERT INTO [Empleados] (Nombre,Email,Imagen) VALUES (@nombre,@email,@imagen)", con);
            cmd.Parameters.Add(new SqlParameter("nombre", nombre));
            cmd.Parameters.Add(new SqlParameter("email", email));
            cmd.Parameters.Add(new SqlParameter("imagen", imgBase64));
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                //return Content("Guardado !exito!");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            finally
            {
                con.Close();
            }

        }
        public ActionResult Datos()
        {
            //[Almacenar la consulta SELECT en la List]
            List<Empleados> modelo = new List<Empleados>();

            SqlConnection con = new SqlConnection(@"Data Source=PC-JOSE\DBCURSO; Initial Catalog=CursoDB; User ID=sa; Password=qwerty456");
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM [Empleados];", con);
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    modelo.Add(new Empleados { empleado_id = dr.GetInt32(0) ,nombre = dr.GetString(1), email = dr.GetString(2), imagen = dr.GetString(3) });
                }
                return View(modelo);

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            finally
            {
                con.Close();
            }

        }              
        public ActionResult Email(String email)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587; //Puerto
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Host = "smtp.gmail.com"; //Servidor
            //smtp.Host = "outlook.office365.com"; //Servidor
            //smtp.Credentials = new NetworkCredentials("jose.valtierrap@my.unitec.edu.mx", "14932418");//Usuario y contraseña
            smtp.Credentials = new System.Net.NetworkCredential("josel.valtierra.sk@gmail.com", "Mercury$3l3nium");

            MailMessage mail = new MailMessage();
            //mail.To.Add("jose.valtierrap@my.unitec.edu.mx"); //Destinatarios correos
            mail.To.Add(email);
            //mail.To.Add();
            mail.Subject = "Prueba";
            mail.Body = "<h1>Prueba de Envio de Correo</h1>";
            mail.IsBodyHtml = true;
            mail.From = new MailAddress("josel.valtierra.skp@gmail.com", "Jose Luis Valtierra"); //Remitente (correo y nombre)
            try
            {
                smtp.Send(mail);
                return Content("E-mail Enviado");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        public ActionResult Formulario()
        {
            return View();
        }
        public ActionResult InsertarEmpleado()
        {
            //Vista donde esta el formulario donde se almacena el empleado
            return View();
        }
        public ActionResult GuardarEmpleado(String nombre, String pApellido, String sApellid, String genero, String telefono, Int32 edad,
                                            DateTime fecha_nacimiento, DateTime fecha_completa)
        {
            //[Accion que guarda los datos del Empleado con la accion del Boton Guardar o Insertar]
            //[Conexion a la Base de Datos]            
            SqlConnection con = new SqlConnection(@"Data Source=PC-JOSE\DBCURSO; Initial Catalog=CursoDB; User ID=sa; Password=qwerty456");
            SqlCommand cmd = new SqlCommand(@"INSERT INTO [Empleado] (nombre,pApellido,sApellid,genero,telefono,edad,fecha_nacimiento,fecha_completa) 
                                                VALUES (@nombre,@pApellido,@sApellid,@genero,@telefono,@edad,@fecha_nacimiento,@fecha_completa)", con);
            cmd.Parameters.Add(new SqlParameter("nombre", nombre));
            cmd.Parameters.Add(new SqlParameter("pApellido", pApellido));
            cmd.Parameters.Add(new SqlParameter("sApellid", sApellid));
            cmd.Parameters.Add(new SqlParameter("genero", genero));
            cmd.Parameters.Add(new SqlParameter("telefono", telefono));
            cmd.Parameters.Add(new SqlParameter("edad", edad));
            cmd.Parameters.Add(new SqlParameter("fecha_nacimiento", fecha_nacimiento));
            cmd.Parameters.Add(new SqlParameter("fecha_completa", fecha_completa));
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                //return Content("Empleado Guardado !exito!");
                return RedirectToAction("DatosEmpleado");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public ActionResult DatosEmpleado()
        {
            //[Almacenar la consulta SELECT en la List]
            List<Empleado> modelo = new List<Empleado>();

            SqlConnection con = new SqlConnection(@"Data Source=PC-JOSE\DBCURSO; Initial Catalog=CursoDB; User ID=sa; Password=qwerty456");
            SqlCommand cmd = new SqlCommand(@"SELECT TOP 10 * FROM [Empleado] ORDER BY id_empleado DESC;", con);
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    modelo.Add(new Empleado
                    {
                        id_empleado = dr.GetInt32(0),
                        nombre = dr.GetString(1),
                        pApellido = dr.GetString(2),
                        sApellid = dr.GetString(3),
                        genero = dr.GetString(4),
                        telefono = dr.GetString(5),
                        edad = dr.GetInt32(6),
                        fecha_nacimiento = dr.GetDateTime(7),
                        fecha_completa = dr.GetDateTime(8)
                    });
                }
                return View(modelo);

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public ActionResult ConsultarEmpleadoId(Int32 id_empleado)
        {
            List<Empleado> modelo = new List<Empleado>();
            SqlConnection con = new SqlConnection(@"Data Source=PC-JOSE\DBCURSO; Initial Catalog=CursoDB; User ID=sa; Password=qwerty456");
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM [Empleado] WHERE id_empleado=@id_empleado;", con);
            cmd.Parameters.Add(new SqlParameter("id_empleado", id_empleado));
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    modelo.Add(new Empleado
                    {
                        id_empleado = dr.GetInt32(0),
                        nombre = dr.GetString(1),
                        pApellido = dr.GetString(2),
                        sApellid = dr.GetString(3),
                        genero = dr.GetString(4),
                        telefono = dr.GetString(5),
                        edad = dr.GetInt32(6),
                        fecha_nacimiento = dr.GetDateTime(7),
                        fecha_completa = dr.GetDateTime(8)
                    });
                }
                return View(modelo);

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public ActionResult ConsultarEmpleado(String nombre, String pApellido, String sApellid)
        {
            List<Empleado> modelo = new List<Empleado>();
            SqlConnection con = new SqlConnection(@"Data Source=PC-JOSE\DBCURSO; Initial Catalog=CursoDB; User ID=sa; Password=qwerty456");
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM [Empleado] WHERE nombre=@nombre OR pApellido=@pApellido OR sApellid=@sApellid;", con);
            cmd.Parameters.Add(new SqlParameter("nombre", nombre));
            cmd.Parameters.Add(new SqlParameter("pApellido", pApellido));
            cmd.Parameters.Add(new SqlParameter("sApellid", sApellid));
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    modelo.Add(new Empleado
                    {
                        id_empleado = dr.GetInt32(0),
                        nombre = dr.GetString(1),
                        pApellido = dr.GetString(2),
                        sApellid = dr.GetString(3),
                        genero = dr.GetString(4),
                        telefono = dr.GetString(5),
                        edad = dr.GetInt32(6),
                        fecha_nacimiento = dr.GetDateTime(7),
                        fecha_completa = dr.GetDateTime(8)
                    });
                }
                return View(modelo);

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public ActionResult BorrarEmpleado(Int32 id_empleado)
        {
            SqlConnection con = new SqlConnection(@"Data Source=PC-JOSE\DBCURSO; Initial Catalog=CursoDB; User ID=sa; Password=qwerty456");
            SqlCommand cmd = new SqlCommand(@"DELETE FROM [Empleado] WHERE id_empleado= @id_empleado;", con);
            cmd.Parameters.Add(new SqlParameter("id_empleado", id_empleado));
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return RedirectToAction("DatosEmpleado");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public ActionResult Actualizar(Int32 id_empleado)
        {
            //[Vista donde se encuentra el formulario para actualizar]
            List<Empleado> modelo = new List<Empleado>();
            SqlConnection con = new SqlConnection(@"Data Source=PC-JOSE\DBCURSO; Initial Catalog=CursoDB; User ID=sa; Password=qwerty456");
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM [Empleado] WHERE id_empleado=@id_empleado;", con);
            cmd.Parameters.Add(new SqlParameter("id_empleado", id_empleado));
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    modelo.Add(new Empleado
                    {
                        id_empleado = dr.GetInt32(0),
                        nombre = dr.GetString(1),
                        pApellido = dr.GetString(2),
                        sApellid = dr.GetString(3),
                        genero = dr.GetString(4),
                        telefono = dr.GetString(5),
                        edad = dr.GetInt32(6),
                        fecha_nacimiento = dr.GetDateTime(7),
                        fecha_completa = dr.GetDateTime(8)
                    });
                }
                return View(modelo);

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public ActionResult ActualizarEmpleado(Int32 id_empleado, String nombre, String pApellido, String sApellid, String telefono, Int32 edad)
        {
            SqlConnection con = new SqlConnection(@"Data Source=PC-JOSE\DBCURSO; Initial Catalog=CursoDB; User ID=sa; Password=qwerty456");
            SqlCommand cmd = new SqlCommand(@"UPDATE [Empleado] SET nombre=@nombre, pApellido=@pApellido, sApellid=@sApellid, telefono=@telefono, 
                                            edad=@edad WHERE id_empleado=@id_empleado;", con);
            cmd.Parameters.Add(new SqlParameter("id_empleado", id_empleado));
            cmd.Parameters.Add(new SqlParameter("nombre", nombre));
            cmd.Parameters.Add(new SqlParameter("pApellido", pApellido));
            cmd.Parameters.Add(new SqlParameter("sApellid", sApellid));            
            cmd.Parameters.Add(new SqlParameter("telefono", telefono));
            cmd.Parameters.Add(new SqlParameter("edad", edad));            
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return RedirectToAction("DatosEmpleado");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }
}