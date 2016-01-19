using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Macroscop.Client.Presentation.Core;

namespace Macroscop.Client.Presentation.Views
{
    public interface IMainView : IView
    {
        string ChannelId { get; }

        event Action ExpandChannel;

        void SetChannels(Dictionary<string, string> Channels);

    }
}
