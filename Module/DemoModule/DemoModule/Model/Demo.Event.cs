using RegistryLibrary.AppModule;
using RegistryLibrary.Event;
using RegistryLibrary.Interface.Common;

namespace DemoModule.Model
{
    public partial class Demo : IDemo
    {
        public MessageEvent<DemoModel, UserInfo> CreatedEvent { get; set; }
        
        public MessageEvent<DemoModel, UserInfo> DeleteEvent { get; set; }
        
        public MessageEvent<DemoModel, UserInfo> ModifiedEvent { get; set; }
        
        public MessageEvent<DemoModel, UserInfo> PrepareCreatedEvent { get; set; }
        
        public MessageEvent<DemoModel, UserInfo> PrepareDeleteEvent { get; set; }

        public MessageEvent<DemoModel, UserInfo> PrepareModifiedEvent { get; set; }
    }
}
