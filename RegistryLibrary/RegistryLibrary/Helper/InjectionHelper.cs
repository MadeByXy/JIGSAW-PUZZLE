using RegistryLibrary.Interface.Dependence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RegistryLibrary.Helper
{
    /// <summary>
    /// 注入帮助类
    /// </summary>
    public class InjectionHelper
    {
        /// <summary>
        /// 模块列表
        /// </summary>
        private static List<Type> ModuleList { get; set; } = new List<Type>();

        /// <summary>
        /// 启动注入
        /// </summary>
        public static void StartUp()
        {
            Console.WriteLine("开始注入功能模块");

            //加载注入模块
            ModuleList = GetModules();

            //找到所有注入接口的继承类, 注入指定功能
            foreach (var type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly =>
            {
                try { return assembly.GetTypes(); } catch { return new Type[0]; }
            })
            .Where(type => type.IsClass && type.GetInterfaces().Contains(typeof(IInjection))))
            {
                foreach (var constructor in type.GetConstructors())
                {
                    //遍历构造方法及方法参数, 并根据参数类型进行注入
                    var parameters = new List<object>();
                    foreach (var parameter in constructor.GetParameters())
                    {
                        var moduleType = ModuleList.FirstOrDefault(module => module.GetInterface(parameter.ParameterType.Name) != null);
                        if (moduleType == null)
                        {
                            throw new TypeUnloadedException($"{type.Name}模块注入失败，请确认是否开启指定模块");
                        }
                        parameters.Add(moduleType);
                    }
                    Activator.CreateInstance(type, parameters.ToArray());
                }
            }

            Console.WriteLine("注入完成");
        }

        private static List<Type> GetModules()
        {
            var typeList = new List<Type>();

            foreach (var fileInfo in Directory.GetFiles(""))
            {
                var assembly = Assembly.LoadFile(fileInfo);
            }

            return typeList;
        }
    }
}
