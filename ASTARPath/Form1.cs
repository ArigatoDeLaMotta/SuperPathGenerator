using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASTARPath
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();

        public void HNDTimer(Object myObject, EventArgs myEventArgs)
        {
            TheMatrix.Run();
            Invalidate();
        }
        public Form1()
        {
            myTimer.Tick += new EventHandler(HNDTimer);
            DoubleBuffered = true;
            // Sets the timer interval to 5 seconds.
            myTimer.Interval = 50;
            myTimer.Start();

            InitializeComponent();
            DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
           
            TheMatrix.Draw(e);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
