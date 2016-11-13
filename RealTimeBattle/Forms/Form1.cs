using RealTimeBattle.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealTimeBattle
{
    public partial class Form1 : Form
    {
        /*
         Дописал до того момента что ети поцы вбегают в драку и пиздяться, логика выбора цели не обоснована, т.е надо сделать более интирестную которая будет
         базиться не только на росстоянии до объекта, потом поцы после входа в бой так с него и не выходят, надо сдлать проверку позиции после выхода
         с боя, так же сделать базы на которые победители возвращаються, и сделать возможность спавнить юнитов на ходу. + еще хотелось бы сделать камеру
         как в играх а так же сделать через дровинг вистрелы + надо сделать так что бы они не подходили в упор, добавить кроме сего колюзию, которую я 
         когда то написал, + еще нужно сделать им карту что бы они не могли глушить друг друга через стены хоть и в радиусе стрельбы, радиус надо еще написать
         + хотелось бы сделать так что бы юниты не видели друг друга по караям карты а бродили по контрольным точкам ищя врагов\фармя рес, + не маловажно
         сделать сквады, и так что бы они имели общую цель и ходили в месте, само собой лидера сквадов.
          
            Под самы конец добавить игрока который сможет повлиять на жизнь в ЗОНЕ :)
         */
        static List<Solder> Slist = new List<Solder>();
        static Random rand = new Random();
        static Graphics gr;
        static Pen pen = new Pen(Color.Red);

        static int gl_id = 0;

        public Form1()
        {
            InitializeComponent();
            gr = CreateGraphics();
            gr.DrawLine(pen, new Point(0, 0), new Point(400, 400));//Сделатб инициализацию Drowing
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Create_solders(6, 1);
            Create_solders(6, 0);
        }

        public void Create_solders(int x, int team)
        {
            string[] names = { "Алёша", "Андрей", "Ваня", "Боря", "Бодя", "Сёма", "Колян" };
            Color col;
            if (team == 1)
            {
                col = Color.Green;
            }else
            {
                col = Color.Red;
            }
            for(int i = 0; i < x; i++)
            {
                PictureBox pic = new PictureBox();
                pic.Size = new Size(8,8);
                pic.BackColor = col;
                pic.Location = new Point(rand.Next(100,600), rand.Next(100,600));
                pic.Name = "solder_" + gl_id;
                pic.Tag = gl_id;
                pic.Click += new EventHandler(squad_click);
                this.Controls.Add(pic);
                Solder sl = new Solder(gl_id, team, 0, 100, rand.Next(15, 30), rand.Next(2, 6), rand.Next(2, 10),1,9999,0, names[rand.Next(0, names.Length)], pic);
                Slist.Add(sl);
                Console.WriteLine("Created{"+ gl_id + "}");
                gl_id++;
            }
        }

        private void squad_click(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            int id = (int)pic.Tag;
            label1.Text = "Имя: " + Slist[id].Name + "{" + id + "}\nAim-id: " + Slist[id].Aim +"\nHeal: " + Slist[id].Heal + "\nAlive: " + Slist[id].Alive;
        }

        public int set_aim(int id)
        {
            int x = Slist[id].Pic.Location.X;
            int y = Slist[id].Pic.Location.Y;
            double[] prior = new double[Slist.Count];
            int[] who_id = new int[Slist.Count];
            for (int i = 0; i < Slist.Count; i++)
            {
                int x_1 = Slist[i].Pic.Location.X;
                int y_1 = Slist[i].Pic.Location.Y;
                if (Slist[i].Owner_fraction != Slist[id].Owner_fraction && Slist[i].Id != id && Slist[i].Alive == 1)
                {
                    int lengh = (int)Math.Sqrt(Math.Pow(x - x_1, 2) + Math.Pow(y - y_1, 2));
                    prior[i] = lengh;
                    who_id[i] = Slist[i].Id;
                }else
                {
                    who_id[i] = 9999;
                    prior[i] = 9999;
                }
            }
            int index_nearest_enemy = who_id[Array.IndexOf(prior, prior.Min())];
            //if (id >= 5) { System.Console.WriteLine("RED + " + index_nearest_enemy); } else { System.Console.WriteLine("GREEN + " + index_nearest_enemy); }
            return index_nearest_enemy;
        }

        public void logic()
        {
            for (int i = 0; i < Slist.Count; i++) {
                if (Slist[i].Aim == 9999 && Slist[i].Alive == 1)
                {
                    Slist[i].Aim = set_aim(i);
                }
            }
        }

        public void move()
        {
            for (int i = 0; i < Slist.Count; i++)
            {
                if (Slist[i].Aim != 9999 && Slist[i].Alive == 1)
                {
                    if (Slist[i].State == 0)
                    {
                        int x1 = Slist[i].Pic.Location.X;//начало
                        int y1 = Slist[i].Pic.Location.Y;

                        int x2 = Slist[Slist[i].Aim].Pic.Location.X;
                        int y2 = Slist[Slist[i].Aim].Pic.Location.Y;//конец

                        if (x1 > x2 && x1 != x2)
                        {
                            x1--;
                            Slist[i].Pic.Location = new Point(x1, y1);
                        }
                        if (y1 > y2 && y1 != y2)
                        {
                            y1--;
                            Slist[i].Pic.Location = new Point(x1, y1);
                        }
                        if (x1 < x2 && x1 != x2)
                        {
                            x1++;
                            Slist[i].Pic.Location = new Point(x1, y1);
                        }
                        if (y1 < y2 && y1 != y2)
                        {
                            y1++;
                            Slist[i].Pic.Location = new Point(x1, y1);
                        }

                        if (x1 == x2 && y1 == y2)
                        {
                            Slist[i].State = 1;
                        }
                    }
                    else
                    {
                        attak(i);
                    }
                }else{
                    //Console.WriteLine("END");
                }
            }
        }

        public void attak(int id) 
        {
            int aim = Slist[id].Aim;
            if (rand.Next(0, 100) < Slist[aim].Agil)
            {
                //if (id >= 5){System.Console.WriteLine("RED");}else{System.Console.WriteLine("GREEN");}
                Slist[aim].Heal -= (rand.Next(Slist[id].Attak - 10, Slist[id].Attak + 5) - Slist[aim].Defence);
                if(Slist[aim].Heal < 0)
                {
                    Slist[aim].Alive = 0;
                    Slist[aim].Pic.Hide();
                    Console.WriteLine(Slist[id].Name + "{" + id + "} убил " + Slist[aim].Name + "{" + aim + "}");
                    Slist[id].Aim = 9999;
                }
            }else
            {
                Console.WriteLine(Slist[aim].Name + "{"+ aim + "} увернулся от " + Slist[id].Name + "{" + id + "}");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            logic();
            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            move();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < Slist.Count; i++)
            {
                Slist[i].Pic.Location = new Point(rand.Next(100, 600), rand.Next(100, 600));
                Slist[i].State = 0;
                Slist[i].Aim = 9999;
            }
        }
    }
}
