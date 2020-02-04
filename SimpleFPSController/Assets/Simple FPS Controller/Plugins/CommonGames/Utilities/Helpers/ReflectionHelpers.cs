using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

//using UnityEditor;

using CommonGames.Utilities;
using CommonGames.Utilities.Extensions;

using JetBrains.Annotations;

namespace CommonGames.Utilities.Helpers
{
    public static class ReflectionHelpers
    {
        private static void DebugPropertyValues(System.Object obj)
        {
            Type t = obj.GetType();

            Debug.Log($"Type is: {t.Name}");

            PropertyInfo[] props = t.GetProperties();

            Debug.Log($"Properties (N = {props.Length}):");

            foreach(PropertyInfo __prop in props)
            {
                Debug.Log(__prop.GetIndexParameters().Length == 0
                    ? $"   {__prop.Name} ({__prop.PropertyType.Name}): {__prop.GetValue(obj)}"
                    : $"   {__prop.Name} ({__prop.PropertyType.Name}): <Indexed>");
            }
        }

        [PublicAPI]
        public static Type[] GetAllDerivedTypes(this AppDomain appDomain, Type type)
        {
            List<Type> __result = new List<Type>();
            Assembly[] __assemblies = appDomain.GetAssemblies();

            foreach(Assembly __assembly in __assemblies)
            {
                Type[] __types = __assembly.GetTypes();

                foreach(Type __type in __types)
                {
                    if(__type.IsSubclassOf(c: type))
                    {
                        __result.Add(__type);
                    }
                }
            }

            return __result.ToArray();
        }
    }

}