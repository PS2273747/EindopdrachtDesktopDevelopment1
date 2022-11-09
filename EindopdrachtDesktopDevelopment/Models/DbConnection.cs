using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace EindopdrachtDesktopDevelopment.Models
{
    internal class DbConnection
    {
        string connection = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        public string GetGamers(ICollection<Gamer> gamers)
        {
            // if gamers is empty show error message
            if (gamers == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetGamers");
            }
            string methodResult = "unknown";
            //making connection with de database
            using(MySqlConnection conn = new MySqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                    SELECT g.id, g.Name
                    FROM gamers g
                    ";
                    MySqlDataReader reader = sql.ExecuteReader();
                    while (reader.Read()) 
                    {
                        // add the database information from the query into variable gamer, which is from the Object Gamer.
                        Gamer gamer = new Gamer() { 

                            ID = (int)reader["id"],
                            Name = (string)reader["Name"]
                        };

                        // puts the information from gamer into the Icollection gamers as an item
                        gamers.Add(gamer);
                    }
                    methodResult = "ok";
                }
                    catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetGamers));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }
        public string GetGames(ICollection<Game> games)
        {
            // if gamers is empty show error message
            if (games == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetGames");
            }
            string methodResult = "unknown";
            //making connection with de database
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                    SELECT g.id, g.Game
                    FROM games g
                    ";
                    MySqlDataReader reader = sql.ExecuteReader();
                    while (reader.Read())
                    {
                        // add the database information from the query into variable gamer, which is from the Object Game.
                        Game game= new Game()
                        {

                            ID = (int)reader["id"],
                            GameName = (string)reader["Game"]
                        };

                        // puts the information from gamer into the Icollection games as an item
                        games.Add(game);
                    }
                    methodResult = "ok";
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetGames));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

    }
}

