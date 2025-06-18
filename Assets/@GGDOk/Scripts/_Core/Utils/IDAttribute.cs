using System;
using System.Linq;
using System.Reflection;
using UnityEngine.Assertions;

namespace Core.Utils
{
    /// <summary>
    /// 렐릭, 아이템, 몬스터, 장비등 엔티티의 고유값을 클래스에 적용하는 Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class IDAttribute : Attribute
    {
        public readonly string ID;

        public IDAttribute(string id)
        {
            ID = id;
        }

        public static string GetID<T>() 
            => GetID(typeof(T));
        public static string GetID(Type type)
        {
            Assert.IsNotNull(type);
            return type.GetCustomAttribute<IDAttribute>()?.ID;
        }
        public static Type GetType(string id)
        {
            id = id.Trim();
            Assert.IsFalse(string.IsNullOrWhiteSpace(id));
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(t => GetID(t) == id)
                .FirstOrDefault();
        }
    }
}