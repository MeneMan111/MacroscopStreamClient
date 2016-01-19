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

namespace Macroscop.Client.Presentation.Presenters
{
    public class MainPresenter : BasePresenter<IMainView>
    {
        Dictionary<string, string> ChannelInfo;

        public MainPresenter(IApplicationController controller, IMainView view)
            : base(controller, view)
        {
            ProcessRequest();

            SetChannels();

            View.ExpandChannel += () => ExpandChannel(View.ChannelId);

            //
        }

        private void ExpandChannel(string ChannelId)
        {
            Controller.Run<ChannelPresenter,String>(ChannelId);
        }

        private void SetChannels() 
        {
            View.SetChannels(ChannelInfo);
        }

        public void ProcessRequest() 
        {
            string uri = "http://demo.macroscop.com:8080/configex?login=root";
            XNamespace ns = "http://www.macroscop.com";

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uri);

            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                Stream ResponseStream = resp.GetResponseStream();

                var xdoc = XDocument.Load(ResponseStream);
                var items = xdoc.Descendants(ns + "ChannelInfo")
                                .ToDictionary(i => (string)i.Attribute("Id"),
                                              i => (string)i.Attribute("Name"));

                this.ChannelInfo = items;
                resp.Close();
                ResponseStream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Final" + ex.Message);
            }
        }

    }
}
