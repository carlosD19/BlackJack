using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJackENL;
using Npgsql;

namespace BlackJackDAL
{
    public class MesaDAL
    {
        public bool Eliminar(EMesa mesa)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"UPDATE mesa SET activo = @act
                               WHERE id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@act", mesa.Activo);
                cmd.Parameters.AddWithValue("@id", mesa.Id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Modificar(EMesa mesa)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"UPDATE mesa SET nombre = @nom, pass = @pass, capacidad = @cap, privada = @pri
                               WHERE id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nom", mesa.Nombre);
                cmd.Parameters.AddWithValue("@pass", mesa.Pass);
                cmd.Parameters.AddWithValue("@cap", mesa.Capacidad);
                cmd.Parameters.AddWithValue("@pri", mesa.Privada);
                cmd.Parameters.AddWithValue("@id", mesa.Id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Insertar(EMesa mesa)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"insert into mesa(nombre, pass, capacidad, privada) 
                                values(@nom, @pass, @cap, @pri)";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nom", mesa.Nombre);
                cmd.Parameters.AddWithValue("@pass", mesa.Pass);
                cmd.Parameters.AddWithValue("@cap", mesa.Capacidad);
                cmd.Parameters.AddWithValue("@pri", mesa.Privada);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<EMesa> Cargar()
        {
            List<EMesa> lista = new List<EMesa>();
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"select * from mesa where activo = @act";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@act", true);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(CargarMesa(reader));
                }
            }
            return lista;
        }

        private EMesa CargarMesa(NpgsqlDataReader reader)
        {
            EMesa mesa = new EMesa();
            mesa.Id = Int32.Parse(reader["id"].ToString());
            mesa.Privada = Boolean.Parse(reader["privada"].ToString());
            mesa.Activo = Boolean.Parse(reader["activo"].ToString());
            mesa.Pass = reader["pass"].ToString();
            mesa.Nombre = reader["nombre"].ToString();
            mesa.JugadorAct = Int32.Parse(reader["jug_act"].ToString());
            mesa.Capacidad = Int32.Parse(reader["capacidad"].ToString());
            mesa.ContadorJug = Int32.Parse(reader["contador"].ToString());
            return mesa;
        }
    }
}
