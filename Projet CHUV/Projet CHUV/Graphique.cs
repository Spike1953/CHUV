using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Projet_CHUV
{
    class Graphique
    {

        private int sizePoint;
        private int longueur;
        private int largeur;
        private Point origine;
        private List<Point> listAffichage;
        private Boolean withEcartType;
        private List<List<double>> listDoubleN;
        private List<double> listN;
        private List<double> listEcart;
        private double largeurEspacement;
        private double scaleHauteur;
        private double scaleEcartType;


        public Graphique(List<List<double>> list)
        {
            listDoubleN = list;
            initValue();
        }

        public Graphique(List<double> list)
        {
            listN = list;
            initValue();
        }

        private void initValue()
        {
            sizePoint = 10;
            longueur = 600;
            largeur = 370;
            origine = new Point(10, 10);
            withEcartType = true;
            listAffichage = new List<Point>();
            listEcart = new List<double>();
            largeurEspacement = 15;
            scaleHauteur = 100;
            scaleEcartType = 25;
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
            if(listDoubleN.Count>0)
            {
                for (int i = 0; i < listDoubleN.Count; i++)
                {
                    double ecart = ecartType(listDoubleN.ElementAt(i));
                    listEcart.Add(ecart);

                    double somme = 0;
                    for (int j = 0; j < listDoubleN.ElementAt(i).Count; j++)
                    {
                        somme += listDoubleN.ElementAt(i).ElementAt(j);
                    }
                    if (listDoubleN.ElementAt(i).Count != 0)
                    {
                        double average = somme / listDoubleN.ElementAt(i).Count;
                        listAffichage.Add(new Point((int)((i + 1) * largeurEspacement), (int)(average * scaleHauteur)));
                    }
                }
            }
            draw(e);
            listEcart.Clear();
            listAffichage.Clear();

        }

        private void draw(PaintEventArgs e)
        {
            /*int taille = 10;
            int longueur = 370;
            int largeur = 350;
            Point origine = new Point(10, 10);*/

            Graphics g = e.Graphics;
            g.TranslateTransform(10, largeur - origine.Y);
            g.ScaleTransform(1, -1);

            Brush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(Color.Black);

            //Dessin du repère
            g.DrawLine(pen, new Point(origine.X, 2), new Point(origine.X, largeur));
            g.DrawLine(pen, new Point(2, origine.Y), new Point(longueur, origine.Y));

            for (int i = 0; i < listAffichage.Count; i++)
            {
                Point p = listAffichage.ElementAt(i);
                if(p.Y!=0)
                {
                    g.FillEllipse(brush, new Rectangle(p.X, p.Y, sizePoint, sizePoint));
                    if (withEcartType)
                    {
                        g.DrawLine(pen, new Point(p.X + (int)(sizePoint / 2), p.Y + (int)(sizePoint / 2)), new Point(p.X + (int)(sizePoint / 2), p.Y + (int)(sizePoint / 2) + (int)listEcart.ElementAt(i)));
                        g.DrawLine(pen, new Point(p.X + (int)(sizePoint / 2), p.Y + (int)(sizePoint / 2)), new Point(p.X + (int)(sizePoint / 2), p.Y + (int)(sizePoint / 2) - (int)listEcart.ElementAt(i)));
                        g.DrawLine(pen, new Point(p.X, p.Y + (int)(sizePoint / 2) - (int)listEcart.ElementAt(i)), new Point(p.X + sizePoint, p.Y + (int)(sizePoint / 2) - (int)listEcart.ElementAt(i)));
                        g.DrawLine(pen, new Point(p.X, p.Y + (int)(sizePoint / 2) + (int)listEcart.ElementAt(i)), new Point(p.X + sizePoint, p.Y + (int)(sizePoint / 2) + (int)listEcart.ElementAt(i)));
                    }
                }
           }
        }

        private double ecartType(List<double> list)
        {
            double somme = 0;
            for (int i = 0; i < list.Count; i++)
            {
                somme += list.ElementAt(i);
            }
            double moyenne = somme / list.Count;

            somme = 0;

            for (int i = 0; i < list.Count; i++)
            {
                somme += (list.ElementAt(i) - moyenne) * (list.ElementAt(i) - moyenne);
            }
            double result = -1;
            if (somme != 0 && list.Count != 0)
                result = Math.Sqrt(somme / list.Count);


            return result * scaleEcartType;
        }

      
    }
}
