﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Macroscop.Client.Presentation.Views;
using System.Windows;

namespace Macroscop.Client.WF
{
    public partial class ChannelForm : Form, IChannelView
    {
        private readonly ApplicationContext _context;

        public ChannelForm(ApplicationContext context)
        {
            _context = context;

            InitializeComponent();

            this.Shown += (sender, args) => Invoke(StartStream);

            this.FormClosing += (sender, args) => Invoke(StopStream);
        }

        public event Action StartStream;

        public event Action StopStream;

        public void mjpeg_FrameReady(Image sender) 
        {
            pictureBox1.Image = sender;
        }

        private void Invoke(Action action)
        {
            if (action != null) action();
        }

    }
}
