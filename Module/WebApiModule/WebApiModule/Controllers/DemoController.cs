using RegistryLibrary.Interface.Common;
using System.Collections.Generic;
using System.Web.Http;

namespace WebApiModule.Controllers
{
    /// <summary>
    /// Restful风格的api demo
    /// </summary>
    public class DemoController : BaseController
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<UserInfo> Get()
        {
            return new List<UserInfo>
            {
                new UserInfo { UserId = "demo1" },
                new UserInfo { UserId = "demo2" }
            };
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        [HttpGet]
        public UserInfo Get(string id)
        {
            return new UserInfo { UserId = id };
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="user">数据实体</param>
        /// <returns></returns>
        [HttpPost]
        public UserInfo Post(UserInfo user)
        {
            return user;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <param name="user">数据实体</param>
        /// <returns></returns>
        [HttpPut]
        public UserInfo Put(string id, UserInfo user)
        {
            return user;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        [HttpDelete]
        public bool Delete(string id)
        {
            return true;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">id列表</param>
        /// <returns></returns>
        [HttpDelete]
        public bool BatchDelete(List<string> ids)
        {
            return true;
        }
    }
}
