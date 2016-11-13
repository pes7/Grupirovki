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
    class Zona
    {
        public int Id { get; set; }
        public int Id_capture { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Addition_var_resource { get; set; }
        public int Addition_export_resource { get; set; }
        public int Squads_in { get; set; }
        public int Max_suqads { get; set; }
        public int[] Id_of_suqads { get; set; }
        public PictureBox Picturebox { get; set; }
        /// <summary>
        /// Объект Зона - тут находяться все переменные которые связаны с созданием и спользованием елементов
        /// </summary>
        /// <param name="id">Id територии</param>
        /// <param name="id_capture">Id фракции (Glist) которая захватили територию</param>
        /// <param name="name">Название</param>
        /// <param name="type">Тип: 1 - гражданская локация 2 - военный объект</param>
        /// <param name="addition_var_resource">Сколько приносит военного ресурса за ход</param>
        /// <param name="addition_export_resource">Сколько приносит гражданского ресурса за ход</param>
        /// <param name="max_suqads">Сколько максимально сквадов может розместиться на точке</param>
        /// <param name="id_of_suqads">Массив с id сквадов которые расположились на точке</param>
        /// <param name="squads_in">Количество сквадов на точке</param>
        /// <param name="picturebox">Объект визуализации точки</param>
        public Zona(int id,int id_capture,string name,int type,int addition_var_resource,int addition_export_resource,int max_suqads,int[] id_of_suqads,int squads_in, PictureBox picturebox = null)
        {
            Id = id;
            Id_capture = id_capture;
            Name = name;
            Type = type;
            Addition_var_resource = addition_var_resource;
            Addition_export_resource = addition_export_resource;
            Max_suqads = max_suqads;
            Id_of_suqads = id_of_suqads;
            Picturebox = picturebox;
            Squads_in = squads_in;
        }

    }
}
