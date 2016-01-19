using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Macroscop.Client.Presentation.Views;

namespace Macroscop.Client.WF
{
    public partial class MainForm : Form, IMainView
    {
        private readonly ApplicationContext _context;

        public MainForm(ApplicationContext context)
        {
            _context = context;

            InitializeComponent();

            listBox1.DoubleClick += (sender, args) => Invoke(ExpandChannel);
        }

        public new void Show()
        {
            _context.MainForm = this;
            Application.Run(_context);
        }

        public string ChannelId
        {
            get 
            {
                var spec = (KeyValuePair<string, string>)listBox1.SelectedItem;

                return spec.Key;
            }
        }

        public void SetChannels(Dictionary<string, string> Channels)
        {
            listBox1.DataSource = new BindingSource(Channels, null); ;
            listBox1.DisplayMember = "Value";
            listBox1.ValueMember = "Key";
        }

        public event Action ExpandChannel;

    }
}
