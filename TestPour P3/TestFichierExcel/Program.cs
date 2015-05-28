using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace TestFichierExcel
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo di = new DirectoryInfo(@"C:\Users\Kevin\Desktop\ORL");

            FileInfo[] listFile = di.GetFiles();
            string connexionString = @"Data Source = C:\Users\Kevin\Desktop\BDD_CHUV; Version = 3";

            SQLiteConnection con = new SQLiteConnection(connexionString);
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    Console.WriteLine("Connexion Success");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            for (int i = 0; i < listFile.Length; i++)
            //int i = 83;
            {
                string[] tab = listFile[i].Name.Split('.');
                int nbP = int.Parse(tab[0].Substring(1,tab[0].Length-1));

                //Creation du Patient
                string sql = "insert into Patients (Prenom) values ('Anonyme p" + nbP + "')";
                Console.WriteLine(sql);
                SQLiteCommand command = new SQLiteCommand(sql, con);
                command.ExecuteNonQuery();

                sql = "select * from Patients";
                command = new SQLiteCommand(sql, con);
                SQLiteDataReader reader = command.ExecuteReader();

                int idPatient = 0;
                
                while (reader.Read())
                {
                    idPatient = int.Parse(reader["ID"].ToString());
                } 


                string filePath = listFile[i].FullName;
                Console.WriteLine(filePath);
                string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0\";";
                OleDbConnection connection = new OleDbConnection(connectionString);


                string cmdText = "SELECT * FROM [p"+nbP+"$]";
                OleDbCommand command2 = new OleDbCommand(cmdText, connection);

                OleDbDataReader reader2 = null;
                try
                {
                    command2.Connection.Open();
                    reader2 = command2.ExecuteReader();
                    Console.WriteLine("Passage");
                    if (reader2.HasRows)
                    {
                        int nb = 1;
                        while (reader2.Read())
                        {
                            
                            try
                            {
                                Console.WriteLine(nb);
                                string d = reader2[0].ToString().Substring(0, 10);
                                Console.WriteLine(d);
                                string[] tabInter = d.Split('/');
                                int jour = int.Parse(tabInter[0]);
                                int mois = int.Parse(tabInter[1]);
                                int annee = int.Parse(tabInter[2]);

                                var timeSpan = (new DateTime(annee, mois, jour) - new DateTime(1970, 1, 1, 0, 0, 0));
                                long date = (long)timeSpan.TotalSeconds;

                                int nbSeance = nb;// int.Parse(reader2[1].ToString());

                                double xManu = 0;
                                double yManu = 0;
                                double zManu = 0;
                                double rollManu = 0;

                                double xAuto = 0;
                                double yAuto = 0;
                                double zAuto = 0;
                                double rollAuto = 0;


                                string xM = reader2[2].ToString().Replace('.', ',');
                                if (xM != "" && xM != "X")
                                    xManu = double.Parse(xM);


                                string yM = reader2[3].ToString().Replace('.', ',');
                                if (yM != "" && yM != "X")
                                    yManu = double.Parse(yM);


                                string zM = reader2[4].ToString().Replace('.', ',');
                                if (zM != "" && yM != "X")
                                    zManu = double.Parse(zM);


                                string rM = reader2[5].ToString().Replace('.', ',');
                                if (rM != "" && rM != "X")
                                    rollManu = double.Parse(rM);



                                xAuto = double.Parse(reader2[6].ToString().Replace('.', ','));



                                yAuto = double.Parse(reader2[7].ToString().Replace('.', ','));



                                zAuto = double.Parse(reader2[8].ToString().Replace('.', ','));



                                rollAuto = double.Parse(reader2[9].ToString().Replace('.', ','));

                                if (xM == "X")
                                    xManu = xAuto;
                                if (yM == "X")
                                    yManu = yAuto;
                                if (zM == "X")
                                    zManu = zAuto;
                                if (rM == "X")
                                    rollManu = rollAuto;



                                sql = "insert into Traitements (Date,Nb_Seance,X_Manu,Y_Manu,Z_Manu,Roll_Manu,X_Auto,Y_Auto,Z_Auto,Roll_Auto,ID_Patient) values ('" + date + "','" + nbSeance + "','" + xManu + "','" + yManu + "','" + zManu + "','" + rollManu + "','" + xAuto + "','" + yAuto + "','" + zAuto + "','" + rollAuto + "','" + idPatient + "')";

                                SQLiteCommand commands = new SQLiteCommand(sql, con);
                                commands.ExecuteNonQuery();
                                nb++;
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine("Erreur Fichier "+listFile[i].Name);

                                //Console.WriteLine(e);

                                //Console.ReadKey();
                            }
                        
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Lecture impossible du fichier");
                }
                command2.Connection.Close();
            }
            Console.WriteLine("Success Tout est importé!!!!!!!!!! GG !!!!!");

            Console.ReadKey();
        }
    }
}
