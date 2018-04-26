using System;
using System.Collections.Generic;
using System.Net;
using BlackJackENL;
using Newtonsoft.Json;
using Npgsql;

namespace BlackJackDAL
{
    public class MesaDAL
    {
        /// <summary>
        /// Metodo de eliminar mesa
        /// </summary>
        /// <param name="mesa">mesa que se va a eliminar</param>
        /// <returns>si se elimino</returns>
        public bool Eliminar(EMesa mesa)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"UPDATE mesa SET activo = @act
                               WHERE id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@act", false);
                cmd.Parameters.AddWithValue("@id", mesa.Id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        /// <summary>
        /// Metodo de modificar mesa
        /// </summary>
        /// <param name="mesa">mesa que se va a modificar</param>
        /// <returns>si se modifico</returns>
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
        /// <summary>
        /// Metodo de insertar mesa
        /// </summary>
        /// <param name="mesa">mesa que se va a insertar</param>
        /// <returns>si se inserto</returns>
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
        /// <summary>
        /// Metodo de insertar ficha
        /// </summary>
        /// <param name="ficha">url de la ficha</param>
        /// <param name="id">id del jugador</param>
        public void InsertarFicha(string ficha, int id)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"insert into ficha_usu(ficha, id_jug) 
                                values(@nom, @idJ)";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nom", ficha);
                cmd.Parameters.AddWithValue("@idJ", id);
                cmd.ExecuteNonQuery();
                con.Close();
                con.Open();
                string sql1 = @"update usuario set aposto = @apos where id= @id";
                NpgsqlCommand cmd1 = new NpgsqlCommand(sql1, con);
                cmd1.Parameters.AddWithValue("@apos", true);
                cmd1.Parameters.AddWithValue("@id", id);
                cmd1.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Metodo para que un jugador apueste
        /// </summary>
        /// <param name="id">id del jugador</param>
        /// <param name="apuesta">apuesta del jugador</param>
        /// <param name="tipo">si apuesta o no</param>
        public void Apostar(int id, int apuesta, bool tipo)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"update usuario set aposto = @apos, apostado = apostado + @apuesta, dinero = dinero - @apuesta2, apuesta_temp = @apuestaT where id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@apos", tipo);
                cmd.Parameters.AddWithValue("@apuesta", apuesta);
                cmd.Parameters.AddWithValue("@apuesta2", apuesta);
                cmd.Parameters.AddWithValue("@apuestaT", apuesta);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Metodo para saber si tiene blackjack
        /// </summary>
        /// <param name="id">id del jugador</param>
        public void ActualizarBJ(int id)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"update usuario set blackjack = @blackJ where id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@blackJ", true);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Actualiza el dinero del jugador
        /// </summary>
        /// <param name="usu">usuario a actualizar</param>
        /// <param name="dinero">cantidad de dinero</param>
        public void ActualizarDinero(EUsuario usu, int dinero)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"update usuario set dinero = dinero + @din, blackjack = @bj, ganado = ganado + @gan,
                                apuesta_temp = @apuesta  where id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", usu.Id);
                cmd.Parameters.AddWithValue("@din", dinero);
                cmd.Parameters.AddWithValue("@gan", dinero);
                cmd.Parameters.AddWithValue("@bj", false);
                cmd.Parameters.AddWithValue("@apuesta", 0);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        /// <summary>
        /// Reparte cartas a los usuarios
        /// </summary>
        /// <param name="jugadores">jugadores a los que se le van a repartir cartas</param>
        /// <param name="deckID">deck de mazo</param>
        /// <param name="mesaID">mesa en la que estan jugando</param>
        public void Repartir(List<EUsuario> jugadores, string deckID, int mesaID)
        {
            for (int i = 0; i < jugadores.Count; i++)
            {
                if (jugadores[i].Aposto)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        ECarta carta = new ECarta();
                        string URL = String.Format("https://deckofcardsapi.com/api/deck/{0}/draw/?count=1", deckID);
                        string json = new WebClient().DownloadString(URL);
                        carta = JsonConvert.DeserializeObject<ECarta>(json);

                        using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
                        {
                            con.Open();
                            string sql = @"insert into carta_usu(carta, valor, id_jug) 
                                values(@carta, @valor, @idJ)";
                            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                            cmd.Parameters.AddWithValue("@carta", carta.cards[0].image);
                            cmd.Parameters.AddWithValue("@valor", carta.cards[0].value);
                            cmd.Parameters.AddWithValue("@idJ", jugadores[i].Id);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            for (int j = 0; j < 2; j++)
            {
                ECarta carta = new ECarta();
                string URL = String.Format("https://deckofcardsapi.com/api/deck/{0}/draw/?count=1", deckID);
                string json = new WebClient().DownloadString(URL);
                carta = JsonConvert.DeserializeObject<ECarta>(json);

                using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
                {
                    con.Open();
                    string sql = @"insert into carta_mesa(carta, valor, id_mesa) 
                                values(@carta, @valor, @idM)";
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@carta", carta.cards[0].image);
                    cmd.Parameters.AddWithValue("@valor", carta.cards[0].value);
                    cmd.Parameters.AddWithValue("@idM", mesaID);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            CambiarEstadoJuego(mesaID, true);
        }
        /// <summary>
        /// Agrega cartas de la mesa
        /// </summary>
        /// <param name="cartas">cartas de la mesa</param>
        /// <param name="deckID">deck del mazo</param>
        /// <param name="mesaID">mesa en la que juegan</param>
        public void AgregarCartaMesa(List<Card> cartas, string deckID, int mesaID)
        {
            ECarta carta = new ECarta();
            List<Card> tempCarta = new List<Card>();
            EUsuario usuario = new EUsuario();
            int suma = usuario.ContarCartas(cartas);
            int suma2 = 0;

            while (suma <= 16)
            {
                string URL = String.Format("https://deckofcardsapi.com/api/deck/{0}/draw/?count=1", deckID);
                string json = new WebClient().DownloadString(URL);
                carta = JsonConvert.DeserializeObject<ECarta>(json);
                tempCarta.Add(carta.cards[0]);
                suma2 = usuario.ContarCartas(tempCarta);
                suma = suma + suma2;
            }
            for (int i = 0; i < tempCarta.Count; i++)
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
                {
                    con.Open();
                    string sql = @"insert into carta_mesa(carta, valor, id_mesa) 
                                values(@carta, @valor, @idM)";
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@carta", tempCarta[i].image);
                    cmd.Parameters.AddWithValue("@valor", tempCarta[i].value);
                    cmd.Parameters.AddWithValue("@idM", mesaID);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        /// <summary>
        /// Cambia el estado del juego
        /// </summary>
        /// <param name="mesaID">mesa en la que se juega</param>
        /// <param name="estadoPartida">estado del juego</param>
        public void CambiarEstadoJuego(int mesaID, bool estadoPartida)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"update mesa set jugando = @jugan where id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", mesaID);
                cmd.Parameters.AddWithValue("@jugan", estadoPartida);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Verifica la contraseña de la mesa
        /// </summary>
        /// <param name="pass">contraseña</param>
        /// <returns>si es correcta</returns>
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
        /// <summary>
        /// Agrega cartas al usuario
        /// </summary>
        /// <param name="deckID"></param>
        /// <param name="id"></param>
        public void AgregarCarta(string deckID, int id)
        {
            ECarta carta = new ECarta();
            string URL = String.Format("https://deckofcardsapi.com/api/deck/{0}/draw/?count=1", deckID);
            string json = new WebClient().DownloadString(URL);
            carta = JsonConvert.DeserializeObject<ECarta>(json);

            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"insert into carta_usu(carta, valor, id_jug) 
                                values(@carta, @valor, @idJ)";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@carta", carta.cards[0].image);
                cmd.Parameters.AddWithValue("@valor", carta.cards[0].value);
                cmd.Parameters.AddWithValue("@idJ", id);
                cmd.ExecuteNonQuery();
            }
        }

        public void SalirPartida(int id, int mesaId)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                List<int> usuJug = new List<int>();
                string sql1 = @"select * from par_usu where id_mesa = @mesa and orden > 
                              (select orden from par_usu where id_jug = @id and id_mesa = @mesa2)
                              order by orden";
                NpgsqlCommand cmd1 = new NpgsqlCommand(sql1, con);
                cmd1.Parameters.AddWithValue("@mesa", mesaId);
                cmd1.Parameters.AddWithValue("@id", id);
                cmd1.Parameters.AddWithValue("@mesa2", mesaId);
                NpgsqlDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    usuJug.Add(Int32.Parse(reader["id"].ToString()));
                }
                con.Close();
                con.Open();
                string sql = @"delete from par_usu where id_jug = @idUsu";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@idUsu", id);
                cmd.ExecuteNonQuery();
                con.Close();
                EliminarFichasCartasTurno(id);
                for (int i = 0; i < usuJug.Count; i++)
                {
                    con.Open();
                    string sql2 = @"UPDATE par_usu SET turno = turno-1 WHERE id = @id";
                    NpgsqlCommand cmd2 = new NpgsqlCommand(sql2, con);
                    cmd2.Parameters.AddWithValue("@id", usuJug[i]);
                    cmd2.ExecuteNonQuery();
                    con.Close();
                }
                con.Open();
                string sql3 = @"UPDATE mesa SET turno = turno-1 WHERE id = @id returning turno";
                NpgsqlCommand cmd3 = new NpgsqlCommand(sql3, con);
                cmd3.Parameters.AddWithValue("@id", mesaId);
                int idM = Int32.Parse(cmd3.ExecuteScalar().ToString());
                con.Close();
                if (idM == 0)
                {
                    con.Open();
                    string sql4 = @"UPDATE mesa SET deck_id = null, jugando = @jug WHERE id = @id";
                    NpgsqlCommand cmd4 = new NpgsqlCommand(sql4, con);
                    cmd4.Parameters.AddWithValue("@id", mesaId);
                    cmd4.Parameters.AddWithValue("@jug", false);
                    cmd4.ExecuteNonQuery();
                    con.Close();
                    EliminarCartasMesa(mesaId);
                }
            }
        }

        public void EliminarCartasMesa(int mesaId)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql6 = @"delete from carta_mesa where id_mesa = @id";
                NpgsqlCommand cmd6 = new NpgsqlCommand(sql6, con);
                cmd6.Parameters.AddWithValue("@id", mesaId);
                cmd6.ExecuteNonQuery();
                con.Close();
                con.Open();
                string sql = @"update mesa set jugando = @jug where id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", mesaId);
                cmd.Parameters.AddWithValue("@jug", false);
                cmd.ExecuteNonQuery();
                con.Close();
                
            }
        }
        public void ActualizarDeck(EMesa mesa)
        {
            ECarta carta = new ECarta();
            string URL = String.Format("https://deckofcardsapi.com/api/deck/{0}/shuffle/", mesa.Deck_Id);
            string json = new WebClient().DownloadString(URL);
            carta = JsonConvert.DeserializeObject<ECarta>(json);

            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"update mesa set deck_id = @deck where id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", mesa.Id);
                cmd.Parameters.AddWithValue("@deck", carta.deck_id);
                cmd.ExecuteNonQuery();
            }

        }

        public void EliminarFichasCartasTurno(int id)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql6 = @"delete from ficha_usu where id_jug = @idUsu";
                NpgsqlCommand cmd6 = new NpgsqlCommand(sql6, con);
                cmd6.Parameters.AddWithValue("@idUsu", id);
                cmd6.ExecuteNonQuery();
                con.Close();
                con.Open();
                string sql5 = @"delete from carta_usu where id_jug = @idUsu";
                NpgsqlCommand cmd5 = new NpgsqlCommand(sql5, con);
                cmd5.Parameters.AddWithValue("@idUsu", id);
                cmd5.ExecuteNonQuery();
                con.Close();
                con.Open();
                string sql = @"update usuario set aposto = @apos where id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@apos", false);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void Actualizar(EMesa mesa)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
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
                con.Open();
                string sql = @"select id_jug from par_usu
                                where id_mesa = @idP and orden > (select orden from par_usu where id_jug = @idJ and id_mesa = @idP)
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
                                where id_mesa = @idP 
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
            List<Card> cartas = new List<Card>();
            EMesa mesa = new EMesa();
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"select * from mesa where id = @id and activo = @act and turno < capacidad";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@act", true);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    mesa = CargarPartida(reader);
                    mesa.Cartas = CargarCartasMesa(id);
                    return mesa;
                }
            }
            return null;
        }

        private List<Card> CargarCartasMesa(int id)
        {
            List<Card> cartas = new List<Card>();
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"select * from carta_mesa where id_mesa = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Card c = new Card();
                    c.image = reader["carta"].ToString();
                    c.value = reader["valor"].ToString();
                    cartas.Add(c);
                }
            }
            return cartas;
        }

        private EMesa CargarPartida(NpgsqlDataReader reader)
        {
            EMesa m = new EMesa();
            m.Id = Int32.Parse(reader["id"].ToString());
            m.Nombre = reader["nombre"].ToString();
            m.Capacidad = Int32.Parse(reader["capacidad"].ToString());
            m.Pass = reader["pass"].ToString();
            m.Privada = Boolean.Parse(reader["privada"].ToString());
            m.Jugando = Boolean.Parse(reader["jugando"].ToString());
            m.JugadorAct = Int32.Parse(reader["jug_actual"].ToString());
            m.Turno = Int32.Parse(reader["turno"].ToString());
            m.Deck_Id = reader["deck_id"].ToString();
            return m;
        }

        public EMesa Unirse(EUsuario usuario, EMesa mesa, string pass)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {

                EMesa mesaResult = new EMesa();
                mesaResult = CargarPartidaID(mesa.Id);
                if (mesaResult != null)
                {
                    con.Close();
                    con.Open();
                    string sql = @"insert into par_usu(id_jug, id_mesa, turno) 
                               values (@id,
                                      (select id from mesa where id = @idMesa and pass = @pass), (select turno from mesa where id = @idMesa2) + 1)";
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@id", usuario.Id);
                    cmd.Parameters.AddWithValue("@idMesa", mesa.Id);
                    cmd.Parameters.AddWithValue("@pass", pass);
                    cmd.Parameters.AddWithValue("@idMesa2", mesa.Id);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        con.Close();
                        ActualizarTurno(mesa);
                        ActualizarMesa(mesa.Id, usuario.Id);
                    }
                }
                return mesaResult;
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
                        EMazo mazo = new EMazo();
                        string URL = @"https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1";
                        string json = new WebClient().DownloadString(URL);
                        mazo = JsonConvert.DeserializeObject<EMazo>(json);
                        string deck_id = mazo.deck_id;

                        con.Open();
                        string sql2 = @"update mesa set jug_actual = @jugAct, deck_id = @deck where id = @id";
                        NpgsqlCommand cmd2 = new NpgsqlCommand(sql2, con);
                        cmd2.Parameters.AddWithValue("@jugAct", idUsu);
                        cmd2.Parameters.AddWithValue("@deck", deck_id);
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
                con.Open();
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
            mesa.Jugando = Boolean.Parse(reader["jugando"].ToString());
            mesa.Pass = reader["pass"].ToString();
            mesa.Nombre = reader["nombre"].ToString();
            mesa.JugadorAct = Int32.Parse(reader["jug_actual"].ToString());
            mesa.Capacidad = Int32.Parse(reader["capacidad"].ToString());
            mesa.Turno = Int32.Parse(reader["turno"].ToString());
            return mesa;
        }

        public List<EUsuario> CargarJugadores(EMesa mesa)
        {
            List<EUsuario> lista = new List<EUsuario>();
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"SELECT * FROM usuario FULL JOIN par_usu ON par_usu.id_jug = usuario.id
                                WHERE par_usu.id_mesa = @idMesa order by turno";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@idMesa", mesa.Id);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(CargarUsuario(reader));
                }
                foreach (EUsuario u in lista)
                {
                    u.Cartas = CargarCartas(u.Id);
                    u.Fichas = CargarFichas(u.Id);
                }
            }
            return lista;
        }

        private List<string> CargarFichas(int id)
        {
            List<string> fichas = new List<string>();
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"select * from ficha_usu where id_jug = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    fichas.Add(reader["ficha"].ToString());
                }
            }
            return fichas;
        }

        private List<Card> CargarCartas(int id)
        {
            List<Card> cartas = new List<Card>();
            using (NpgsqlConnection con = new NpgsqlConnection(Configuracion.ConStr))
            {
                con.Open();
                string sql = @"select * from carta_usu where id_jug = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Card c = new Card();
                    c.image = reader["carta"].ToString();
                    c.value = reader["valor"].ToString();
                    cartas.Add(c);
                }
            }
            return cartas;
        }

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
            usu.Turno = Int32.Parse(reader["turno"].ToString());
            usu.ApuestaTemp = Int32.Parse(reader["apuesta_temp"].ToString());
            usu.Aposto = Boolean.Parse(reader["aposto"].ToString());
            usu.BlackJack = Boolean.Parse(reader["blackjack"].ToString());
            return usu;
        }
    }
}
