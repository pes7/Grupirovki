using Grupirovki.Classes;
using Grupirovki.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grupirovki
{
    /*
     * Game Plan:
     * 1) Дорабоать/Переработать Логику и поведение сквадов
     * 2) Перевисти весе ресурсы на xml основу
     * 3) Сделать редактор карт
     * 4) Добавить участие/Фракцию игрока
    */

    public partial class Form1 : Form
    {
        static List<Group> Glist = new List<Group>();
        static List<Zona> Zlist = new List<Zona>();
        static List<Squad> Slist = new List<Squad>();
        static PlayerGroup Player = new PlayerGroup(0);

        static Random rand = new Random();
        static Debug Debg;

        static int co_list_index = 0;

        //World
        static int sell_price = 6;
        //

        public Form1(Debug debg)
        {
            InitializeComponent();
            Debg = debg;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            start();
        }

        public void start()
        {
            generate_resource();
            force_game();
        }

        public void force_game()
        {
            day.Start();
            move.Start();
        }

        public void generate_resource()
        {
            Function fun = new Function();
            Zlist = fun.get_map_points();
            create_map_points(Zlist.Count);
            Glist = fun.get_groups();
            create_groups(Glist.Count);

            /*
             * Мистер костыль, если не будет нолевого скавада, на сколько я понял - будет очень плохо 
             */
            Squad sq = new Squad(0, 999, 0, 999, 999, 0, 0);
            Slist.Add(sq);
            //
            create_default_squads(Glist.Count);
            prepare_res();

            //Zlist[3].Picturebox.Image = Resource1.water;
        }


        public void prepare_res()
        {
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Image = Resource1.map_2;
            pictureBox1.Size = this.Size;
            pictureBox1.SendToBack();
        }

        public void create_map_points(int count)
        {
            PictureBox[] pic = new PictureBox[Zlist.Count];

            for (int i = 0; i < count; i++)
            {
                pic[i] = new PictureBox();
                pic[i].Name = "pic_zona_" + i;
                pic[i].Size = new Size(32, 32);
                pic[i].Location = new Point(Zlist[i].Picturebox.Location.X, Zlist[i].Picturebox.Location.Y);
                pic[i].BackColor = Color.Black;
                pic[i].Tag = i;
                pic[i].Click += new EventHandler(zone_click);
                this.Controls.Add(pic[i]);
                Zlist[i].Picturebox = pic[i];
            }
        }

        private void zone_click(object sender, EventArgs e)
        {
            PictureBox cl = (PictureBox)sender;
            char delimiter = '_';
            String[] substrings = cl.Name.Split(delimiter);
            int id = int.Parse(substrings[2]);
            string name = "Пусто";
            try
            {
                if (Zlist[id].Id_capture < 60)
                {
                    name = Glist[Zlist[id].Id_capture].Name;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            label1.Text = "[" + name + "] " + Zlist[id].Name;
            label3.Text = "Отрядов: " + Zlist[id].Squads_in + "/" + Zlist[id].Max_suqads;
        }

        public void create_groups(int count)
        {
            Image[] imagess = { Resource1._4, Resource1.bandits, Resource1._1 };
            for (int i = 0; i < count; i++)
            {
                Glist[i].Icon_numb = imagess[i];
                //Отрисовка
                Zlist[Glist[i].Id_base_set].Picturebox.Image = Glist[i].Icon_numb;
                Zlist[Glist[i].Id_base_set].Id_capture = i;

                teritory_add(i, Glist[i].Id_base_set);
            }
        }

        public void teritory_add(int i, int id_ter)
        {
            int[] gge = new int[Glist[i].Id_terretori_capture.Length + 1];
            for (int uy = 0; uy < Glist[i].Id_terretori_capture.Length; uy++)
            {
                gge[uy] = Glist[i].Id_terretori_capture[uy];
            }
            gge[Glist[i].Id_terretori_capture.Length] = id_ter;

            Glist[i].Id_terretori_capture = gge;
        }

        public void squad_add_to_terr(int i, int id_squad)
        {
            int[] gge = new int[Zlist[i].Id_of_suqads.Length + 1];
            for (int uy = 0; uy < Zlist[i].Id_of_suqads.Length; uy++)
            {
                gge[uy] = Zlist[i].Id_of_suqads[uy];
            }
            gge[Zlist[i].Id_of_suqads.Length] = id_squad;

            Zlist[i].Id_of_suqads = gge;
        }

        public void teritory_remove(int i, int id_ter)
        {
            int[] gge = new int[Glist[i].Id_terretori_capture.Length];
            for (int uy = 0; uy < Glist[i].Id_terretori_capture.Length; uy++)
            {
                if (id_ter != Glist[i].Id_terretori_capture[uy])
                {
                    gge[uy] = Glist[i].Id_terretori_capture[uy];
                }
            }

            Glist[i].Id_terretori_capture = gge;
        }

        public void squad_remove_to_terr(int i, int id_squad)
        {
            int[] gge = new int[Zlist[i].Id_of_suqads.Length];
            for (int uy = 0; uy < Zlist[i].Id_of_suqads.Length; uy++)
            {
                if (id_squad != Zlist[i].Id_of_suqads[uy])
                {
                    gge[uy] = Zlist[i].Id_of_suqads[uy];
                }
            }
            Zlist[i].Id_of_suqads = gge;
        }

        public void create_default_squads(int count)
        {
            //Генерим мёртвые сквады
            PictureBox[] pice = new PictureBox[count * 45];
            for (int i = 0; i < count; i++)
            {
                int[] ids_array = new int[40];
                for (int g = 0; g < 40; g++)
                {
                    pice[g] = new PictureBox();
                    pice[g].Name = "pic_squad_" + Glist[i].Id + "_" + g;
                    pice[g].Size = new Size(8, 8);
                    pice[g].Location = new Point(Zlist[Glist[i].Id_base_set].Picturebox.Location.X, Zlist[Glist[i].Id_base_set].Picturebox.Location.Y);
                    pice[g].BackColor = Color.Green;
                    pice[g].Click += new EventHandler(squad_click);
                    pice[g].Tag = Slist.Count;
                    pice[g].Hide();
                    this.Controls.Add(pice[g]);

                    Squad sq = new Squad(Slist.Count, i, Glist[i].National_soled_count, Glist[i].Id_base_set, Glist[i].Id_base_set, 0, 0, pice[g]);
                    ids_array[g] = Slist.Count;
                    Slist.Add(sq);
                }
                Glist[i].Id_suqad_of_solders = ids_array;
            }
        }

        private void squad_click(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            int ide = (int)pic.Tag;
            label1.Text = "Отряд гупировки " + Glist[Slist[ide].Id_relate].Name + " id:" + ide;
            label3.Text = "От куда: " + Zlist[Slist[ide].Point_from].Name + "\nКуда: " + Zlist[Slist[ide].Point_to].Name;
        }

        private void move_Tick(object sender, EventArgs e)
        {
            for (int i = 1; i < Slist.Count; i++)//Заставим их двигаться, и отлавливать приход на точку
            {
                if (Slist[i].State == 1)
                {
                    int x1 = Slist[i].PictureBox.Location.X;//начало
                    int y1 = Slist[i].PictureBox.Location.Y;

                    int x2 = Zlist[Slist[i].Point_to].Picturebox.Location.X;
                    int y2 = Zlist[Slist[i].Point_to].Picturebox.Location.Y;//конец

                    if (x1 > x2 && x1 != x2)
                    {
                        x1--;
                        Slist[i].PictureBox.Location = new Point(x1, y1);
                    }
                    if (y1 > y2 && y1 != y2)
                    {
                        y1--;
                        Slist[i].PictureBox.Location = new Point(x1, y1);
                    }
                    if (x1 < x2 && x1 != x2)
                    {
                        x1++;
                        Slist[i].PictureBox.Location = new Point(x1, y1);
                    }
                    if (y1 < y2 && y1 != y2)
                    {
                        y1++;
                        Slist[i].PictureBox.Location = new Point(x1, y1);
                    }

                    if (x1 == x2 && y1 == y2)
                    {
                        arraive(i);
                    }
                }
            }
        }

        public void arraive(int i)
        {
            if (Zlist[Slist[i].Point_to].Id_capture != Slist[i].Id_relate)
            {
                if (Zlist[Slist[i].Point_to].Id_capture == 999 || Zlist[Slist[i].Point_to].Id_capture == Slist[i].Id_relate)//Если земля была не захваченой
                {
                    if (Zlist[Slist[i].Point_to].Id_capture == Slist[i].Id_relate)
                    {
                        Zlist[Slist[i].Point_to].Squads_in++;
                        squad_add_to_terr(Slist[i].Point_to, Slist[i].Id);
                        Slist[i].State = 0;
                    }
                    else
                    {
                        teritory_add(Slist[i].Id_relate, Zlist[Slist[i].Point_to].Id);
                        Zlist[Slist[i].Point_to].Picturebox.Image = Glist[Slist[i].Id_relate].Icon_numb;
                        Zlist[Slist[i].Point_to].Id_capture = Slist[i].Id_relate;

                        Zlist[Slist[i].Point_to].Squads_in++;
                        squad_add_to_terr(Slist[i].Point_to, Slist[i].Id);
                        Slist[i].State = 0;
                    }
                }
                else//Значит захвачена врагом, будет бой с губой
                {
                    /*Надо написать бой!1111*/
                    int who = Slist[i].Id_relate; // Атакует
                    int kud = Zlist[Slist[i].Point_to].Id_capture; // Держит оборону

                    int health_attaker = 30 * Glist[who].National_soled_count;
                    int health_defender = 30 * Glist[kud].National_soled_count;
                    int defend_bonus = 3;

                    int c_1 = Glist[who].National_soled_count;
                    int c_2 = Glist[kud].National_soled_count;

                    for (int hf = 1; hf > 0; hf++)
                    {
                        int attak = rand.Next(Glist[who].Attak_min, Glist[who].Attak_max);
                        health_defender = health_defender - (attak * c_1 - Glist[kud].Defence * c_2 - defend_bonus);
                        int attak_1 = rand.Next(Glist[kud].Attak_min, Glist[kud].Attak_max);
                        health_attaker = health_attaker - (attak_1 * c_2 - Glist[who].Defence * c_1 - defend_bonus);
                        if (health_attaker <= 0 && health_defender <= 0)
                        {
                            int[] ids = Zlist[Slist[i].Point_to].Id_of_suqads;
                            int r = rand.Next(0, ids.Length - 1);
                            deth(ids[r], Zlist[Slist[i].Point_to].Id);
                            deth(i, Zlist[Slist[i].Point_to].Id);
                            Zlist[Slist[i].Point_to].Squads_in = 0;
                            Console.WriteLine(Glist[who].Name + " вышли в ничью с " + Zlist[Slist[i].Point_to].Name + " | xp_a:" + health_attaker + " | xp_d:" + health_defender);
                            break;
                        }
                        if (health_attaker <= 0 && health_defender > 0)
                        {
                            deth(i, Zlist[Slist[i].Point_from].Id);
                            Console.WriteLine(Glist[who].Name + " проиграли атаку на " + Zlist[Slist[i].Point_to].Name + " | xp_a:" + health_attaker + " | xp_d:" + health_defender);
                            break;
                        }
                        if (health_defender <= 0 && health_attaker > 0)
                        {
                            for (int j = 0; j < Zlist[Slist[i].Point_to].Id_of_suqads.Length; j++)
                            {
                                deth(Zlist[Slist[i].Point_to].Id_of_suqads[j], Zlist[Slist[i].Point_to].Id);
                            }
                            teritory_remove(kud, Zlist[Slist[i].Point_to].Id);
                            Zlist[Slist[i].Point_to].Id_capture = who;
                            Zlist[Slist[i].Point_to].Picturebox.Image = Glist[who].Icon_numb;
                            Zlist[Slist[i].Point_to].Squads_in = 0;
                            Console.WriteLine(Glist[who].Name + " побидили атакуя " + Zlist[Slist[i].Point_to].Name + " | xp_a:" + health_attaker + " | xp_d:" + health_defender);
                            break;
                        }
                    }
                }
                //И отхват територии teritory_remove();
                //И смерть отряда deth();
            }
        }

        public void deth(int id, int where)
        {
            if (null_exept(id))
            {
                Slist[id].Alive = 0;
                Slist[id].PictureBox.Hide();
                Slist[id].State = 0;
                squad_remove_to_terr(where, id);
            }
        }

        public bool null_exept(int id)
        {
            if (id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void day_Tick(object sender, EventArgs e)
        {
            logic();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public string[] get_group_names_list()
        {
            string[] names = new string[Glist.Count];
            for (int i = 0; i < Glist.Count; i++)
            {
                names[i] = Glist[i].Name;
            }
            return names;
        }

        public void co_list_change(int index)
        {
            co_list_index = index;
            label2.Text = Glist[index].Name;
            label4.Text = "Деньги: " + Glist[index].Count_of_money + "\nВоенный ресурс: " + Glist[index].Count_of_var_resource + "\nУровень развития: " + Glist[index].Level_of_var_resource + "\nКоличество солдат: " + Glist[index].Count_of_solders;
            pictureBox2.Image = Glist[index].Icon_numb;
        }

        public void logic()
        {
            global_event();
            think();
        }

        public void global_event()
        {
            //Тут будут происходить события влияющие на войну групировок(Сложно)
            //Смена цен на рынке
            sell_change();
            
        }

        public void sell_change()
        {

        }

        public void think()
        {
            Debg.listBox1.Items.Clear();
            for (int i = 0; i < Glist.Count; i++)
            {
                //Просчитка уровня снабжения групировки
                int lvl = charge_var_lvl(i);
                Glist[i].Level_of_var_resource = lvl;
                //Просчитка боевых характеристик сквадов групировки
                int[] dd = charge_attak(lvl, i);
                Glist[i].Attak_min = dd[0];
                Glist[i].Attak_max = dd[1];
                Glist[i].Defence = charge_defend(lvl,i);
                //Просчитака ближайших целей для атаки
                think_attak_aims(i);
                //Получить коливо сквадов на локациях
                get_squads_in_zones(i);
                //Посчитать сколько сквадов нужно, и нанять коливо людей
                reqruit(i);
                //Создать новые сквады(Сложно) //Послать сквады на оборону позиций(Средне)
                create_squads(i);
                //Выплатить жалование сквадам(Легко) Оставим на конец
                pay_my_money();
                //Послать сквады на оборону позиций(Очень сложно)

                //Послать сквады на атаку позиций(Сложно)
                attak(i);
                //Собрать ресурсы с локаций(Легко)
                take_resources(i);
                //Продать експортный ресурс(Легко)
                sell_export(i);
            }
        }

        public void pay_my_money()
        {

        }

        public void sell_export(int g_num)
        {
            int money = Glist[g_num].Count_of_money;
            int export = Glist[g_num].Count_of_export_resource;
            money = money + export * sell_price;
            export = 0;
            Glist[g_num].Count_of_money = money;
            Glist[g_num].Count_of_export_resource = export;
        }

        public void take_resources(int g_num){
            Debg.listBox4.Items.Clear();
            int var = Glist[g_num].Count_of_var_resource;
            int export = Glist[g_num].Count_of_export_resource;
            for (int i = 0; i< Glist[g_num].Id_terretori_capture.Length; i++)
            {
                var = var + Zlist[Glist[g_num].Id_terretori_capture[i]].Addition_var_resource;
                export = export + Zlist[Glist[g_num].Id_terretori_capture[i]].Addition_export_resource;
                Debg.listBox4.Items.Add(Zlist[Glist[g_num].Id_terretori_capture[i]].Name + " || " + Glist[g_num].Name);
            }
            Glist[g_num].Count_of_var_resource = var;
            Glist[g_num].Count_of_export_resource = export;
        }

        public void attak(int g_num)
        {
            //Понять что нам нужно атаковать
            //Console.WriteLine("We need to attak " + Zlist[Glist[g_num].Id_aim].Name + " | " + Glist[g_num].Name);
            //
            bool kke = false;
            for (int hy = 0; hy < Slist.Count; hy++)
            {
                if(Slist[hy].Id_relate == g_num && Slist[hy].Point_to == Glist[g_num].Id_aim && Slist[hy].Alive == 1)
                {
                    kke = true;
                }
            }
            if (kke != true)
            {
                int aim_id = Zlist[Glist[g_num].Id_aim].Id;
                int base_id = Glist[g_num].Id_base_set;
                if (Zlist[base_id].Squads_in > 1)
                {
                    int[] ids = Zlist[base_id].Id_of_suqads;
                    int r = rand.Next(0, ids.Length - 1);

                    Zlist[Glist[g_num].Id_base_set].Squads_in--;
                    squad_remove_to_terr(base_id, ids[r]);

                    Slist[ids[r]].Point_from = base_id;
                    Slist[ids[r]].Point_to = aim_id;
                    Slist[ids[r]].State = 1;
                }
            }
        }

        public void create_squads(int g_num)
        {
            //Смотрим где у нас мало сквадов на точках
            int zero = 0;
            for (int i = 0; i < Zlist.Count; i++)
            {
                if (Zlist[i].Id_capture == g_num && Zlist[i].Squads_in == 0)//Если на точке 0 сквадов // Только для баз, надо сделать для всех точек
                {
                    //Проверяем сколько живих у нас сквадов
                    for (int h = 0; h < Slist.Count; h++)
                    {
                        if (Slist[h].Id_relate == g_num && Slist[h].Alive == 0 && Glist[g_num].Count_of_solders >= Glist[g_num].National_soled_count)//Создаём сквад при наличие людей
                        {
                            Glist[g_num].Count_of_solders = Glist[g_num].Count_of_solders - Glist[g_num].National_soled_count;

                            Slist[h].Alive = 1;
                            Slist[h].Count_of_solders = Glist[g_num].National_soled_count;
                            Slist[h].Point_from = Glist[g_num].Id_base_set;//Пускаем их с базы
                            Slist[h].Point_to = Zlist[i].Id;//на ту точку
                            Slist[h].State = 1;
                            Slist[h].PictureBox.Location = new Point(Zlist[Glist[g_num].Id_base_set].Picturebox.Location.X, Zlist[Glist[g_num].Id_base_set].Picturebox.Location.Y);
                            Slist[h].PictureBox.Show();
                            Slist[h].PictureBox.BringToFront();

                            Zlist[i].Squads_in++;

                            squad_add_to_terr(i,h);//Закрепляем за територией сквад

                            zero++;
                            //Console.WriteLine(Glist[g_num].Name);
                            break;
                        }
                    }
                }
            }
            if(zero == 0)//Если все точки имеют хотябы по одному отряду, наполняем базу
            {
                for (int h = 0; h < Slist.Count; h++)
                {
                    if (Slist[h].Id_relate == g_num && Slist[h].Alive == 0 && Glist[g_num].Count_of_solders >= Glist[g_num].National_soled_count && Zlist[Glist[g_num].Id_base_set].Squads_in < Zlist[Glist[g_num].Id_base_set].Max_suqads)//Создаём сквад при наличие людей
                    {
                        Glist[g_num].Count_of_solders = Glist[g_num].Count_of_solders - Glist[g_num].National_soled_count;

                        Slist[h].Alive = 1;
                        Slist[h].Count_of_solders = Glist[g_num].National_soled_count;
                        Slist[h].Point_from = Glist[g_num].Id_base_set;//Пускаем их с базы
                        Slist[h].Point_to = Zlist[Glist[g_num].Id_base_set].Id;//на ту точку
                        Slist[h].State = 1;
                        Slist[h].PictureBox.Location = new Point(Zlist[Glist[g_num].Id_base_set].Picturebox.Location.X, Zlist[Glist[g_num].Id_base_set].Picturebox.Location.Y);
                        Slist[h].PictureBox.Show();
                        Slist[h].PictureBox.BringToFront();

                        Zlist[Glist[g_num].Id_base_set].Squads_in++;

                        squad_add_to_terr(Glist[g_num].Id_base_set, h);//Закрепляем за територией сквад

                        Console.WriteLine(Glist[g_num].Name);
                        break;
                    }
                }
            }
            if (Zlist[Glist[g_num].Id_base_set].Squads_in == Zlist[g_num].Max_suqads)
            {
                for (int i = 0; i < Zlist.Count; i++)
                {
                    if (Zlist[i].Id_capture == g_num)
                    {
                        for (int h = 0; h < Slist.Count; h++)
                        {
                            if (Slist[h].Id_relate == g_num && Slist[h].Alive == 0 && Glist[g_num].Count_of_solders >= Glist[g_num].National_soled_count)//Создаём сквад при наличие людей
                            {
                                Glist[g_num].Count_of_solders = Glist[g_num].Count_of_solders - Glist[g_num].National_soled_count;

                                Slist[h].Alive = 1;
                                Slist[h].Count_of_solders = Glist[g_num].National_soled_count;
                                Slist[h].Point_from = Glist[g_num].Id_base_set;//Пускаем их с базы
                                Slist[h].Point_to = Zlist[i].Id;//на ту точку
                                Slist[h].State = 1;
                                Slist[h].PictureBox.Location = new Point(Zlist[Glist[g_num].Id_base_set].Picturebox.Location.X, Zlist[Glist[g_num].Id_base_set].Picturebox.Location.Y);
                                Slist[h].PictureBox.Show();
                                Slist[h].PictureBox.BringToFront();

                                Zlist[i].Squads_in++;

                                squad_add_to_terr(i, h);//Закрепляем за територией сквад

                                zero++;
                                //Console.WriteLine(Glist[g_num].Name);
                                break;
                            }
                        }
                    }
                }
            }
            //end
        }

        public void reqruit(int g_num)//Я написал только для случая с пустой територоией, нужно написать еще с пополнением уже не пустых тер, и главной базы.
        {
            //Стоимость одного солдата
            int cost_m = 5;
            int cost_v = 2;
            int money = Glist[g_num].Count_of_money;
            int var = Glist[g_num].Count_of_var_resource;

            int how_much_we_need = Glist[g_num].Count_of_solders_need;//Проблема в том что закупаем мы только для одного региона или что то в етом роже || высокий риск багов!!11

            if (how_much_we_need == 0)//Только если закупить нужно 0, иначе будет перекуп-баг
            {
                int zero = 0;//Проверим сколько у нас пустых тер
                for (int i = 0; i < Zlist.Count; i++)
                {
                    if (Zlist[i].Id_capture == g_num && Zlist[i].Squads_in == 0)
                    {
                        //очень нужны отряды!!!111
                        if (Glist[g_num].Count_of_solders < Glist[g_num].National_soled_count)//Проверяем, может мы уже купили солдат???77
                        {
                            how_much_we_need = how_much_we_need + Glist[g_num].National_soled_count;
                            zero++;
                        }
                    }
                }
                if(zero == 0 && Zlist[Glist[g_num].Id_base_set].Squads_in < Zlist[Glist[g_num].Id_base_set].Max_suqads)//Если нету пустых переходим к следующей фазе, докуп солдат на главную базу
                {
                    /*
                    int max_squads = Zlist[Glist[g_num].Id_base_set].Max_suqads;
                    int squads = Zlist[Glist[g_num].Id_base_set].Squads_in;
                    int yy = max_squads - squads;
                    */
                    int yy = 1;
                    how_much_we_need = yy * Glist[g_num].National_soled_count;
                }
            }

            if (how_much_we_need != 0)
            {
                if (money > how_much_we_need * cost_m && var > how_much_we_need * cost_v)//Если бабла хватает закупаем сколько надо
                {
                    Glist[g_num].Count_of_money = money - how_much_we_need * cost_m;
                    Glist[g_num].Count_of_var_resource = var - how_much_we_need * cost_v;
                    Glist[g_num].Count_of_solders = how_much_we_need;
                    how_much_we_need = 0;
                }
                else
                {
                    //Смотрим сколько можно купить и покупаем
                    int[] how = { 0, 0 };

                    how[0] = (int)money / cost_m;
                    how[1] = (int)var / cost_v;

                    Glist[g_num].Count_of_money = money - how.Min() * cost_m;
                    Glist[g_num].Count_of_var_resource = var - how.Min() * cost_v;
                    Glist[g_num].Count_of_solders = how.Min();

                    how_much_we_need = how_much_we_need - how.Min();
                }
            }
            //Console.WriteLine(Glist[g_num].Count_of_solders.ToString());
            Glist[g_num].Count_of_solders_need = how_much_we_need;//Записуем сколько осталось закупить
        }

        public void get_squads_in_zones(int g_num)
        {
            for (int i = 0; i < Zlist.Count; i++)
            {
                if (Zlist[i].Id_capture == g_num)
                {
                    Debg.listBox1.Items.Add(Glist[g_num].Name + ": " + Zlist[i].Name + " " + Zlist[i].Squads_in + "/" + Zlist[i].Max_suqads);
                }
            }
        }

        public double che(int i)// Выдаём коефициент free точкам для приоритетности
        {
            double x = 0.2;
            if (i == 999)
            {
                x = 0.5;
            }
            return x;
        }

        public void think_attak_aims(int g_num)// Надо добавить приоритетность захвата Free точек
        {
            Debg.listBox2.Items.Clear();
            Debg.listBox3.Items.Clear();
            int id_self_base = Glist[g_num].Id_base_set;
            int x = Zlist[id_self_base].Picturebox.Location.X;
            int y = Zlist[id_self_base].Picturebox.Location.Y;
            double[] prior = new double[Zlist.Count];// Приоритет
            for(int i = 0; i < Zlist.Count; i++)
            {
                int x_1 = Zlist[i].Picturebox.Location.X;
                int y_1 = Zlist[i].Picturebox.Location.Y;
                Debg.listBox3.Items.Add("x:" + x + " y:" + y + " || x:" + x_1 + " y:" + y_1 + " || " + Glist[g_num].Name);
                if (x != x_1 && y != y_1 && Zlist[i].Id_capture != g_num)//Проверили не наше ли ето
                {

                    int lengh = (int)Math.Sqrt(Math.Pow(x - x_1, 2) + Math.Pow(y - y_1, 2));
                    prior[i] = (lengh * 1.2) / che(Zlist[i].Id_capture);
                    Debg.listBox2.Items.Add(prior[i] + " | " + Glist[g_num].Name);
                }
                else
                {
                    prior[i] = 9999;
                    Debg.listBox2.Items.Add(prior[i] + " | " + Glist[g_num].Name);
                }
            }

            int index_nearest_location = Array.IndexOf(prior, prior.Min());
            Glist[g_num].Id_aim = index_nearest_location;//Нужно проверять, если наш то не нападать// написано

            /*Ищем ближайший не наший блокпост.
            if(owner == self){
                Array.IndexOf(lengh, lengh.Min()+g)
                g++;
            }
            */

        }

        public int charge_var_lvl(int i)
        {
            int lvl = 1;
            if (Glist[i].Count_of_var_resource > 200)
            {
                lvl = 2;
            }
            if (Glist[i].Count_of_var_resource < 600 && Glist[i].Count_of_var_resource > 200)
            {
                lvl = 3;
            }
            if (Glist[i].Count_of_var_resource > 600)
            {
                lvl = 4;
            }
            return lvl;
        }

        public int charge_defend(int lvl, int g_num)
        {
            int defend = 0;

            switch (lvl) {
                case 1:
                    defend = Glist[g_num].National_defend_bonus + 2;
                    break;
                case 2:
                    defend = Glist[g_num].National_defend_bonus + 4;
                    break;
                case 3:
                    defend = Glist[g_num].National_defend_bonus + 6;
                    break;
                case 4:
                    defend = Glist[g_num].National_defend_bonus + 8;
                    break;
            }

            return defend;
        }

        public int[] charge_attak(int lvl, int g_num)
        {
            int[] attak = { 0,0 };

            switch (lvl)
            {
                case 1:
                    attak[0] = Glist[g_num].National_attak_bonus + 2;
                    attak[1] = Glist[g_num].National_attak_bonus + 8;
                    break;
                case 2:
                    attak[0] = Glist[g_num].National_attak_bonus + 4;
                    attak[1] = Glist[g_num].National_attak_bonus + 10;
                    break;
                case 3:
                    attak[0] = Glist[g_num].National_attak_bonus + 6;
                    attak[1] = Glist[g_num].National_attak_bonus + 12;
                    break;
                case 4:
                    attak[0] = Glist[g_num].National_attak_bonus + 8;
                    attak[1] = Glist[g_num].National_attak_bonus + 14;
                    break;
            }

            return attak;
        }

        public void change_colors_of_factions_squads(int who_is_suspected, bool turn)
        {
            if (turn == true)
            {
                for (int i = 1; i < Slist.Count; i++)
                {
                    int who_related = Slist[i].Id_relate;
                    if (who_is_suspected == who_related)
                    {
                        Slist[i].PictureBox.BackColor = Color.Green;
                    }
                    else
                    {
                        Slist[i].PictureBox.BackColor = Color.Red;
                    }
                }
            }else{
                for (int i = 1; i < Slist.Count; i++)
                {
                    Slist[i].PictureBox.BackColor = Color.Green;
                }
            }
        }

    }
}
