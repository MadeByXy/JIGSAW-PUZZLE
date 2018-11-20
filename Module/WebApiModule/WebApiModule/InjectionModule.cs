using Microsoft.Owin.Hosting;
using RegistryLibrary.Interface.Dependence;

namespace WebApiModule
{
    /// <summary>
    /// 注册模块
    /// </summary>
    public class InjectionModule: IInjection
    {
        public InjectionModule(string baseUri)
        {
            WebApp.Start<Startup>(baseUri);
        }
    }
}
