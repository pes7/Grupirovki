using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grupirovki.Test
{
    public partial class Test_form : Form
    {
        public Test_form()
        {
            InitializeComponent();
        }

        public double che(bool i)
        {
            double x = 0.2;
            if (i)
            {
                x = 0.5;
            }
            return x;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int x_1, y_1, x_2, y_2, len_1, len_2 ;
            double type_1, type_2, prior_1, prior_2;
            x_1 = int.Parse(textBox1.Text);
            y_1 = int.Parse(textBox2.Text);
            x_2 = int.Parse(textBox4.Text);
            y_2 = int.Parse(textBox3.Text);
            pictureBox1.Location = new Point(x_1,y_1);
            pictureBox2.Location = new Point(x_2, y_2);
            type_1 = che(checkBox1.Checked);
            type_2 = che(checkBox2.Checked);
            len_1 = (int)Math.Sqrt(Math.Pow(pictureBox1.Location.X - pictureBox3.Location.X, 2) + Math.Pow(pictureBox1.Location.Y - pictureBox3.Location.Y, 2));
            len_2 = (int)Math.Sqrt(Math.Pow(pictureBox2.Location.X - pictureBox3.Location.X, 2) + Math.Pow(pictureBox2.Location.Y - pictureBox3.Location.Y, 2));
            prior_1 = (len_1 * 1.2) / type_1;
            prior_2 = (len_2 * 1.2) / type_2;
            label1.Text = "First: " + prior_1 + "\nSecond: " + prior_2;
        }
    }
}
