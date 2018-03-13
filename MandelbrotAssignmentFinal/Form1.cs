using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MandelbrotAssignmentFinal
{
    public partial class Fractal : Form
    {
        public Fractal()
        {
            InitializeComponent();
        }


        private readonly int MAX = 256;      // max iterations
        private readonly double SX = -2.025; // start value real
        private readonly double SY = -1.125; // start value imaginary
        private readonly double EX = 0.6;    // end value real
        private readonly double EY = 1.125;  // end value imaginary
        private static int x1, y1, xs, ys, xe, ye;
        private static double xstart, ystart, xende, yende, xzoom, yzoom;
        private static bool action, rectangle, finished;
        private static float xy;
        private Image picture;
        private Graphics g1;
        private Cursor c1, c2;
        private HSB HSBcol = new HSB();



        public void init() // all instances will be prepared
        {
            //HSBcol = new HSB();
            
            finished = false;
            c1 = Cursors.WaitCursor;
            c2 = Cursors.Cross;
            x1 = Width;
            y1 = Height;
            xy = (float)x1 / (float)y1;
            picture = pictureBox1.Image;
            g1 = pictureBox1.CreateGraphics();
            finished = true;
        }

        public void destroy() // delete all instances 
        {
            if (finished)
            {
                
                picture = null;
                g1 = null;
                c1 = null;
                c2 = null;
                System.GC.Collect(); // garbage collection
            }
        }

        public void start()
        {
            action = false;
            rectangle = false;
            initvalues();
            xzoom = (xende - xstart) / (double)x1;
            yzoom = (yende - ystart) / (double)y1;
            mandelbrot();
        }

        public void stop()
        {
        }

        public void paint(Graphics g)
        {
            update(g);
        }

        private void propertiesToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        public void update(Graphics g)
        {
            g.DrawImage(picture, 0, 0);
            if (rectangle)
            {
                //g.setColor(Color.white);
                if (xs < xe)
                {
                    if (ys < ye) g.DrawRectangle(new Pen(Color.White),xs, ys, (xe - xs), (ye - ys));
                    else g.DrawRectangle(new Pen(Color.White), xs, ye, (xe - xs), (ys - ye));
                }
                else
                {
                    if (ys < ye) g.DrawRectangle(new Pen(Color.White), xe, ys, (xs - xe), (ye - ys));
                    else g.DrawRectangle(new Pen (Color.White), xe, ye, (xs - xe), (ys - ye));
                }
            }
        }

        private void mandelbrot() // calculate all points
        {
            int x, y;
            float h, b, alt = 0.0f;

            action = false;
            pictureBox1.Cursor = c1;
            //showStatus("Mandelbrot-Set will be produced - please wait...");
            for (x = 0; x < x1; x += 2)
                for (y = 0; y < y1; y++)
                {
                    h = pointcolour(xstart + xzoom * (double)x, ystart + yzoom * (double)y); // color value
                    if (h != alt)
                    {
                        b = 1.0f - h * h; // brightnes
                                          ///djm added
                                          HSBcol.fromHSB(h,0.8f,b); //convert hsb to rgb then make a Java Color
                                          //Color col = new Color(0,HSBcol.rChan,HSBcol.gChan,HSBcol.bChan);
                                          //g1.setColor(col);
                        //djm end
                        //djm added to convert to RGB from HSB

                       // g1.setColor(Color.getHSBColor(h, 0.8f, b));
                        //djm test
                       // Color col = Color.getHSBColor(h, 0.8f, b);
                       // int red = col.getRed();
                       // int green = col.getGreen();
                        //int blue = col.getBlue();
                        //djm 
                        //alt = h;
                    }
                    g1.DrawLine(new Pen(Color.White), x, y, x + 1, y);
                }
            //showStatus("Mandelbrot-Set ready - please select zoom area with pressed mouse.");
            this.pictureBox1.Cursor = c2;
            action = true;
        }

        private float pointcolour(double xwert, double ywert) // color value from 0.0 to 1.0 by iterations
        {
            double r = 0.0, i = 0.0, m = 0.0;
            int j = 0;

            while ((j < MAX) && (m < 4.0))
            {
                j++;
                m = r * r - i * i;
                i = 2.0 * r * i + ywert;
                r = m + xwert;
            }
            return (float)j / (float)MAX;
        }

        private void initvalues() // reset start values
        {
            xstart = SX;
            ystart = SY;
            xende = EX;
            yende = EY;
            if ((float)((xende - xstart) / (yende - ystart)) != xy)
                xstart = xende - (yende - ystart) * (double)xy;
        }

        //public void mousePressed(object sender, EventArgs e)
        //{
        //    e.GC.consume();
        //    if (action)
        //    {
        //        xs = e.getX();
        //        ys = e.getY();
        //    }
        //}

        //public void mouseReleased(object sender, EventArgs e)
        //{
        //    int z, w;

        //    e.consume();
        //    if (action)
        //    {
        //        xe = e.getX();
        //        ye = e.getY();
        //        if (xs > xe)
        //        {
        //            z = xs;
        //            xs = xe;
        //            xe = z;
        //        }
        //        if (ys > ye)
        //        {
        //            z = ys;
        //            ys = ye;
        //            ye = z;
        //        }
        //        w = (xe - xs);
        //        z = (ye - ys);
        //        if ((w < 2) && (z < 2)) initvalues();
        //        else
        //        {
        //            if (((float)w > (float)z * xy)) ye = (int)((float)ys + (float)w / xy);
        //            else xe = (int)((float)xs + (float)z * xy);
        //            xende = xstart + xzoom * (double)xe;
        //            yende = ystart + yzoom * (double)ye;
        //            xstart += xzoom * (double)xs;
        //            ystart += yzoom * (double)ys;
        //        }
        //        xzoom = (xende - xstart) / (double)x1;
        //        yzoom = (yende - ystart) / (double)y1;
        //        mandelbrot();
        //        rectangle = false;
        //        repaint();
        //    }
        //}

        public void mouseEntered(object sender, EventArgs e)
        {
        }

        public void mouseExited(object sender, EventArgs e)
        {
        }

        public void mouseClicked(object sender, EventArgs e)
        {
        }

        //public void mouseDragged(object sender, EventArgs e)
        //{
        //    e.consume();
        //    if (action)
        //    {
        //        xe = e.getX();
        //        ye = e.getY();
        //        rectangle = true;
        //        repaint();
        //    }
        //}

        public void mouseMoved(object sender, EventArgs e)
        {
        }

        public String getAppletInfo()
        {
            return "fractal.class - Mandelbrot Set a Java Applet by Eckhard Roessel 2000-2001";
        }












        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do You want to quit ?", "Quit", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }
            else if (dialogResult == DialogResult.No)
            {
                
            }
        }
    }
}
