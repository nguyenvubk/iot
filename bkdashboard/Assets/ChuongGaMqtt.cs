using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;
using DG.Tweening;

namespace ChuongGa
{

    public class Status_Data
    {
        public string project_id { get; set; }
        public string project_name { get; set; }
        public string station_name { get; set; }
        public List<data_ss> data_ss { get; set; }
        public string device_status { get; set; }
    }

    public class data_ss
    {
        public string ss_name { get; set; }
        public string ss_unit { get; set; }
        public string ss_value { get; set; }
    }

    public class Config_Data
    {
        public string device { get; set; }
        public string status { get; set; }
    }

    public class ControlFan_Data
    {
        public int fan_status { get; set; }
        public int device_status { get; set; }

    }

    public class ChuongGaMqtt : M2MqttUnityClient
    {
        public List<string> topics = new List<string>();


        public string msg_received_from_topic_status = "";
        public string msg_received_from_topic_led = "";


        private List<string> eventMessages = new List<string>();
        [SerializeField]
        public Status_Data _status_data;
        [SerializeField]
        public Config_Data _config_data;
        [SerializeField]
        public ControlFan_Data _controlFan_data;


        public void PublishLedConfig()
        {
            _config_data = new Config_Data();
            GetComponent<ChuongGaManager>().Update_Led_Value(_config_data);
            string msg_config = JsonConvert.SerializeObject(_config_data);
            client.Publish(topics[1], System.Text.Encoding.UTF8.GetBytes(msg_config), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("publish led config");
        }

        public void PublishPumpConfig()
        {
            _config_data = new Config_Data();
            GetComponent<ChuongGaManager>().Update_Pump_Value(_config_data);
            string msg_config = JsonConvert.SerializeObject(_config_data);
            client.Publish(topics[2], System.Text.Encoding.UTF8.GetBytes(msg_config), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("publish pump config");
        }

        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }


        protected override void OnConnecting()
        {
            base.OnConnecting();
            //SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            
            base.OnConnected();
            Status_Data _pub_data = new Status_Data();
            _pub_data.project_id = "1915983";
            _pub_data.project_name = "lab2Unity";
            _pub_data.station_name = "HCM";
            _pub_data.device_status = "1";
            _pub_data.data_ss = new List<data_ss> (){new data_ss(){ss_name = "temperature",ss_unit = "0" ,ss_value = "30"}, new data_ss(){ss_name = "humidity",ss_unit = "0" ,ss_value = "70"}};
            string msg_pub = JsonConvert.SerializeObject(_pub_data);
            client.Publish(topics[0], System.Text.Encoding.UTF8.GetBytes(msg_pub), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            SubscribeTopics();
            GetComponent<SwitchLayerLogin>().SwitchLayer();
        }

        protected override void SubscribeTopics()
        {

            foreach (string topic in topics)
            {
                if (topic != "")
                {
                    client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

                }
            }
        }

        protected override void UnsubscribeTopics()
        {
            foreach (string topic in topics)
            {
                if (topic != "")
                {
                    client.Unsubscribe(new string[] { topic });
                }
            }

        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            Debug.Log("CONNECTION FAILED! " + errorMessage);
        }

        protected override void OnDisconnected()
        {
            base.OnDisconnected();
            Debug.Log("Disconnected.");
            GetComponent<SwitchLayerLogin>().SwitchLayer();
        }

        protected override void OnConnectionLost()
        {
            Debug.Log("CONNECTION LOST!");
        }



        protected override void Start()
        {

            base.Start();
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("Received: " + msg);
            //StoreMessage(msg);
            if (topic == topics[0])
                ProcessMessageStatus(msg);
            if (topic == topics[1])
                ProcessMessageLed(msg);
            if (topic == topics[2])
                ProcessMessageLed(msg);
        }

        private void ProcessMessageStatus(string msg)
        {
             _status_data = JsonConvert.DeserializeObject<Status_Data>(msg);
            msg_received_from_topic_status = msg;
            GetComponent<ChuongGaManager>().Update_Status(_status_data);

        }
        private void ProcessMessageLed(string msg)
        {
             _config_data = JsonConvert.DeserializeObject<Config_Data>(msg);
            msg_received_from_topic_led = msg;
            GetComponent<ChuongGaManager>().Update_Control(_config_data);

        }

        // private void ProcessMessageControl(string msg)
        // {
        //     _controlFan_data = JsonConvert.DeserializeObject<ControlFan_Data>(msg);
        //     msg_received_from_topic_control = msg;
        //     GetComponent<ChuongGaManager>().Update_Control(_controlFan_data);

        // }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
            //if (autoTest)
            //{
            //    autoConnect = true;
            //}
        }

        public void UpdateConfig()
        {
           
        }

        public void UpdateControl()
        {

        }
    }
}