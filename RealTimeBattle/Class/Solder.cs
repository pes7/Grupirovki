using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealTimeBattle.Class
{
    class Solder
    {
        public int Id { get; set; }
        public int Owner_fraction { get; set; }
        public int Id_squad { get; set; }
        public int Heal { get; set; }
        public int Attak { get; set; }
        public int Defence { get; set; }
        public int Agil { get; set; }
        public int Alive { get; set; }
        public int Aim { get; set; }
        public int State { get; set; }
        public string Name { get; set; }
        public PictureBox Pic { get; set; }
        public Solder(int id, int owner_fraction, int id_squad, int heal,int attak, int defence, int agil,int alive,int aim,int state, string name, PictureBox pic)
        {
            Id = id;
            Owner_fraction = owner_fraction;
            Id_squad = id_squad;
            Heal = heal;
            Attak = attak;
            Defence = defence;
            Agil = agil;
            Alive = alive;
            Aim = aim;
            State = state;
            Name = name;
            Pic = pic;
        }
    }
}
