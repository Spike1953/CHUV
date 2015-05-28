using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projet_CHUV
{
    class grapheDataManagement
    {
        private therapyGraph graphX;
        private therapyGraph graphY;
        private therapyGraph graphZ;
        private therapyGraph graphD;
        private therapyGraph graphR;
        private therapyGraph graphW;

        private List<List<double>> listX;
        private List<List<double>> listY;
        private List<List<double>> listZ;
        private List<List<double>> listD;
        private List<List<double>> listR;
        private List<List<double>> listW;



        private static grapheDataManagement instance;
        public static grapheDataManagement Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new grapheDataManagement();
                }
                return instance;
            }
        }

        private grapheDataManagement()
        {
            //TRaitement
        }

        public void initialise(List<List<double>> listX, List<List<double>> listY, List<List<double>> listZ, List<List<double>> listD, List<List<double>> listR, List<List<double>> listW)
        {
            graphX = new therapyGraph(listX, "X");
            graphY = new therapyGraph(listY, "Y");
            graphZ = new therapyGraph(listZ, "Z");
            graphD = new therapyGraph(listD, "D");
            graphR = new therapyGraph(listR, "R");
            graphW = new therapyGraph(listW, "W");

            this.listX = listX;
            this.listY = listY;
            this.listZ = listZ;
            this.listD = listD;
            this.listR = listR;
            this.listW = listW;
        }

        public void change(List<List<double>> listX, List<List<double>> listY, List<List<double>> listZ, List<List<double>> listD, List<List<double>> listR, List<List<double>> listW)
        {
            graphX = new therapyGraph(listX,"X");
            graphY = new therapyGraph(listY,"Y");
            graphZ = new therapyGraph(listZ,"Z");
            graphD = new therapyGraph(listD,"D");
            graphR = new therapyGraph(listR,"R");
            graphW = new therapyGraph(listW,"W");
        }

        public void drawGraphX(PaintEventArgs e)
        {
            graphX.drawGraph(e);
        }

        public void drawGraphY(PaintEventArgs e)
        {
            graphY.drawGraph(e);
        }
        public void drawGraphZ(PaintEventArgs e)
        {
            graphZ.drawGraph(e);
        }

        public void drawGraphD(PaintEventArgs e)
        {
            graphD.drawGraph(e);
        }

        public void drawGraphR(PaintEventArgs e)
        {
            graphR.drawGraph(e);
        }

        public void drawGraphW(PaintEventArgs e)
        {
            graphW.drawGraph(e);
        }

        public void general()
        {
            graphX = new therapyGraph(listX, "X");
            graphY = new therapyGraph(listY, "Y");
            graphZ = new therapyGraph(listZ, "Z");
            graphD = new therapyGraph(listD, "D");
            graphR = new therapyGraph(listR, "R");
            graphW = new therapyGraph(listW, "W");
        }

        public void changeRatio()
        {
            graphX.changeTestRatio();
            graphY.changeTestRatio();
            graphZ.changeTestRatio();
            graphD.changeTestRatio();
            graphR.changeTestRatio();
            graphW.changeTestRatio();
        }

        public void changeEcart()
        {
            graphX.changeEcartType();
            graphY.changeEcartType();
            graphZ.changeEcartType();
            graphD.changeEcartType();
            graphR.changeEcartType();
            graphW.changeEcartType();
        }

        public void changeBC(double b, double c)
        {
            graphX.setBC(b, c);
            graphY.setBC(b, c);
            graphZ.setBC(b, c);
            graphD.setBC(b, c);
            graphR.setBC(b, c);
            graphW.setBC(b, c);
        }

        public void regression()
        {
            graphX.regression();
            graphY.regression();
            graphZ.regression();
            graphD.regression();
            graphR.regression();
            graphW.regression();
        }

    }

}
