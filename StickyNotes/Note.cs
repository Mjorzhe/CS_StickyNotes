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
        int _counter;
        public Note(int _counter)
        {
            InitializeComponent();
            this._counter = _counter;
            _counter = _counter % colour.Length;
            textArea.BackColor = getLightColour(_counter);
            panel2.BackColor = getDarkColour(_counter);
            this.BackColor = getLightColour(_counter);
            textArea.ForeColor = getDarkColour(_counter);
        }

        private Size formSize;
        //Overridden methods
        protected override void WndProc(ref Message m)
        {
            const int WM_NCCALCSIZE = 0x0083;//Standar Title Bar - Snap Window
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MINIMIZE = 0xF020; //Minimize form (Before)
            const int SC_RESTORE = 0xF120; //Restore form (Before)
            const int WM_NCHITTEST = 0x0084;//Win32, Mouse Input Notification: Determine what part of the window corresponds to a point, allows to resize the form.
            const int resizeAreaSize = 20;
            #region Form Resize
            // Resize/WM_NCHITTEST values
            const int HTCLIENT = 1; //Represents the client area of the window
            const int HTLEFT = 10;  //Left border of a window, allows resize horizontally to the left
            const int HTRIGHT = 11; //Right border of a window, allows resize horizontally to the right
            const int HTTOP = 12;   //Upper-horizontal border of a window, allows resize vertically up
            const int HTTOPLEFT = 13;//Upper-left corner of a window border, allows resize diagonally to the left
            const int HTTOPRIGHT = 14;//Upper-right corner of a window border, allows resize diagonally to the right
            const int HTBOTTOM = 15; //Lower-horizontal border of a window, allows resize vertically down
            const int HTBOTTOMLEFT = 16;//Lower-left corner of a window border, allows resize diagonally to the left
            const int HTBOTTOMRIGHT = 17;//Lower-right corner of a window border, allows resize diagonally to the right
            ///<Doc> More Information: https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-nchittest </Doc>
            if (m.Msg == WM_NCHITTEST)
            { //If the windows m is WM_NCHITTEST
                base.WndProc(ref m);
                if (this.WindowState == FormWindowState.Normal)//Resize the form if it is in normal state
                {
                    if ((int)m.Result == HTCLIENT)//If the result of the m (mouse pointer) is in the client area of the window
                    {
                        Point screenPoint = new Point(m.LParam.ToInt32()); //Gets screen point coordinates(X and Y coordinate of the pointer)                           
                        Point clientPoint = this.PointToClient(screenPoint); //Computes the location of the screen point into client coordinates                          
                        if (clientPoint.Y <= resizeAreaSize)//If the pointer is at the top of the form (within the resize area- X coordinate)
                        {
                            if (clientPoint.X <= resizeAreaSize) //If the pointer is at the coordinate X=0 or less than the resizing area(X=10) in 
                                m.Result = (IntPtr)HTTOPLEFT; //Resize diagonally to the left
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize))//If the pointer is at the coordinate X=11 or less than the width of the form(X=Form.Width-resizeArea)
                                m.Result = (IntPtr)HTTOP; //Resize vertically up
                            else //Resize diagonally to the right
                                m.Result = (IntPtr)HTTOPRIGHT;
                        }
                        else if (clientPoint.Y <= (this.Size.Height - resizeAreaSize)) //If the pointer is inside the form at the Y coordinate(discounting the resize area size)
                        {
                            if (clientPoint.X <= resizeAreaSize)//Resize horizontally to the left
                                m.Result = (IntPtr)HTLEFT;
                            else if (clientPoint.X > (this.Width - resizeAreaSize))//Resize horizontally to the right
                                m.Result = (IntPtr)HTRIGHT;
                        }
                        else
                        {
                            if (clientPoint.X <= resizeAreaSize)//Resize diagonally to the left
                                m.Result = (IntPtr)HTBOTTOMLEFT;
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize)) //Resize vertically down
                                m.Result = (IntPtr)HTBOTTOM;
                            else //Resize diagonally to the right
                                m.Result = (IntPtr)HTBOTTOMRIGHT;
                        }
                    }
                }
                return;
            }
            #endregion
            //Remove border and keep snap window
            if (m.Msg == WM_NCCALCSIZE && m.WParam.ToInt32() == 1)
            {
                return;
            }
            //Keep form size when it is minimized and restored. Since the form is resized because it takes into account the size of the title bar and borders.
            if (m.Msg == WM_SYSCOMMAND)
            {
                /// <see cref="https://docs.microsoft.com/en-us/windows/win32/menurc/wm-syscommand"/>
                /// Quote:
                /// In WM_SYSCOMMAND messages, the four low - order bits of the wParam parameter 
                /// are used internally by the system.To obtain the correct result when testing 
                /// the value of wParam, an application must combine the value 0xFFF0 with the 
                /// wParam value by using the bitwise AND operator.
                int wParam = (m.WParam.ToInt32() & 0xFFF0);
                if (wParam == SC_MINIMIZE)  //Before
                    formSize = this.ClientSize;
                if (wParam == SC_RESTORE)// Restored form(Before)
                    this.Size = formSize;
            }
            base.WndProc(ref m);
        }


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
            //Form1.removeButton(this._counter);
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

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

        private void textArea_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            menuDropDown.Show(buttonDropDown, 0, buttonDropDown.Height);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }
        private void pickColourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ColorDialog colourPicker;
            colourPicker = new ColorDialog();
            colourPicker.ShowDialog();
            Color _primaryColor = colourPicker.Color;
            Color _secondaryColor = ChangeColorBrightness(colourPicker.Color, -0.3f);
            this.BackColor =        _primaryColor;
            textArea.BackColor =    _primaryColor;
            panel2.BackColor =      _secondaryColor;
            textArea.ForeColor =    _secondaryColor;
        }

        private void menuDropDown_Opening(object sender, CancelEventArgs e)
        {

        }

        private void yellowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor =        getLightColour(0);
            textArea.BackColor =    getLightColour(0);
            panel2.BackColor =      getDarkColour(0);
            textArea.ForeColor =    getDarkColour(0);
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor =        getLightColour(1);
            textArea.BackColor =    getLightColour(1);
            panel2.BackColor =      getDarkColour(1);
            textArea.ForeColor =    getDarkColour(1);
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor =        getLightColour(2);
            textArea.BackColor =    getLightColour(2);
            panel2.BackColor =      getDarkColour(2);
            textArea.ForeColor =    getDarkColour(2);
        }
        private void pinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor =        getLightColour(3);
            textArea.BackColor =    getLightColour(3);
            panel2.BackColor =      getDarkColour(3);
            textArea.ForeColor =    getDarkColour(3);
        }
        private void orangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor =        getLightColour(4);
            textArea.BackColor =    getLightColour(4);
            panel2.BackColor =      getDarkColour(4);
            textArea.ForeColor =    getDarkColour(4);
        }
                
    }
}