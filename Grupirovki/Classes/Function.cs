using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Grupirovki.Classes
{
    class Function
    {
        static List<Zona> Zlist = new List<Zona>();
        public List<Zona> get_map_points()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("Xmls/Map_points.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                int id = 999;
                string name = "null";
                int type = 1;
                int add_var = 0;
                int add_exp = 0;
                int max_suqads = 0;
                int position_x = 0;
                int position_y = 0;
                int[] ids = { };
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("id");
                    if (attr != null)
                        id = int.Parse(attr.Value);
                }
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    switch (childnode.Name)
                    {
                        case "name":
                            name = childnode.InnerText;
                            break;
                        case "type":
                            type = int.Parse(childnode.InnerText);
                            break;
                        case "add_var":
                            add_var = int.Parse(childnode.InnerText);
                            break;
                        case "add_exp":
                            add_exp = int.Parse(childnode.InnerText);
                            break;
                        case "max_squads":
                            max_suqads = int.Parse(childnode.InnerText);
                            break;
                        case "positions_x":
                            position_x = int.Parse(childnode.InnerText);
                            break;
                        case "positions_y":
                            position_y = int.Parse(childnode.InnerText);
                            break;
                    }
                }
                PictureBox pic = new PictureBox();
                pic.Location = new Point(position_x, position_y);
                Zona mew_zona = new Zona(id, 999, name, type, add_var, add_exp, max_suqads, ids, 0, pic);
                Zlist.Add(mew_zona);
            }
            return Zlist;
        }

        static List<Group> Glist = new List<Group>();
        public List<Group> get_groups()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("Xmls/Grupirovki.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                int id = 999;
                string name = "null";
                int id_mine_base = 0;
                int national_attak_bonus = 0;
                int national_defend_bonus = 0;
                int national_solders_count = 0;

                int[] ids_zone_capture = { };
                int[] ids_squads = { };
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("id");
                    if (attr != null)
                        id = int.Parse(attr.Value);
                }
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    switch (childnode.Name)
                    {
                        case "name":
                            name = childnode.InnerText;
                            break;
                        case "id_mine_base":
                            id_mine_base = int.Parse(childnode.InnerText);
                            break;
                        case "national_attak_bonus":
                            national_attak_bonus = int.Parse(childnode.InnerText);
                            break;
                        case "national_defend_bonus":
                            national_defend_bonus = int.Parse(childnode.InnerText);
                            break;
                        case "national_solders_count":
                            national_solders_count = int.Parse(childnode.InnerText);
                            break;
                    }
                }
                Group group = new Group(id,name,Resource1._1, 10, 0, 60, 1, 0, 70, 0, 0, 0, id_mine_base, ids_zone_capture, ids_squads,0, national_attak_bonus, national_defend_bonus, 9999, national_solders_count);
                Glist.Add(group);
            }
            return Glist;
        }
    }
}
