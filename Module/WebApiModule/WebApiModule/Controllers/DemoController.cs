using System.Web.Http;

namespace WebApiModule.Controllers
{
    public class DemoController : BaseController
    {
        [HttpGet]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}
