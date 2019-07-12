using Panacea.Modularity.Relays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panacea.Modules.SerialRelay
{
    class SerialRelayPlugin : IRelayModulePlugin
    {
        IRelayModule _module;

        public Task BeginInit()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

        public Task EndInit()
        {
            return Task.CompletedTask;
        }

        public Task<IRelayModule> GetModuleAsync()
        {
            if (_module == null)
            {
                _module = new SerialRelayModule();
            }
            return Task.FromResult(_module);
        }

        public Task Shutdown()
        {
            return Task.CompletedTask;
        }
    }
}
