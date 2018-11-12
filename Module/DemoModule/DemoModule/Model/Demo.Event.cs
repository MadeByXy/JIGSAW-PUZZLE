using RegistryLibrary.AppModule;
using RegistryLibrary.Event;
using RegistryLibrary.Interface.Common;
using RegistryLibrary.Interface.Event;

namespace DemoModule.Model
{
    public partial class Demo : IDemo
    {
        public IEvent<DemoModel, UserInfo> CreatedEvent { get; set; }
        
        public IEvent<DemoModel, UserInfo> DeleteEvent { get; set; }
        
        public IEvent<DemoModel, UserInfo> ModifiedEvent { get; set; }
        
        public IEvent<DemoModel, UserInfo> PrepareCreatedEvent { get; set; }
        
        public IEvent<DemoModel, UserInfo> PrepareDeleteEvent { get; set; }

        public IEvent<DemoModel, UserInfo> PrepareModifiedEvent { get; set; }
    }
}
