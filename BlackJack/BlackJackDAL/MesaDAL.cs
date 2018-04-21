using System;
using System.Collections.Generic;
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

        public bool VerificarP(string pass)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"select * from mesa where pass = @pass and activo = @act";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@pass", pass);
                cmd.Parameters.AddWithValue("@act", true);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                return reader.Read();
            }
        }

        public void SalirPartida(int id)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                //Abrir una conexion
                con.Open();
                //Definir la consulta
                string sql = @"delete from par_usu where id_jug = @idUsu";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@idUsu", id);
                cmd.ExecuteNonQuery();
            }
        }

        public void Actualizar(EMesa mesa)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                //Abrir una conexion
                con.Open();
                //Definir la consulta
                string sql = @"update mesa set jug_actual = @sig where id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", mesa.Id);
                cmd.Parameters.AddWithValue("@sig", Siguiente(mesa));
                cmd.ExecuteNonQuery();
            }
        }

        private int Siguiente(EMesa mesa)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                //Abrir una conexion
                con.Open();
                //Definir la consulta
                string sql = @"select id_jug from par_usu
                                where id_par = @idP and orden > (select orden from par_usu where id_jug = @idJ and id_par = @idP)
                                order by orden limit 1";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@idP", mesa.Id);
                cmd.Parameters.AddWithValue("@idJ", mesa.JugadorAct);
                object o = cmd.ExecuteScalar();
                int id = o == DBNull.Value ? 0 : Convert.ToInt32(o);
                if (id > 0)
                {
                    return id;
                }
                cmd.Parameters.Clear();
                sql = @"select id_jug from par_usu
                                where id_par = @idP 
                                order by orden limit 1";
                cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@idP", mesa.Id);
                o = cmd.ExecuteScalar();
                id = o == DBNull.Value ? 0 : Convert.ToInt32(o);
                return id;
            }
        }

        public EMesa CargarPartidaID(int id)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"select * from mesa where id = @id and activo = @act";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@act", true);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return CargarPartida(reader);
                }
            }
            return null;
        }

        private EMesa CargarPartida(NpgsqlDataReader reader)
        {
            EMesa m = new EMesa();
            m.Id = Int32.Parse(reader["id"].ToString());
            m.Nombre = reader["nombre"].ToString();
            m.Capacidad = Int32.Parse(reader["capacidad"].ToString());
            m.Pass = reader["pass"].ToString();
            m.Privada = Boolean.Parse(reader["privada"].ToString());
            m.JugadorAct = Int32.Parse(reader["jug_actual"].ToString());
            m.JugadorAct = Int32.Parse(reader["turno"].ToString());
            return m;
        }

        public EMesa Unirse(EUsuario usuario, EMesa mesa, string pass)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"insert into par_usu(id_jug, id_mesa, turno) 
                               values (@id,
                                      (select id from mesa where id = @idMesa and pass = @pass), (select turno from mesa where id = @idMesa2) + 1) 
                               returning id_mesa";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", usuario.Id);
                cmd.Parameters.AddWithValue("@idMesa", mesa.Id);
                cmd.Parameters.AddWithValue("@pass", pass);
                cmd.Parameters.AddWithValue("@idMesa2", mesa.Id);
                int idMesa = Convert.ToInt32(cmd.ExecuteScalar());
                if (idMesa > 0)
                {
                    con.Close();
                    ActualizarTurno(mesa);
                    ActualizarMesa(idMesa, usuario.Id);
                }
                return CargarPartidaID(idMesa);
            }
        }

        private void ActualizarMesa(int idMesa, int idUsu)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"select jug_actual from mesa where id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idMesa);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int jugActual = Int32.Parse(reader["jug_actual"].ToString());
                    con.Close();
                    if (jugActual == 0)
                    {
                        con.Open();
                        string sql2 = @"update mesa set jug_actual = @jugAct where id = @id";
                        NpgsqlCommand cmd2 = new NpgsqlCommand(sql2, con);
                        cmd2.Parameters.AddWithValue("@jugAct", idUsu);
                        cmd2.Parameters.AddWithValue("@id", idMesa);
                        NpgsqlDataReader reader2 = cmd2.ExecuteReader();
                    }
                }
            }
        }

        private void ActualizarTurno(EMesa mesa)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                //Abrir una conexion
                con.Open();
                //Definir la consulta
                string sql = @"update mesa set turno = (select turno from mesa where id = @id) + 1 where id = @id2";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", mesa.Id);
                cmd.Parameters.AddWithValue("@id2", mesa.Id);
                cmd.ExecuteNonQuery();
                con.Close();
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
            mesa.JugadorAct = Int32.Parse(reader["jug_actual"].ToString());
            mesa.Capacidad = Int32.Parse(reader["capacidad"].ToString());
            mesa.Turno = Int32.Parse(reader["turno"].ToString());
            return mesa;
        }
    }
}
