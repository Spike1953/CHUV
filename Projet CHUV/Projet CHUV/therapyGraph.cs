using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Projet_CHUV
{
    class therapyGraph
    {
        private int sizePoint;
        private int longueur;
        private int largeur;
        private Point origine;
        private List<Point> listAffichage;
        private List<Point> listCourbeBleu;
        private Boolean withEcartType;
        private Boolean withCourbeBleu;
        private List<List<double>> listDoubleN;
        private List<double> listN;
        private List<double> listEcart;
        private double largeurEspacement;
        private double largeurX;
        private double scaleHauteur;
        private double scaleEcartType;
        private string name;
        private Boolean testRatio;
        private bool testEcart;
        private double b, c;
        private bool testRegression;
        private Point pointDepartRegression, pointArriveRegression;
        private List<Point> listAffichageSave;

        public therapyGraph(List<List<double>> list,string nom)
        {
            name = nom;
            listDoubleN = list;
            initValue();
        }

        private void initValue()
        {
            pointArriveRegression = new Point(0, 0);
            pointDepartRegression = new Point(0, 0);

            testRatio = true;
            testEcart = true;
            testRegression = false;
            sizePoint = 10;
            longueur = 600;
            largeur = 370;
            origine = new Point(10, 10);
            withEcartType = true;
            listAffichage = new List<Point>();
            listAffichageSave = new List<Point>();
            listCourbeBleu = new List<Point>();
            listEcart = new List<double>();
            largeurEspacement = 15;
            largeurX = 50;
            scaleHauteur = 75;
            scaleEcartType = 25;
            withCourbeBleu = true;
            listCourbeBleu.Add(origine);
            for (int i = 0; i < listDoubleN.Count; i++)
            {
                listCourbeBleu.Add(new Point((int)(i * 50/5)+origine.X, origine.Y+listDoubleN.ElementAt(i).Count));
                //Console.WriteLine(listDoubleN.ElementAt(i).Count);
            }

        }

        public void EnabledEcartType(Boolean b)
        {
            withEcartType = b;
        }

        public void setScaleHeight(double hauteur)
        {
            scaleHauteur = hauteur;
        }

        public void setScaleWidth(double largeur)
        {
            largeurEspacement = largeur;
        }

        public void setScaleEcartType(double e)
        {
            scaleEcartType = e;
        }

        public void drawGraph(PaintEventArgs e)
        {
           if (listDoubleN.Count > 0)
            {
                for (int i = 0; i < listDoubleN.Count; i++)
                {
                    double ecart = ecartType(listDoubleN.ElementAt(i),i);
                    listEcart.Add(ecart);

                    double somme = 0;
                    for (int j = 0; j < listDoubleN.ElementAt(i).Count; j++)
                    {
                        somme += listDoubleN.ElementAt(i).ElementAt(j);
                    }
                    if (listDoubleN.ElementAt(i).Count != 0)
                    {
                        double average = somme / listDoubleN.ElementAt(i).Count;
                        listAffichage.Add(new Point((int)((i) * largeurEspacement), (int)(average * scaleHauteur)));
                    }
                }
            }
            List<Point> newList = new List<Point>();
            float addition = 0.0f;
            int x = 50;
            int compteurjour = 0;
            for (int i = 0; i < listAffichage.Count; i++)
            {
                addition += listAffichage.ElementAt(i).Y;
                compteurjour++;
                if(compteurjour==5)
                {
                    newList.Add(new Point(x, (int)(addition / 5.0f)));
                    addition = 0.0f;
                    x += 50;
                    compteurjour = 0;
                }
            }

            listAffichage = newList;
            regression();
            draw(e);
            listEcart.Clear();
            listAffichage.Clear();
             
        }

        private void draw(PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            g.TranslateTransform(10, largeur - origine.Y);
            g.ScaleTransform(1, -1);

            Brush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(Color.Black);

            //Dessin du repère
            g.DrawLine(pen, new Point(origine.X, 2), new Point(origine.X, largeur));
            g.DrawLine(pen, new Point(2, origine.Y), new Point(longueur, origine.Y));

           
            double[] myX = new double[7] {1.244,1.265,1.29,1.366,1.415,1.385,1.418};
            double[] myY = new double[7] { 0.659, 0.722, 0.741, 0.737, 0.788, 0.81, 0.929 };
            double[] myZ = new double[7] {1.61,1.642,1.806,1.864,1.936,1.961,1.957 };


            brush = new SolidBrush(Color.Black);

            //Dessin du trait en (0,1)
            g.DrawLine(pen, new Point(0,(int)(1*scaleHauteur)), new Point(20,(int)(1*scaleHauteur)));

            //g.DrawLine(pen, new Point(0, (int)(listCourbeBleu.ElementAt(1).Y)), new Point(20, (int)(listCourbeBleu.ElementAt(1).Y )));
            
            //Dessin des valeurs
            Font drawFont = new Font("Arial", 16);
            g.DrawString("0", drawFont, brush, new Point(7, 7));
            g.ScaleTransform(1, -1);
            g.DrawString("1", drawFont, brush, new Point(7, -(int)(1.2*scaleHauteur)));

            Color colorB = Color.Gray;// Color.FromArgb(50, 220, 220, 255);
            brush = new SolidBrush(colorB);
            Font drawFontB = new Font("Arial", 13);
            g.DrawString("" + (listCourbeBleu.ElementAt(1).Y-origine.Y), drawFontB, brush, new Point(-15, -(int)(listCourbeBleu.ElementAt(1).Y)));

            g.ScaleTransform(1, -1);
            
            for (int i = 0; i < listAffichage.Count; i++)
            {
                Point p = listAffichage.ElementAt(i);
                if (p.Y != 0)
                {
                    brush = new SolidBrush(Color.Black);
                    g.FillEllipse(brush, new Rectangle(p.X, p.Y, sizePoint, sizePoint));
                    
                    if (withEcartType)
                    {
                        g.DrawLine(pen, new Point(p.X + (int)(sizePoint / 2), p.Y + (int)(sizePoint / 2)), new Point(p.X + (int)(sizePoint / 2), p.Y + (int)(sizePoint / 2) + (int)listEcart.ElementAt(i)));
                        g.DrawLine(pen, new Point(p.X + (int)(sizePoint / 2), p.Y + (int)(sizePoint / 2)), new Point(p.X + (int)(sizePoint / 2), p.Y + (int)(sizePoint / 2) - (int)listEcart.ElementAt(i)));
                        g.DrawLine(pen, new Point(p.X, p.Y + (int)(sizePoint / 2) - (int)listEcart.ElementAt(i)), new Point(p.X + sizePoint, p.Y + (int)(sizePoint / 2) - (int)listEcart.ElementAt(i)));
                        g.DrawLine(pen, new Point(p.X, p.Y + (int)(sizePoint / 2) + (int)listEcart.ElementAt(i)), new Point(p.X + sizePoint, p.Y + (int)(sizePoint / 2) + (int)listEcart.ElementAt(i)));
                    }

                    brush = new SolidBrush(Color.Red);
                    if(name == "X")
                        g.FillRectangle(brush, new Rectangle(p.X, (int)(myX[i]*scaleHauteur), sizePoint, sizePoint));
                    else if(name == "Y")
                        g.FillRectangle(brush, new Rectangle(p.X, (int)(myY[i] * scaleHauteur), sizePoint, sizePoint));
                    else if(name == "Z")
                        g.FillRectangle(brush, new Rectangle(p.X, (int)(myZ[i] * scaleHauteur), sizePoint, sizePoint));

                }
            }
            if (withCourbeBleu)
            {
                //Color color = Color.FromArgb(50, 220, 220, 255);
                Color color = Color.FromArgb(50, 100, 100, 255);

                Brush brushP = new SolidBrush(color);
                g.FillPolygon(brushP, listCourbeBleu.ToArray());
            }
            if(testRegression)
            {
                pen = new Pen(Color.Green);
                g.DrawLine(pen,pointDepartRegression, pointArriveRegression);
            }
            
        }

        public void changeEcartType()
        {
            if(testEcart)
            {
                testEcart = false;
            }
            else
            {
                testEcart = true;
            }
        }

        public void setBC(double b, double c)
        {
            this.b = b;
            this.c = c;
        }

        private double ecartType(List<double> list, int nbSemaine)
        {
            
            double somme = 0.0;
            for (int i = 0; i < list.Count; i++)
            {
                somme += list.ElementAt(i);
            }
            double moyenne = somme / list.Count;
            if (testEcart)
            {
                somme = 0.0;

                for (int i = 0; i < list.Count; i++)
                {
                    somme += (list.ElementAt(i) - moyenne) * (list.ElementAt(i) - moyenne);
                }
                double result = -1;
                if (list.Count != 0)
                    result = Math.Sqrt(somme / list.Count);

                if (testRatio)
                    return result / (8 - nbSemaine) * scaleHauteur;
                else
                    return result * scaleEcartType;
            }
            else
            {
                double epsilon = 0.0001;
                return 0.05 + b * nbSemaine + c * moyenne + epsilon;
            }
            
        }

        public void changeTestRatio()
        {
            if (testRatio)
                testRatio = false;
            else
                testRatio = true;
        }

        public void regression()
        {
            //y = a*x +b
            //a = cov(x,y) / s^2x
            //b = yMoy - a*xMmoy
            if(listAffichage.Count!=0)
            {
                double sommeX = 0;
                double sommeY = 0;

                for (int i = 0; i < listAffichage.Count; i++)
                {
                    sommeX += listAffichage.ElementAt(i).X;
                    sommeY += listAffichage.ElementAt(i).Y;
                }

                double xMoy = sommeX / listAffichage.Count;
                double yMoy = sommeY / listAffichage.Count;

                double covXY = 0;
                double s2x = 0;
                for (int i = 0; i < listAffichage.Count; i++)
                {
                    covXY += (listAffichage.ElementAt(i).X - xMoy) * (listAffichage.ElementAt(i).Y - yMoy);
                    s2x += (listAffichage.ElementAt(i).X - xMoy) * (listAffichage.ElementAt(i).X - xMoy);
                }
              
                covXY = covXY / listAffichage.Count;
                s2x = s2x / listAffichage.Count;

                double a = covXY / s2x;
                double b = yMoy - a * xMoy;

                testRegression = true;
                int x = (int)(origine.X + largeurX);

                pointDepartRegression = new Point(x, (int)b + origine.Y);
                x = (int)(7 * largeurX + origine.X);
                pointArriveRegression = new Point(x, (int)(a * x + b + origine.Y));
            }
        }
    }
}
