using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
namespace Utils
{
    public class TypeUtility
    {
        public static List<Type> getAllClassesOfType<T>(AppDomain appDomain)
        {
            List<Type> result = new List<Type>();
            Assembly[] assemblies = appDomain.GetAssemblies();
            Type type = typeof(T);
            foreach (Assembly eachAssembly in assemblies)
            {
                Type[] types = eachAssembly.GetTypes();
                foreach (Type eachType in types)
                {
                    if (eachType.IsSubclassOf(type))
                    {
                        Debug.Log(eachType);
                    }
                }
            }

            return result;
        }

        public static Type[] getAllClassOfTypeInAssembly<T>(string name)
        {
            AssemblyName assemblyName = new AssemblyName(name);
            Assembly assembly = Assembly.Load(assemblyName);
            Type currentType = typeof(T);
            Type[] result = (from type in assembly.GetTypes() where type.IsSubclassOf(currentType) select type).ToArray();
            return result;
        }
    }
}


