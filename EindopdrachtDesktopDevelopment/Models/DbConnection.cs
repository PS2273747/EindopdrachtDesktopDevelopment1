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
        public const string OK = "OK";

        string connection = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;



        // ---OPTIMISING DB EXPERIMENTING BELOW, ENTER AT YOUR OWN RISK, STUFF CAN BREAK---//

        // T is here a generic, it's type will be filled depending on the method called
        // the delegate contains the callbackmethod and will be an argument in the called method
        private delegate T EntityToClass<T>(MySqlDataReader reader);

        private string ReadObjects<T>(ICollection<T> objectList, string sqlCommand, EntityToClass<T> entityToClass)
        {
            // The ReadObjects has 4 parameters in case you want to use this for SQL Parameters
            // the sqlcommand will contain the query you need depending on the method called
            // the enitityToClass is your callback method which converts the data to the proper class
            // the null is there as parameter so the function set the sqlparameters as null
            return ReadObjects(objectList, sqlCommand, null, entityToClass);

        }

        // gets all elements from table ReadObjects
        private string ReadObjects<T>(ICollection<T> objectList, string sqlCommand, MySqlParameter[] sqlParameters,
                        EntityToClass<T> entityToClass)
        {
            if (objectList == null)
            {
                throw new ArgumentException("ILLEGALARGUMENT");
            }
            string methodResult = "";
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = sqlCommand;

                    // query parameters will be added to the SqlCommand 
                    if (sqlParameters != null)
                    {
                        sql.Parameters.AddRange(sqlParameters);
                    }
                    // reads the information from the triggered method which uses the reader
                    MySqlDataReader reader = sql.ExecuteReader();

                    //callback method who puts info from reader into the objectlist which is the delegate T
                    while (reader.Read())
                    {
                        objectList.Add(entityToClass(reader));
                    }
                    methodResult = "OK";
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(ReadObjects));
                    Console.Error.WriteLine(sqlCommand);
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult != "" ? methodResult : "OK";
        }

        //gets selected ID ReadObject
        private string ReadObject<T>(string sqlCommand, out T t, EntityToClass<T> entityToClass,
        MySqlParameter[] sqlParameters = null)
        {
            // t will get a value and can't be called on null
            string methodResult = ""; t = default(T);
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = sqlCommand;
                    if (sqlParameters != null)
                    {
                        sql.Parameters.AddRange(sqlParameters);
                    }
                    MySqlDataReader reader = sql.ExecuteReader();
                    if (reader.Read())
                    {
                        // puts information from reader into t
                        t = entityToClass(reader);
                    }
                    methodResult = "OK1";
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(ReadObjects));
                    Console.Error.WriteLine(sqlCommand);
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult != "" ? methodResult : "OK2";


        }

        public string GetGamers(ICollection<Gamer> gamers)
        {
            // contains the query and saves it into sqlCommand
            string sqlCommand = @"
                SELECT g.id, g.name
                FROM gamers g
                ";

            return ReadObjects(gamers, sqlCommand,
            // saves the info from the query into the object to use for binding
            reader => new Gamer()
            {
                ID = (int)reader["id"],
                Name = (string)reader["name"],
            });
        }

        private string CreateUpdateOrDeleteObject(string sqlCommand, MySqlParameter[] sqlParameters = null)
        {
            string methodResult = "";
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = sqlCommand;
                    sql.Parameters.AddRange(sqlParameters);
                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = "Unexpected number of rows affected";
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(ReadObjects));
                    Console.Error.WriteLine(sqlCommand);
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult != "" ? methodResult : OK;
        }

        public string GetGames(ICollection<Game> games)
        {
            // contains the query and saves it into sqlCommand
            string sqlCommand = @"
                SELECT g.id, g.Game
                FROM games g
                ";

            return ReadObjects(games, sqlCommand,
            // saves the info from the query into the object to use for binding and uses it as parameter for ReadObjects
            reader => new Game()
            {
                ID = (int)reader["id"],
                GameName = (string)reader["game"],
            });
        }

        public string GetGameID(ICollection<Game> games)
        {
            // contains the query and saves it into sqlCommand
            string sqlCommand = @"
                SELECT g.id
                FROM games g where g.id =
                "; // fucking hell how do i put a parameter here

            return ReadObjects(games, sqlCommand, // add parameter hereeeeeeeeeee (is it id, idfk?)
            // saves the info from the query into the object to use for binding and uses it as parameter for ReadObjects
            reader => new Game()
            {
                ID = (int)reader["id"],
               
            });
        }

        // DeleteGamerId verwijdert de Gamer met de id gamerId
        // fres: OK als succesvol, anders melding wat fout ging
        public string DeleteGamer(int gamerId)
        {
            // contains the query and saves it into sqlCommand
            string sqlCommand = @"
                DELETE FROM `gamers` 
                 WHERE id = @idVanDeGamer
                "; // fucking hell how do i put a parameter here

            MySqlParameter[] sqlParameters =
            {
                new MySqlParameter("@idVanDeGamer", gamerId)
            };
            return CreateUpdateOrDeleteObject(sqlCommand, sqlParameters);
        }
    }
}

