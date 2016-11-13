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
    class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Image Icon_numb { get; set; }

        public int Count_of_solders { get; set; }
        public int Count_of_solders_need { get; set; }
        public int Count_of_var_resource { get; set; }
        public int Level_of_var_resource { get; set; }
        public int Count_of_export_resource { get; set; }
        public int Count_of_money { get; set; }

        public int Attak_min { get; set; }
        public int Attak_max { get; set; }
        public int Defence { get; set; }

        public int Id_base_set { get; set; }
        public int[] Id_terretori_capture { get; set; }
        public int[] Id_suqad_of_solders { get; set; }

        public int National_attak_bonus { get; set; }
        public int National_defend_bonus { get; set; }
        public int National_soled_count { get; set; }


        public int Id_aim { get; set; }

        public int Logic_type { get; set; }

        /// <summary>
        /// Объект хранящий переменные связаные с Группировками
        /// </summary>
        /// <param name="id">Id групировки</param>
        /// <param name="name">Название</param>
        /// <param name="icon_numb">Image - картинка группировки с файла с ресурсами</param>
        /// <param name="count_of_solder">Количество отрядов группировки</param>
        /// <param name="count_of_solders_need">Количество отрядов которые требуеться закупить</param>
        /// <param name="count_of_var_resource">Количество военного ресурса</param>
        /// <param name="level_of_var_resource">Уровень группировки -> зависит от военного ресурса</param>
        /// <param name="count_of_export_resource">Сколько гражданского ресурса на експорт</param>
        /// <param name="count_of_money">Сколько денег у группировки</param>
        /// <param name="attak_min">Минимальная атака группировки</param>
        /// <param name="attak_max">Максимальная атака группировки</param>
        /// <param name="defence">Защита группировки</param>
        /// <param name="id_base_set">Id (Zlist) теретории наиминованой Базой</param>
        /// <param name="id_terretori_capture">Массив с Id (Zlist) захваченых точек</param>
        /// <param name="id_suqad_of_solders">Массив с Id (Slist) отрядов группировки</param>
        /// <param name="logic_type">Тип логики групировки</param>
        /// <param name="national_attak_bonus">Национальный бонус к атаке</param>
        /// <param name="national_defend_bonus">Национальный бонус к защите</param>
        /// <param name="id_aim">Id точки которую группировка хочет атаковать</param>
        /// <param name="national_soled_count">Количество бойцов в группировке</param>
        public Group(int id, string name, Image icon_numb, int count_of_solder, int count_of_solders_need, int count_of_var_resource, int level_of_var_resource,int count_of_export_resource, int count_of_money, int attak_min, int attak_max, int defence, int id_base_set, int[] id_terretori_capture, int[] id_suqad_of_solders, int logic_type, int national_attak_bonus, int national_defend_bonus, int id_aim, int national_soled_count)
        {
            Id = id;
            Name = name;
            Icon_numb = icon_numb;
            Count_of_solders = count_of_solder;
            Count_of_solders_need = count_of_solders_need;
            Count_of_var_resource = count_of_var_resource;
            Level_of_var_resource = level_of_var_resource;
            Count_of_export_resource = count_of_export_resource;
            Count_of_money = count_of_money;
            Attak_min = attak_min;
            Attak_max = attak_max;
            Defence = defence;
            Id_base_set = id_base_set;
            Id_terretori_capture = id_terretori_capture;
            Id_suqad_of_solders = id_suqad_of_solders;
            National_attak_bonus = national_attak_bonus;
            National_defend_bonus = national_defend_bonus;
            Id_aim = id_aim;
            National_soled_count = national_soled_count;
        }
    }
}
