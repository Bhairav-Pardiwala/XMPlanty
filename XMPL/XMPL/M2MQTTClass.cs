
//using M2Mqtt;
//using M2Mqtt.Messages;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace BackgroundApplicationRelay
{
    class M2MQTTMod
    {
        MqttClient cl;
        string sendTopic  = "starshaqos1";
        string[] reciveTopic = { "iot" };
        string diffResponse1 = "Hello World", diffResponse2 = "Hi Bhairav";
        Boolean run = true;
        String instanceid = "";
        XMPL.MainPage mpl;
        MqttFactory factory;
        IMqttClient mqttClient;

        public M2MQTTMod(XMPL.MainPage p)
        {
            this.mpl = p;
        }
        public  async Task CreateClient()
        {
            factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
    .WithClientId("Client1")
    .WithTcpServer("moose.rmq.cloudamqp.com",1883)
   
    //.WithTls()
    .WithCleanSession()
    .Build();
            try
            {
                mqttClient.Connected += MqttClient_Connected;
                await mqttClient.ConnectAsync(options);
                mqttClient.ApplicationMessageReceived += MqttClient_ApplicationMessageReceived;
                
            }
            catch (Exception e)
            {

              
            }

        }

        private async void MqttClient_Connected(object sender, MQTTnet.Client.MqttClientConnectedEventArgs e)
        {
            await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(reciveTopic[0]).Build());
            //await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(sendTopic).Build());
            msg = "Init Completed";
            Xamarin.Forms.Device.BeginInvokeOnMainThread(SetStatus);

          
        }
        static String msg = "";

        private void SetStatus()
        {
            mpl.setStatus(msg);
        }

        private void MqttClient_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.ApplicationMessage.Payload != null && !string.IsNullOrWhiteSpace(Encoding.UTF8.GetString(e.ApplicationMessage.Payload)))
            {
                CommunicationMessage mrcv = deserializeIt(e.ApplicationMessage.Payload);
                if (mrcv.id != instanceid)
                {
                    if (mrcv.msg.Equals("status"))
                    {
                        msg = mrcv.response;

                        Xamarin.Forms.Device.BeginInvokeOnMainThread(SetStatus);
                        //communicationmessage commmsg = new communicationmessage();
                        //commmsg.id = instanceid;
                        //commmsg.msg = diffresponse1;
                        //publishmessage(sendtopic, serializeit(commmsg));
                    }

                    else
                    {

                    }

                }
            }
        }

        public async Task init()
        {
            await CreateClient();


            //cl = new MqttClient ("moose.rmq.cloudamqp.com", 1883, false, MqttSslProtocols.None,t,a);
            
         
            //cl.Connect("starsha", "jcnssqgp:jcnssqgp", "_X-Z_K53Aw7LuQtwZ5amkWYeexWsnITz", true, 100);
            //byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE };
            //cl.Subscribe(reciveTopic, qosLevels);
            //cl.MqttMsgPublishReceived += Cl_MqttMsgPublishReceived;
            instanceid = Guid.NewGuid().ToString();

            
        }

        public void SendCommmessage(CommunicationMessage msg)
        {
            //CommunicationMessage commMsg = new CommunicationMessage();
            //commMsg.id = instanceid;
            //commMsg.msg = diffResponse1;
            msg.id = instanceid;

             publishMessage(sendTopic,  JsonConvert.SerializeObject(msg));
        }

        private X509Certificate a(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            throw new NotImplementedException();
        }

        private bool t(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            throw new NotImplementedException();
        }

        //private void Cl_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        //{
        //    if (e.Message != null && !String.IsNullOrWhiteSpace(Encoding.UTF8.GetString(e.Message)))
        //    {
        //        CommunicationMessage mrcv = deserializeIt(e.Message);
        //        if (mrcv.id != instanceid)
        //        {
        //            if (mrcv.msg.Equals("status"))
        //            {
        //                mpl.setStatus(mrcv.msg);
        //                //CommunicationMessage commMsg = new CommunicationMessage();
        //                //commMsg.id = instanceid;
        //                //commMsg.msg = diffResponse1;
        //                //publishMessage(sendTopic, SerializeIt(commMsg));
        //            }

        //            else
        //            {

        //            }
        //        }


        //    }
        //}

        public async void publishMessage(String topic, String message1)
        {

            try
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(sendTopic)
                    .WithPayload(message1)
                   // .WithExactlyOnceQoS()
                   // .WithRetainFlag()
                    .Build();

                await mqttClient.PublishAsync(message);
            }
            catch (Exception e)
            {


            }
        }
        public byte[] SerializeIt(Object obj)
        {
            try
            {
                //MemoryStream ms = new MemoryStream();
                //JsonSerializer serializer = new JsonSerializer();

                //// serialize product to BSON
                //BsonWriter writer = new BsonWriter(ms);
                //serializer.Serialize(writer, obj);
                //return ms.ToArray();

                return System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
            }
            catch (Exception)
            {


            }
            return new byte[10];
        }
        public CommunicationMessage deserializeIt( byte[] b)
        {
            //MemoryStream ms = new MemoryStream(b);
            //JsonSerializer serializer = new JsonSerializer();
            //BsonReader reader = new BsonReader(ms);
            //CommunicationMessage rcv = serializer.Deserialize<CommunicationMessage>(reader);
            //return rcv;
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<CommunicationMessage>(System.Text.Encoding.UTF8.GetString(b));
            }
            catch (Exception)
            {


            }
            return new CommunicationMessage();
        }
    }
}
