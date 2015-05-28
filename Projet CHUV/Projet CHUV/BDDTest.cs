using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace Projet_CHUV
{
    class BDDTest
    {

        private SQLiteConnection con;

        public BDDTest()
        {
            string connexionString = @"Data Source = " + System.AppDomain.CurrentDomain.BaseDirectory + "/BDD_TEST; Version = 3";
            con = new SQLiteConnection(connexionString);
            con.Open();
        }

        public void generatePatient_Traitement(int nbP,int nbT)
        {
            for (int i = 0; i < nbP; i++)
            {

                string sql0 = "insert into Patients (Prenom) values (\'test"+i+ "\')";

                SQLiteCommand command0 = new SQLiteCommand(sql0, con);
                command0.ExecuteNonQuery();


                string sqlID = "select * from Patients where Prenom = \'test" + i + "\';";
                SQLiteCommand commandID = new SQLiteCommand(sqlID, con);
          
                SQLiteDataReader readerID = commandID.ExecuteReader();

                int id = 0;
                while (readerID.Read())
                {
                    id = int.Parse("" + readerID["ID"]);
                }

                for (int j = 0; j < nbT; j++)
                {
                    string sql = "select * from Traitements where ID_Patient = \'" + id + "\' ;";

                    SQLiteCommand command = new SQLiteCommand(sql, con);
                    Console.WriteLine(sql);
                    SQLiteDataReader reader = command.ExecuteReader();

                    int NbSeance = 0;
                    while (reader.Read())
                    {
                        NbSeance = int.Parse("" + reader["Nb_Seance"]);
                    }
                    NbSeance++;

                    Random rand = new Random();

                    double xAuto = rand.Next(0, 101) / 100.0;
                    double yAuto = rand.Next(0, 101) / 100.0;
                    double zAuto = rand.Next(0, 101) / 100.0;

                    double xManu = rand.Next(100, 201) / 100.0;
                    double yManu = rand.Next(100, 201) / 100.0;
                    double zManu = rand.Next(100, 201) / 100.0;

                    double rAuto = rand.Next(100, 201) / 100.0;
                    double rManu = rand.Next(200, 301) / 100.0;

                    double poid = rand.Next(500,1001)/10.0;

                    string sql2 = "insert into Traitements (X_Auto,Y_Auto,Z_Auto,X_Manu,Y_Manu,Z_Manu,Roll_Auto,Roll_Manu,Poids,Nb_Seance,ID_Patient) values (\'" + xAuto + "\',\'" + yAuto + "\',\'" + zAuto + "\',\'" + xManu + "\',\'" + yManu + "\',\'" + zManu + "\',\'" + rAuto + "\',\'" + rManu + "\',\'" + poid + "\',\'" + NbSeance + "\',\'" + id + "\')";

                    SQLiteCommand command2 = new SQLiteCommand(sql2, con);
                    command2.ExecuteNonQuery();
                }
            }
        }

    }
}
