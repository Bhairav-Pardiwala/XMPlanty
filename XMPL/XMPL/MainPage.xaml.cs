using BackgroundApplicationRelay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XMPL
{
	public partial class MainPage : ContentPage
	{
        BackgroundApplicationRelay.M2MQTTMod mod ;
        Label devStat;

        public MainPage()
		{
			InitializeComponent();
            devStat = this.FindByName<Label>("devstatus");
            devStat.Text = "Init";
            mod = new BackgroundApplicationRelay.M2MQTTMod(this);
            mod.init();
          
		}
        public void setStatus(String t)
        {
            devStat.Text = t;
        }
         void StartPump(object sender,EventArgs e)
        {
            //CommunicationMessage commMsg = new CommunicationMessage();
            //commMsg.id = "1";
            //commMsg.msg = "t";
            //mod.publishMessage(reciveTopic[0], SerializeIt(commMsg));
            CommunicationMessage commMsg = new CommunicationMessage();
            commMsg.id = "1";
            commMsg.msg = "StartPump";
            mod.SendCommmessage(commMsg);
        }
        void StopPump(object sender, EventArgs e)
        {
            //CommunicationMessage commMsg = new CommunicationMessage();
            //commMsg.id = "1";
            //commMsg.msg = "t";
            //mod.publishMessage(reciveTopic[0], SerializeIt(commMsg));
            CommunicationMessage commMsg = new CommunicationMessage();
            commMsg.id = "1";
            commMsg.msg = "StopPump";
            mod.SendCommmessage(commMsg);
        }
         void DeviceStatus(object sender, EventArgs e)
        {
            // CommunicationMessage commMsg = new CommunicationMessage();
            // commMsg.id = "1";
            // commMsg.msg = "t";
            //mod. publishMessage(reciveTopic[0], SerializeIt(commMsg));
            CommunicationMessage commMsg = new CommunicationMessage();
            commMsg.id = "1";
            commMsg.msg = "DeviceStatus";
            mod.SendCommmessage(commMsg);
        }
        void TimeSet(object sender, EventArgs e)
        {
            // CommunicationMessage commMsg = new CommunicationMessage();
            // commMsg.id = "1";
            // commMsg.msg = "t";
            //mod. publishMessage(reciveTopic[0], SerializeIt(commMsg));
            CommunicationMessage commMsg = new CommunicationMessage();
            commMsg.id = "1";
            commMsg.msg = "TimeSet";
            commMsg.response = DateTime.Now.ToString();
            mod.SendCommmessage(commMsg);
        }
    }
}
