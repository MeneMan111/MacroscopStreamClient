using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Macroscop.Client.Presentation.Core;
using Macroscop.Client.Presentation.Views;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Drawing;
using Macroscop.Client.Presentation.Extensions;
using Macroscop.Client.Presentation.JStreamParser;

namespace Macroscop.Client.Presentation.Presenters
{
    public class ChannelPresenter : BasePresenter<IChannelView, String>
    {
        private JStreamResolver _JStreamResolver;

        private JpegResult _JpegResult;

        public string ChannelId;

        private bool StreamState;


        public ChannelPresenter(IApplicationController controller, IChannelView view)
            : base(controller, view)
        {
            ProcessFrame += View.mjpeg_FrameReady;

            View.StartStream += ParseStream;

            View.StopStream += StopStream;
        }


        public event Action<Image> ProcessFrame;


        public override void Run(String argument)
        {
            this.ChannelId = argument;
            View.Show();
        }

        public void StopStream()
        {
            StreamState = false;
        }

        public void ParseStream()
        {
            string uri = " http://demo.macroscop.com:8080/mobile?login=root&channelid=" + ChannelId + "&resolutionX=640&resolutionY=480&fps=25";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            request.BeginGetResponse(ResponseHandler, request);
        }

        private void ResponseHandler(IAsyncResult asyncResult)
        {
            StreamState = true;
            HttpWebRequest req = (HttpWebRequest)asyncResult.AsyncState;

            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.EndGetResponse(asyncResult);
                Stream ResponseStream = resp.GetResponseStream();
                BinaryReader br = new BinaryReader(ResponseStream);

                _JStreamResolver = new JStreamResolver(br);

                while (StreamState)
                {
                    _JpegResult = _JStreamResolver.ParseJpegResult();

                    using (MemoryStream mStream = new MemoryStream(_JpegResult.JpegBytes))
                    {
                        ProcessFrame(Image.FromStream(mStream));
                    }
                }
                resp.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Final" + ex.Message);
            }
        }
    
    //
    }
}
