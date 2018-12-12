using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

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
            Model = Instance(GetDataFromXml(LoadConfiguration()));

            Console.WriteLine("注入完成");
        }

        /// <summary>
        /// 模块集合
        /// </summary>
        private static InjectionModel Model { get; set; }

        /// <summary>
        /// 配置文件访问节点
        /// </summary>
        private const string ConfigurationPath = "Constructor.config";

        /// <summary>
        /// 加载配置文件
        /// </summary>
        private static XmlDocument LoadConfiguration()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(ConfigurationPath);
            return xml;
        }

        /// <summary>
        /// 从配置文件中读取数据
        /// </summary>
        /// <param name="doc">配置文件</param>
        /// <returns>配置数据</returns>
        private static InjectionModel GetDataFromXml(XmlDocument doc)
        {
            var model = new InjectionModel();
            foreach (XmlNode node in doc.DocumentElement.FirstChild.ChildNodes)
            {
                if (!(node is XmlComment))
                {
                    var module = new InjectionModuleModel
                    {
                        Name = node.Attributes["name"]?.Value,
                        Assembly = node.Attributes["assembly"]?.Value,
                        Class = node.Attributes["class"]?.Value
                    };

                    if (string.IsNullOrWhiteSpace(module.Name))
                    {
                        //如果名称为空, 随便起一个名
                        module.Name = Guid.NewGuid().ToString();
                    }

                    foreach (XmlNode p_node in node.ChildNodes)
                    {
                        if (!(p_node is XmlComment))
                        {
                            var parameter = new InjectionParameterModel
                            {
                                Name = p_node.Attributes["name"]?.Value,
                                Ref = p_node.Attributes["ref"]?.Value,
                                Value = p_node.Attributes["value"]?.Value,
                            };

                            switch (p_node.Name)
                            {
                                case "constructor":
                                    module.ConstructorList.Add(parameter);
                                    break;
                                case "property":
                                    module.PropertyList.Add(parameter);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    model.Modules.Add(module);
                }
            }
            return model;
        }

        /// <summary>
        /// 初始化注入
        /// </summary>
        /// <param name="model">注入实体信息</param>
        private static InjectionModel Instance(InjectionModel model)
        {
            //加载待注入模块
            var pendinglist = model.Modules.Select(module => module).ToList();

            //加载需要注入模块
            var is_load = false;
            while (pendinglist.Count > 0)
            {
                foreach (var module in pendinglist.Select(module => module).ToList())
                {
                    //判断当前是否可以注入
                    if (module.ConstructorList.All(
                        constructor =>
                        string.IsNullOrEmpty(constructor.Ref) || model.InstanceCollection.ContainsKey(constructor.Ref)))
                    {
                        model.InstanceCollection.Add(
                            module.Name,
                            Instance(module, module.ConstructorList.ToDictionary(
                                constructor => constructor.Name,
                                constructor =>
                                string.IsNullOrEmpty(constructor.Ref) ? constructor.Value : model.InstanceCollection[constructor.Ref])));
                        pendinglist.Remove(module);
                    }
                    is_load = true;
                }

                if (is_load)
                {
                    is_load = false;
                }
                else
                {
                    throw new System.Exception("存在无法实例化的模块, 请检查是否配置正确",
                        new System.Exception(pendinglist.ToType<string>()));
                }
            }

            //加载属性
            foreach (var module in model.Modules)
            {
                var instance = model.InstanceCollection[module.Name];
                foreach (var property in module.PropertyList)
                {
                    instance.GetType().GetProperty(property.Name).SetValue(
                        model.InstanceCollection[module.Name],
                        string.IsNullOrEmpty(property.Ref) ? property.Value : model.InstanceCollection[property.Ref]);
                }
            }
            return model;
        }

        /// <summary>
        /// 注入对象
        /// </summary>
        /// <param name="module">待注入对象</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>注入结果对象</returns>
        private static object Instance(InjectionModuleModel module, Dictionary<string, object> parameters)
        {
            var assembly = Assembly.Load(module.Assembly);
            var type = assembly.GetType(module.Class);

            foreach (var constructor in
                type.GetConstructors().Where(
                    constructor => constructor.GetParameters().Count() <= parameters.Count).OrderByDescending(
                    constructor => constructor.GetParameters().Count()))
            {
                var paramsList = new List<object>();
                foreach (var parameter in constructor.GetParameters())
                {
                    if (parameters.ContainsKey(parameter.Name))
                    {
                        if (parameter.ParameterType.IsInterface)
                        {
                            paramsList.Add(parameters[parameter.Name]);
                        }
                        else
                        {
                            paramsList.Add(parameters[parameter.Name].ToType(parameter.ParameterType));
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                return constructor.Invoke(paramsList.ToArray());
            }
            return null;
        }
    }
}
