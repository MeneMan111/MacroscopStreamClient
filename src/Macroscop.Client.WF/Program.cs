using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LightInject;
using Macroscop.Client.Presentation.Core;
using Macroscop.Client.Presentation.Views;
using Macroscop.Client.Presentation.Presenters;

namespace Macroscop.Client.WF
{
    static class Program
    {
        public static readonly ApplicationContext Context = new ApplicationContext();

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var controller = new ApplicationController(new InjectAdapder())
                .RegisterView<IMainView, MainForm>()
                .RegisterView<IChannelView, ChannelForm>()
                .RegisterInstance(new ApplicationContext());

            controller.Run<MainPresenter>();
        }
    }
}
