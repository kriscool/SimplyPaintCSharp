using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using System.Drawing.Drawing2D;
using AForge;

namespace Paint
{
    public partial class Form1 : Form
    {
        String File;
        SolidBrush color;
        Bitmap DrawArea;
        Graphics g;
        Pen pen;
        Bitmap image;
        Color c;
        int size1 = 10;
        int size2 = 10;
        SolidBrush myBrush;
        public Form1()
        {
            InitializeComponent();
            DrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            g = Graphics.FromImage(DrawArea);
            myBrush = new SolidBrush(Color.Black);
            c = Color.Black;
            pen = new Pen(c, 20);
            textBox2.Text = size1.ToString();
            
        }





        private void Form1_Load(object sender, EventArgs e)
        {

        }
        

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Left)
            {
                g.FillEllipse(pen.Brush, e.X, e.Y, size1, size2);
                pictureBox1.Image = DrawArea;
            
            }
    }

        private void button2_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            File = openFileDialog1.FileName;
                            image = new Bitmap(@File);



                            g.DrawImage(image, 0, 0, pictureBox1.Width, pictureBox1.Height);
                            pictureBox1.Image = DrawArea;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void minus_Click(object sender, EventArgs e)
        {
            size1--;
            size2--;

            textBox2.Text = size1.ToString();
        }

        private void plus_Click(object sender, EventArgs e)
        {
            size1++;
            size2++;
            textBox2.Text = size1.ToString();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
           
            // Keeps the user from selecting a custom color.
            MyDialog.AllowFullOpen = false;
            // Allows the user to get help. (The default is false.)
            MyDialog.ShowHelp = true;
            // Sets the initial color select to the current text color.

            // Update the text box color if the user clicks OK 
            if (MyDialog.ShowDialog() == DialogResult.OK)
                c = MyDialog.Color;
            pen = new Pen(c);
        }

        private void GreyScale_Click(object sender, EventArgs e)
        {
           
            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            DrawArea = filter.Apply(DrawArea);
            pictureBox1.Image = DrawArea;

            makeScreenCopy();
        }

        private void makeScreenCopy()
        {
            Image screenCopy = pictureBox1.Image;
            DrawArea = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(DrawArea);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            try
            {
                g.DrawImage(screenCopy, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            }catch
            {

            }
            pictureBox1.Image = DrawArea;
        }

        private void SecondScale_Click(object sender, EventArgs e)
        {
            LevelsLinear filter = new LevelsLinear();
            filter.InRed = new IntRange(30, 320);
            filter.InGreen = new IntRange(50, 240);
            filter.InBlue = new IntRange(10, 210);
            DrawArea = filter.Apply(DrawArea);
            pictureBox1.Image = DrawArea;
            
            makeScreenCopy();
        }

        private void ThirdScale_Click(object sender, EventArgs e)
        {
            Edges filter = new Edges();
            DrawArea = filter.Apply(DrawArea);
            pictureBox1.Image = DrawArea;

            makeScreenCopy();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();

            savefile.FileName = SaveFile.Text;
            if (savefile.FileName != null)
            {
                savefile.Filter = "Files|*.jpg;*.jpeg;";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    image.Dispose();

                    if (System.IO.File.Exists(savefile.FileName))
                        System.IO.File.Delete(savefile.FileName);

                    DrawArea.Save(savefile.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }

        private void SaveFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            makeScreenCopy();
        }
    }
}

