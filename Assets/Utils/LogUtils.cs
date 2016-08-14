using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Utils
{
    public static class LogUtils
    {
        public static void LogArray(object[] array)
        {
            foreach(object eachObject in array)
            {
                Debug.Log(eachObject);
            }
        }
    }
}
