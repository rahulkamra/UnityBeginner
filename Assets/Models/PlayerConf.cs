using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Models
{
    public class PlayerConf:ScriptableObject
    {
        public string NAME;
        public int HP = 100;
        public int MAX_ATTACK = 200;

    }
}
