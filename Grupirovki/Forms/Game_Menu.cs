using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grupirovki.Forms
{
    public partial class Game_Menu : Form
    {
        static Debug dubg = new Debug();
        static Form1 game = new Form1(dubg);

        public Game_Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            game.start();
            string[] names = game.get_group_names_list();
            comboBox1.Items.AddRange(names);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            game.Show();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Location = new Point(game.Location.X - this.Size.Width,game.Location.Y);
            dubg.Location = new Point(game.Location.X - this.Size.Width, game.Location.Y + this.Height);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dubg.Size = new Size(this.Size.Width,game.Size.Height - this.Size.Height);
            dubg.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            game.co_list_change(comboBox1.SelectedIndex);
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                game.change_colors_of_factions_squads(comboBox1.SelectedIndex, true);
            }
            else
            {
                game.change_colors_of_factions_squads(0, false);
            }
        }
    }
}
