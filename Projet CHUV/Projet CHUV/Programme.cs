using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Projet_CHUV
{

    public partial class Programme : Form
    {
        private List<List<double>> listPointsX;
        private List<List<double>> listPointsY;
        private List<List<double>> listPointsZ;
        private List<List<double>> listPointsD;
        private List<List<double>> listPointsR;
        private List<List<double>> listPointsW;
        private Boolean testSwitch;

        private List<List<string>> listRecherche;
        private int idPatientCourant;
        private string bdd = "/BDD_CHUV; Version = 3";
        private string bddTest = "/BDD_TEST; Version = 3";

        private double b, c;
        private bool switchEcart;

        private SQLiteConnection con;

        public Programme()
        {
            InitializeComponent();
            initListPoints(bdd);
        }

        private void initListPoints(string zonebdd)
        {
            b = 2.0;
            c = 7.0;
            switchEcart = true;

            listPointsX = new List<List<double>>();
            listPointsY = new List<List<double>>();
            listPointsZ = new List<List<double>>();
            listPointsD = new List<List<double>>();
            listPointsR = new List<List<double>>();
            listPointsW = new List<List<double>>();
            testSwitch = false;

            listRecherche = new List<List<string>>();

            string connexionString = @"Data Source = " + System.AppDomain.CurrentDomain.BaseDirectory + zonebdd;
            con = new SQLiteConnection(connexionString);
            con.Open();

            int nbElement = 1;

            int i = 1;

            while (nbElement != 0)
            {
                List<double> listX = new List<double>();
                List<double> listY = new List<double>();
                List<double> listZ = new List<double>();
                List<double> listD = new List<double>();
                List<double> listR = new List<double>();
                List<double> listW = new List<double>();


                string sql = "select * from Traitements where \"Nb_Seance\" = " + i;// +" and \"ID_Patient\" = " + 1045;
                
                SQLiteCommand command = new SQLiteCommand(sql, con);
                SQLiteDataReader reader = command.ExecuteReader();

                nbElement = 0;

                Console.WriteLine("i : " + i);
                while (reader.Read())
                {
                    string strxAuto = reader.GetValues().Get(3);
                    strxAuto = strxAuto.Replace(".", ",");

                    //Console.WriteLine(strxAuto);
                    double xAuto = double.Parse(strxAuto);
                    //Console.WriteLine(xAuto);

                    string strxManu = reader.GetValues().Get(6);
                    strxManu = strxManu.Replace(".", ",");

                    double xManu = double.Parse(strxManu);
                    //Console.WriteLine(xManu);
                    
                    double deltaX;
                    if(xAuto>=xManu)
                        deltaX = Math.Abs(xAuto - xManu);
                    else
                        deltaX = Math.Abs(xManu -xAuto);

                    //Console.WriteLine(deltaX);
                    listX.Add(deltaX);


                    //Console.WriteLine("" + reader["Nb_Seance"]);


                    string stryAuto = reader.GetValues().Get(4);
                    stryAuto = stryAuto.Replace(".", ",");

                    double yAuto = double.Parse(stryAuto);
                    //Console.WriteLine(xAuto);

                    string stryManu = reader.GetValues().Get(7);
                    stryManu = stryManu.Replace(".", ",");

                    double yManu = double.Parse(stryManu);
                    //Console.WriteLine(xManu);

                    double deltaY;
                    if (yAuto >= yManu)
                        deltaY = Math.Abs(yAuto - yManu);
                    else
                        deltaY = Math.Abs(yManu - yAuto);

                    //Console.WriteLine(deltaX);
                    listY.Add(deltaY);



                    string strzAuto = reader.GetValues().Get(5);
                    strzAuto = strzAuto.Replace(".", ",");

                    double zAuto = double.Parse(strzAuto);
                    //Console.WriteLine(xAuto);

                    string strzManu = reader.GetValues().Get(8);
                    strzManu = strzManu.Replace(".", ",");

                    double zManu = double.Parse(strzManu);
                    //Console.WriteLine(xManu);

                    double deltaZ;

                    if (zAuto >= zManu)
                        deltaZ = Math.Abs(zAuto - zManu);
                    else
                        deltaZ = Math.Abs(zManu - zAuto);
                    
                    //Console.WriteLine(deltaX);
                    listZ.Add(deltaZ);


                    string strRollAuto = reader.GetValues().Get(12);
                    strRollAuto = strRollAuto.Replace(".", ",");

                    double rollAuto = double.Parse(strRollAuto);
                    //Console.WriteLine(rollAuto);

                    string strRollManu = reader.GetValues().Get(13);
                    strRollManu = strRollManu.Replace(".", ",");

                    double rollManu = double.Parse(strRollManu);
                    //Console.WriteLine(rollManu);

                    double deltaRoll;
                    if (rollAuto >= rollManu)
                        deltaRoll = Math.Abs(rollAuto - rollManu);
                    else
                        deltaRoll = Math.Abs(rollManu - rollAuto);
                    
                    //Console.WriteLine(deltaRoll);
                    listR.Add(deltaRoll);


                    string strW = reader.GetValues().Get(18);
                    if(strW!="")
                    {
                        strW = strW.Replace(".", ",");
                        double w = double.Parse(strW);
                        listW.Add(w);
                    }
                    

                    listD.Add((Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ)));

                    nbElement++;
                }

                listPointsX.Add(listX);
                listPointsY.Add(listY);
                listPointsZ.Add(listZ);
                listPointsD.Add(listD);
                listPointsR.Add(listR);
                listPointsW.Add(listW);

                i++;
            }

            grapheDataManagement gestion = grapheDataManagement.Instance;

            Console.WriteLine("NB PointX : " + listPointsX.ElementAt(0).Count);

            gestion.initialise(listPointsX, listPointsY, listPointsZ, listPointsD,listPointsR,listPointsW);
            
        }

        public void DessinX_Paint(object sender, PaintEventArgs e)
        {
            grapheDataManagement.Instance.drawGraphX(e);
        }

        public void DessinY_Paint(object sender, PaintEventArgs e)
        {
            grapheDataManagement.Instance.drawGraphY(e);
        }

        public void DessinZ_Paint(object sender, PaintEventArgs e)
        {
            grapheDataManagement.Instance.drawGraphZ(e);
        }

        public void DessinW_Paint(object sender, PaintEventArgs e)
        {
            grapheDataManagement.Instance.drawGraphW(e);
        }

        public void DessinD_Paint(object sender, PaintEventArgs e)
        {
            grapheDataManagement.Instance.drawGraphD(e);
        }

        public void DessinR_Paint(object sender, PaintEventArgs e)
        {
            grapheDataManagement.Instance.drawGraphR(e);
        }


        private void fermerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void buttonCreation_Click(object sender, EventArgs e)
        {
            DialogResult result1 = MessageBox.Show("Voulez-vous vraiment créer ce nouveau Patient ?", "Question de création", MessageBoxButtons.YesNo);
            //Console.WriteLine(result1.ToString());//Yes --------- No

            if(result1.ToString().Equals("Yes"))
            {
                var timeSpanN = (dateTimeNaissance.Value - new DateTime(1970, 1, 1, 0, 0, 0));
                long dateNaissace = (long)timeSpanN.TotalSeconds;

                var timeSpanD = (dateTimeDate.Value - new DateTime(1970, 1, 1, 0, 0, 0));
                long date = (long)timeSpanD.TotalSeconds;

                string sexe = "Féminin";
                if (radioSexeM.Checked)
                    sexe = "Masculin";

                string sql = "insert into Patients (Prenom,DateNaissance,IPP,Sexe,Sejour_UF,Date,UF_nom,Heb,PTV1_Par_Fraction,PTV2_Par_Fraction,PTV3_Par_Fraction,PTV4_Par_Fraction,Stereotaxie,Nom) values (\'" + textBoxPrenom.Text + "\', \'" + dateNaissace + "\', \'" + textBoxIPP.Text + "\', \'" + sexe + "\', \'" + textBoxSejourUF.Text + "\', \'" + date + "\', \'" + textBoxUFNom.Text + "\', \'" + textBoxHeb.Text + "\', \'" + numericPTV1.Value + "\', \'" + numericPTV2.Value + "\', \'" + numericPTV3.Value + "\', \'" + numericPTV4.Value + "\', \'" + textBoxStereotaxie.Text + "\', \'" + textBoxNom.Text + "\')";

                SQLiteCommand command = new SQLiteCommand(sql, con);
                idPatientCourant = command.ExecuteNonQuery();
                MessageBox.Show("Patient Crée \nPrenom : " + textBoxPrenom.Text);
                //Recuperer l'ID du patient ajouter :/

            }
            
        }

        private void buttonRecherche_Click(object sender, EventArgs e)
        {
            listBoxRecherche.Items.Clear();
            listRecherche.Clear();

            string critere = "";

            switch (comboBoxChoixRecherche.SelectedItem.ToString())
            {
                case "Prenom":
                    critere = "Prenom";
                    break;
                case "Nom":
                    critere = "Nom";
                    break;
                case "UF_Nom":
                    critere = "UF_nom";
                    break;
                default:
                    break;
            }


            string sql = "select * from Patients where " + critere + " = '" + textBoxRecherche.Text + "' ;";           

            SQLiteCommand command = new SQLiteCommand(sql, con);
            Console.WriteLine(sql);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                List <string > listTemp = new List<string>();
                listBoxRecherche.Items.Add(" Prenom : " + reader["Prenom"] + "\n Nom : " + reader["Nom"] );
                listTemp.Add("" + reader["ID"]);
                listTemp.Add("" + reader["Prenom"]);
                listTemp.Add("" + reader["Nom"]);
                listRecherche.Add(listTemp);
            }
            
        }

        public void changementGraph(string id)
        {
            listPointsX = new List<List<double>>();
            listPointsY = new List<List<double>>();
            listPointsZ = new List<List<double>>();
            listPointsD = new List<List<double>>();
            listPointsR = new List<List<double>>();
            listPointsW = new List<List<double>>();
            
            string connexionString = @"Data Source = " + System.AppDomain.CurrentDomain.BaseDirectory + "/BDD_CHUV; Version = 3";
            con = new SQLiteConnection(connexionString);
            con.Open();

            int nbElement = 1;

            int i = 1;

            while (nbElement != 0)
            {
                List<double> listX = new List<double>();
                List<double> listY = new List<double>();
                List<double> listZ = new List<double>();
                List<double> listD = new List<double>();
                List<double> listR = new List<double>();
                List<double> listW = new List<double>();


                string sql = "select * from Traitements where \"Nb_Seance\" = " + i +" and \"ID_Patient\" = " + id;
                
                SQLiteCommand command = new SQLiteCommand(sql, con);
                SQLiteDataReader reader = command.ExecuteReader();

                nbElement = 0;

                while (reader.Read())
                {
                    string strxAuto = reader.GetValues().Get(3);
                    strxAuto = strxAuto.Replace(".", ",");

                    double xAuto = double.Parse(strxAuto);
                    //Console.WriteLine(xAuto);

                    string strxManu = reader.GetValues().Get(6);
                    strxManu = strxManu.Replace(".", ",");

                    double xManu = double.Parse(strxManu);
                    //Console.WriteLine(xManu);
                    
                    double deltaX;
                    if(xAuto>=xManu)
                        deltaX = Math.Abs(xAuto - xManu);
                    else
                        deltaX = Math.Abs(xManu -xAuto);

                    //Console.WriteLine(deltaX);
                    listX.Add(deltaX);


                    //Console.WriteLine("" + reader["Nb_Seance"]);


                    string stryAuto = reader.GetValues().Get(4);
                    stryAuto = stryAuto.Replace(".", ",");

                    double yAuto = double.Parse(stryAuto);
                    //Console.WriteLine(xAuto);

                    string stryManu = reader.GetValues().Get(7);
                    stryManu = stryManu.Replace(".", ",");

                    double yManu = double.Parse(stryManu);
                    //Console.WriteLine(xManu);

                    double deltaY;
                    if (yAuto >= yManu)
                        deltaY = Math.Abs(yAuto - yManu);
                    else
                        deltaY = Math.Abs(yManu - yAuto);

                    //Console.WriteLine(deltaX);
                    listY.Add(deltaY);



                    string strzAuto = reader.GetValues().Get(5);
                    strzAuto = strzAuto.Replace(".", ",");

                    double zAuto = double.Parse(strzAuto);
                    //Console.WriteLine(xAuto);

                    string strzManu = reader.GetValues().Get(8);
                    strzManu = strzManu.Replace(".", ",");

                    double zManu = double.Parse(strzManu);
                    //Console.WriteLine(xManu);

                    double deltaZ;

                    if (zAuto >= zManu)
                        deltaZ = Math.Abs(zAuto - zManu);
                    else
                        deltaZ = Math.Abs(zManu - zAuto);
                    
                    //Console.WriteLine(deltaX);
                    listZ.Add(deltaZ);


                    string strRollAuto = reader.GetValues().Get(12);
                    strRollAuto = strRollAuto.Replace(".", ",");

                    double rollAuto = double.Parse(strRollAuto);
                    //Console.WriteLine(rollAuto);

                    string strRollManu = reader.GetValues().Get(13);
                    strRollManu = strRollManu.Replace(".", ",");

                    double rollManu = double.Parse(strRollManu);
                    //Console.WriteLine(rollManu);

                    double deltaRoll;
                    if (rollAuto >= rollManu)
                        deltaRoll = Math.Abs(rollAuto - rollManu);
                    else
                        deltaRoll = Math.Abs(rollManu - rollAuto);
                    
                    //Console.WriteLine(deltaRoll);
                    listR.Add(deltaRoll);


                    string strW = reader.GetValues().Get(18);
                    if(strW!="")
                    {
                        strW = strW.Replace(".", ",");
                        double w = double.Parse(strW);
                        listW.Add(w);
                    }
                    

                    listD.Add((Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ))/2);

                    nbElement++;
                }

                listPointsX.Add(listX);
                listPointsY.Add(listY);
                listPointsZ.Add(listZ);
                listPointsD.Add(listD);
                listPointsR.Add(listR);
                listPointsW.Add(listW);

                i++;
            }

            grapheDataManagement gestion = grapheDataManagement.Instance;

            gestion.change(listPointsX, listPointsY, listPointsZ, listPointsD,listPointsR,listPointsW);
            
        
        }

        private void listBoxRecherche_SelectedIndexChanged(object sender, EventArgs e)
        {

            int index =  listBoxRecherche.SelectedIndex;
            labelTraitementPrenom.Text = listRecherche.ElementAt(index).ElementAt(1);
            labelTraitementNom.Text = listRecherche.ElementAt(index).ElementAt(2);
        }

        private void listBoxResearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBoxResearch.SelectedIndex;

            Console.WriteLine(listRecherche.Count);

            if(index>=0)
            {
                //changementGraph(listRecherche.ElementAt(index).ElementAt(0));
                idPatientCourant = int.Parse(listRecherche.ElementAt(index).ElementAt(0));
               
                panel1.Refresh();
                panel2.Refresh();
                panel3.Refresh();
                panel4.Refresh();
                panel5.Refresh();
                dessinX.Refresh();
            }
            
        }

        private void buttonTraitementAdd_Click(object sender, EventArgs e)
        {
            int index = listBoxRecherche.SelectedIndex;
            string idPatient = listRecherche.ElementAt(index).ElementAt(0);

            string sql = "select * from Traitements where ID_Patient = \'" + idPatient + "\' ;";           

            SQLiteCommand command = new SQLiteCommand(sql, con);
            Console.WriteLine(sql);
            SQLiteDataReader reader = command.ExecuteReader();

            int NbSeance = 0;
            while (reader.Read())
            {
                NbSeance = int.Parse("" + reader["Nb_Seance"]);
            }
            NbSeance++;

            Console.WriteLine(NbSeance);

            var timeSpanD = (dateTimeTraitementDate.Value - new DateTime(1970, 1, 1, 0, 0, 0));
            long date = (long)timeSpanD.TotalSeconds;


            string sql2 = "insert into Traitements (Date,Fraction,X_Auto,Y_Auto,Z_Auto,X_Manu,Y_Manu,Z_Manu,Roll_Auto,Roll_Manu,PTV1,PTV2,PTV3,PTV4,Poids,Medecin,Appel_Medecin,Nb_Seance,ID_Patient) values (\'" + date + "\', \'" + numericTraitementFraction.Value + "\', \'" + numericTraitementXAuto.Value + "\', \'" + numericTraitementYAuto.Value + "\', \'" + numericTraitementZAuto.Value + "\', \'" + numericTraitementXManu.Value + "\', \'" + numericTraitementYManu.Value + "\', \'" + numericTraitementZManu.Value + "\', \'" + numericTraitementRollAuto.Value + "\', \'" + numericTraitementRollManu.Value + "\', \'" + numericTraitementPTV1.Value + "\', \'" + numericTraitementPTV2.Value + "\', \'" + numericTraitementPTV3.Value + "\', \'" + numericTraitementPTV4.Value + "\', \'" + numericTraitementPoids.Value + "\', \'" + textBoxTraitementMedecin.Text + "\', \'" + textBoxTraitementAppel.Text + "\', \'" + NbSeance + "\', \'" + idPatient + "\')";

            SQLiteCommand command2 = new SQLiteCommand(sql2, con);
            command2.ExecuteNonQuery();
            MessageBox.Show("Traitement ajouté");
        }

        private void btnResearch_Click(object sender, EventArgs e)
        {
            listBoxResearch.Items.Clear();
            listRecherche.Clear();

            string critere = "";
            critere = "Prenom";

            string sql = "select * from Patients where " + critere + " = '" + textBoxResearch.Text + "' ;";

            SQLiteCommand command = new SQLiteCommand(sql, con);
            Console.WriteLine(sql);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                List<string> listTemp = new List<string>();
                listBoxResearch.Items.Add(" Prenom : " + reader["Prenom"] + "\n Nom : " + reader["Nom"]);
                listTemp.Add("" + reader["ID"]);
                listTemp.Add("" + reader["Prenom"]);
                listTemp.Add("" + reader["Nom"]);
                listRecherche.Add(listTemp);
                idPatientCourant = int.Parse("" + reader["ID"]);
            }
            
        }


        private void buttonSwitch_Click(object sender, EventArgs e)
        {
            if(testSwitch)
            {
                grapheDataManagement.Instance.general();
                testSwitch = false;
                buttonSwitch.Text = "Individuel";
            }
            else
            {
                changementGraph(""+idPatientCourant);
                testSwitch = true;
                buttonSwitch.Text = "Général";
            }
            refreshAll();
        }

        private void ratioEcartTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grapheDataManagement.Instance.changeRatio();
            refreshAll();
        }

        private void refreshAll()
        {
            panel1.Refresh();
            panel2.Refresh();
            panel3.Refresh();
            panel4.Refresh();
            panel5.Refresh();
            dessinX.Refresh();
        }

        private void générerDesDonnéesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BDDTest t = new BDDTest();
            t.generatePatient_Traitement(100, 35);
        }

        private void afficherLesDonnéesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            initListPoints(bddTest);
            refreshAll();
        }

        private void remettreLesBonnesDonnéesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            initListPoints(bdd);
            refreshAll();
        }

        private void buttonStatsMoinsEcartB_Click(object sender, EventArgs e)
        {
            b -= 0.05;
            labelValueB.Text = ""+b;
            grapheDataManagement.Instance.changeBC(b, c);
            refreshAll();
        }

        private void buttonStatsPlusEcartB_Click(object sender, EventArgs e)
        {
            b += 0.05;
            labelValueB.Text = "" + b;
            grapheDataManagement.Instance.changeBC(b, c);
            refreshAll();
        }

        private void buttonStatsMoinsEcartC_Click(object sender, EventArgs e)
        {
            c -= 0.05;
            labelValueC.Text = "" + c;
            grapheDataManagement.Instance.changeBC(b, c);
            refreshAll();
        }

        private void buttonStatsPlusEcartC_Click(object sender, EventArgs e)
        {
            c += 0.05;
            labelValueC.Text = "" + c;
            grapheDataManagement.Instance.changeBC(b, c);
            refreshAll();
        }

        private void buttonStatsEcart_Click(object sender, EventArgs e)
        {
            grapheDataManagement.Instance.changeEcart();
            grapheDataManagement.Instance.changeBC(b,c);
            refreshAll();
        }

        private void regressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grapheDataManagement.Instance.regression();
        }

    }
}
