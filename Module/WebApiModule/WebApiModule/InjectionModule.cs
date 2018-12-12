using Microsoft.Owin.Hosting;

namespace WebApiModule
{
    /// <summary>
    /// 注册模块
    /// </summary>
    /// <remarks>引用本模块的同时, 还应在System项目下引用Microsoft.Owin.Host.HttpListener包, 否则会造成启动失败</remarks>
    public class InjectionModule
    {
        public InjectionModule(string baseUri)
        {
            WebApp.Start<Startup>(baseUri);
        }
    }
}
