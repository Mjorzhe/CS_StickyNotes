using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace StickyNotes
{

    public partial class Note : Form
    {
        public Note(int _counter)
        {
            InitializeComponent();
            this._counter = _counter;
            textArea.BackColor = getLightColour(_counter);
            panel1.BackColor = getLightColour(_counter);
        }
        int _counter;
        String[] colour = { "255;230;105|237;177;74"   //Yellow - Dark Yellow
                           , "162;214;242|75;162;189"   //Cyan - Dark Cyan
                           , "177;232;132|131;191;82"   //Green - Dark Green
                           , "247;166;227|207;103;170"  //Pink - Dark Pink
                           , "235;170;66|176;123;39"};  //Orange - Dark Orange
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        //int windowSizeX = 500, windowSizeY = 500;
        
        
        public System.Drawing.Color getLightColour(int _index)
        {
            return Color.FromArgb(Int32.Parse((colour[_index].Split('|')[0]).Split(';')[0]), Int32.Parse((colour[_index].Split('|')[0]).Split(';')[1]), Int32.Parse((colour[_index].Split('|')[0]).Split(';')[2]));
        }

        public System.Drawing.Color getDarkColour(int _index)
        {
            return Color.FromArgb(Int32.Parse((colour[_index].Split('|')[1]).Split(';')[0]), Int32.Parse((colour[_index].Split('|')[1]).Split(';')[1]), Int32.Parse((colour[_index].Split('|')[1]).Split(';')[2]));
        }


        private void Note_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //System.Drawing.Color _lightColour = getLightColour(_counter);
            //panel1.BackColor = _lightColour;
        }

        private void buttonFechar_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonMinimize.Location = new Point(424, 0);
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Color _darkColour = getDarkColour(_counter);
            panel2.BackColor = _darkColour;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void a(object sender, EventArgs e)
        {
            
        }

        private void checkBoxTop_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = true;
        }
    }
}
