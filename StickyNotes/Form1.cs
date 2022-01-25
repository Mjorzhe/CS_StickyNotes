using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StickyNotes
{
    public partial class Form1 : Form
    {
        int _buttonNoteHeight;
        int _panelHeight;
        bool _isPanelHidden;
        public Form1()
        {
            
            InitializeComponent();
            _panelHeight = panel1.Size.Height;
            _isPanelHidden = true;
            _buttonNoteHeight = 54;

        }
        
        int _counter = -1;
        

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void buttonFechar_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(1000);
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
            
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        public void removeButton(int _buttonNumber)
        {

        }
        private void addButton(int _qtt)
        {
            Button button = new Button();
            button.Name = "Note " + (_counter + 1);
            button.Text = "Note " + (_counter + 1);
            button.Size = new Size(192, 49);
            button.FlatStyle = FlatStyle.Flat;
            button.ForeColor = Color.FromArgb(50, 121, 143);
            button.Location = new Point(9, (_buttonNoteHeight) * _counter);
            Note note = new Note(_counter);
            note.Show();
            note.ShowInTaskbar = false;
            button.Click += new EventHandler(button_MouseClick);
            button.DoubleClick += new EventHandler(button_MouseDoubleClick);

            void button_MouseClick(object sender, EventArgs e)
            {
                note.Show();
                note.Focus();
            }

            void button_MouseDoubleClick(object sender, EventArgs e)
            {

            }
            panelButtonNotes.Controls.Add(button);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            this._counter++;
            timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Height >= _panelHeight + panelButtonNotes.Height + 10)
            {
                this.Height += 1;
            } else if (this.Height < _panelHeight + 5) {
                this.Height += 1;
            } else
            {
                this.Height += 5;
            }
            if (this.Height >= _panelHeight + _buttonNoteHeight)
            {
                _panelHeight = this.Height;
                panelButtonNotes.Height += _buttonNoteHeight;
                addButton(_counter);
                timer1.Stop();
                this.Refresh();
            }


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonNote_Click(object sender, EventArgs e)
        {

        }
    }
}
