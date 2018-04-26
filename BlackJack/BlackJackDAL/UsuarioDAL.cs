using BlackJackENL;
using System;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackDAL
{
    public class UsuarioDAL
    {
        /// <summary>
        /// Verificar si el usuario ya esta registrado
        /// </summary>
        /// <param name="usuario">usuario que se va a validar</param>
        /// <returns>un usuario</returns>
        public EUsuario Verificar(EUsuario usuario)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"select * from usuario where (email = @email and id_app = @id)";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@email", usuario.Email);
                cmd.Parameters.AddWithValue("@id", usuario.IdApp);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return CargarUsuario(reader);
                }
                else
                {
                    con.Close();
                    return InsertarUsuario(usuario);
                }
            }
        }
        /// <summary>
        /// Metodo que agrega dinero al jugador
        /// </summary>
        /// <param name="id">id del jugador</param>
        /// <param name="value">valor a agregar</param>
        /// <returns>true si lo agrego y false sino</returns>
        public bool AgregarDinero(int id, int value)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"update usuario set dinero = dinero + @din where id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@din", value);
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        /// <summary>
        /// Carga la lista de usuarios
        /// </summary>
        /// <returns>lista de usuarios</returns>
        public List<EUsuario> Cargar()
        {
            List<EUsuario> lista = new List<EUsuario>();
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"select * from usuario";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(CargarUsuario(reader));
                }
            }
            return lista;
        }
        /// <summary>
        /// Metodo que agrega un usuario a la BD
        /// </summary>
        /// <param name="usuario">usuario que se va a registrar</param>
        /// <returns>un usuario</returns>
        private EUsuario InsertarUsuario(EUsuario usuario)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"insert into usuario(id_app, nombre, email, apellido, imagen) 
                            values (@id, @nom, @email, @ape, @img) returning id_app";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", usuario.IdApp);
                cmd.Parameters.AddWithValue("@nom", usuario.Nombre);
                cmd.Parameters.AddWithValue("@email", usuario.Email);
                cmd.Parameters.AddWithValue("@ape", usuario.Apellido);
                cmd.Parameters.AddWithValue("@img", usuario.Imagen);
                long idApp = long.Parse(cmd.ExecuteScalar().ToString());
                if (idApp > 0)
                {
                    string sql2 = @"select * from usuario where (email = @email or id_app = @id)";
                    NpgsqlCommand cmd2 = new NpgsqlCommand(sql2, con);
                    cmd2.Parameters.AddWithValue("@email", usuario.Email);
                    cmd2.Parameters.AddWithValue("@id", idApp);
                    NpgsqlDataReader reader = cmd2.ExecuteReader();
                    if (reader.Read())
                    {
                        return CargarUsuario(reader);
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Carga los atributos del usuario
        /// </summary>
        /// <param name="reader">atributos del usuario desde la base de datos</param>
        /// <returns>un usuario</returns>
        private EUsuario CargarUsuario(NpgsqlDataReader reader)
        {
            EUsuario usu = new EUsuario();
            usu.Id = Convert.ToInt32(reader["id"].ToString());
            usu.IdApp = long.Parse(reader["id_app"].ToString());
            usu.Nombre = reader["nombre"].ToString();
            usu.Apellido = reader["apellido"].ToString();
            usu.Imagen = reader["imagen"].ToString();
            usu.Apostado = Double.Parse(reader["apostado"].ToString());
            usu.Ganado = Double.Parse(reader["ganado"].ToString());
            usu.Dinero = Double.Parse(reader["dinero"].ToString());
            usu.Email = reader["email"].ToString();
            return usu;
        }
    }
}
