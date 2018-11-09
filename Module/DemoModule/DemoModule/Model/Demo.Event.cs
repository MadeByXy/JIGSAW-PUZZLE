using RegistryLibrary.AppModule;
using RegistryLibrary.ImplementsClass;
using RegistryLibrary.Interface.Common;
using System;

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
