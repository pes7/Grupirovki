using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupirovki.Classes
{
    class PlayerGroup
    {
        public static int Id_of_faction { get; set; }
        public PlayerGroup(int id_of_faction)
        {
            Id_of_faction = id_of_faction;
        }
    }
}
