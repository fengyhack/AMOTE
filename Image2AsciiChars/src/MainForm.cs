using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

namespace Image2AsciiChars
{
    public partial class MainForm : Form
    {
        private Bitmap bmp = null;
        private int ncs = 1;

        public MainForm()
        {
            InitializeComponent();
            textBox_pixelUnits.Text = "1";
            trackBar1.Minimum = 1;
            trackBar1.Maximum = 1;
        }

         private void btn_OpenImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "JPG|*.jpg|PNG|*.png|BMP|*.bmp|All|*.*";

            if(ofd.ShowDialog(this)==DialogResult.OK)
            {
                if(bmp!=null)
                {
                    bmp.Dispose();
                    trackBar1.Maximum = 1;
                    textBox_pixelUnits.Text = "1";
                }

                var img = new Bitmap(ofd.FileName);
                bmp = new Bitmap(img);
                img.Dispose();

                int w = bmp.Width;
                int h = bmp.Height;
                if (w < 32 || h < 32)
                {
                    MessageBox.Show("图片尺寸太小(建议32*32以上)");
                    return;
                }

                trackBar1.Maximum = w < h ? w / 16 : h / 16;
                pictureBox1.Image = bmp;
                pictureBox1.Refresh();
            }
        }

        private void btn_SaveText_Click(object sender, EventArgs e)
        {
            if(bmp==null)
            {
                MessageBox.Show("请先打开一张图片");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "TXT|*.txt";
            if(sfd.ShowDialog(this)==DialogResult.OK)
            {
                SaveAsString(ref bmp, sfd.FileName);
            }
        }

        private void SaveAsString(ref Bitmap bmp, string txtName)
        {
            StreamWriter sw = new StreamWriter(txtName);

            int w = bmp.Width;
            int h = bmp.Height;

            int ncw, nch;
            if(w>h)
            {
                ncw = ncs * w / h;
                nch = ncs;
            }
            else
            {
                ncw = ncs;
                nch = ncs * h / w; 
            }

            for (int i = 0; i < h; i += nch)
            {
                for (int j = 0; j < w; j += ncw)
                {
                    sw.Write(GetPixelVChar(bmp.GetPixel(j,i)));
                }
                sw.WriteLine();
            }

            sw.Close();
        }

        private char GetPixelVChar(Color cr)
        {
            int b = (cr.R * 30 + cr.G * 59 + cr.B * 11 + 50) / 100;

            int f = 255 / 8;

            if (b < f)
            {
                return '#';
            }
            else if (b < 2*f)
            {
                return '&';
            }
            else if (b < 3*f)
            {
                return '$';
            }
            else if (b < 4*f)
            {
                return '*';
            }
            else if (b < 5*f)
            {
                return 'o';
            }
            else if (b < 6*f)
            {
                return '!';
            }
            else if (b < 7*f)
            {
                return ';';
            }
            else
            {
                return ' ';
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            ncs = trackBar1.Value;
            textBox_pixelUnits.Text = ncs.ToString();
        }

        private void btn_Preview_Click(object sender, EventArgs e)
        {

        }
    }
}
