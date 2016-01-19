using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Macroscop.Client.Presentation.Core;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Drawing;

namespace Macroscop.Client.Presentation.Views
{
    public interface IChannelView : IView
    {
        event Action StartStream;

        event Action StopStream;

        void mjpeg_FrameReady(Image sender);
    }
}
