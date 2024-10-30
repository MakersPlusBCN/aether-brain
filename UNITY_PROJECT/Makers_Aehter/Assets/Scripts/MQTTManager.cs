using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

public class MQTTManager : M2MqttUnityClient
{


    private List<string> eventMessages = new List<string>();
    public string[] topicSubscribe = { };


    protected override void OnConnecting()
    {
        base.OnConnecting();
        Debug.Log("[MQTT MANAGER] Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        //SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
    }

    protected override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("Connected to broker on " + brokerAddress + "\n");
        /*SetUiMessage("Connected to broker on " + brokerAddress + "\n");

        if (autoTest)
        {
            TestPublish();
        }*/
    }

    public void ResetPublicMessage()
    {
        client.Publish("imu0/cmd", System.Text.Encoding.UTF8.GetBytes("reset"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        Debug.Log("Reset message published");
        
    }

    protected override void SubscribeTopics()
    {
        //client.Subscribe( topicSubscribe, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        //client.Subscribe(new string[] { "imu0/event" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        //client.Subscribe(new string[] { "imu1/event" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

        client.Subscribe(new string[] { topicSubscribe[0] }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        client.Subscribe(new string[] { topicSubscribe[1] }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
    }

    protected override void UnsubscribeTopics()
    {
        client.Unsubscribe(topicSubscribe);
    }

    protected override void OnConnectionFailed(string errorMessage)
    {
        //AddUiMessage("CONNECTION FAILED! " + errorMessage);
        Debug.Log("CONNECTION FAILED! " + errorMessage);
    }

    protected override void OnDisconnected()
    {
        //AddUiMessage("Disconnected.");
        Debug.Log("Disconnected.");
        UnsubscribeTopics();

    }

    protected override void OnConnectionLost()
    {
        // AddUiMessage("CONNECTION LOST!");
        Debug.Log("CONNECTION LOST!");

    }

    protected override void Start()
    {
        Debug.Log("Ready.");
        //SetUiMessage("Ready.");
        //updateUI = true;
        base.Start();
    }

    protected override void DecodeMessage(string topic, byte[] message)
    {
        string msg = System.Text.Encoding.UTF8.GetString(message);
        //Debug.Log("Received - " + topic + ": " + msg);
        //StoreMessage(msg, topic);

        if (topic == StateMachine.Instance.config.getMqttTopic0())
        {
            GestureType msgValue = (GestureType)Int32.Parse(msg);
            GameManager.Instance.gesturesPlayerA.Add(msgValue);
        }

        if (topic == StateMachine.Instance.config.getMqttTopic1())
        {
            GestureType msgValue = (GestureType)Int32.Parse(msg);
            GameManager.Instance.gesturesPlayerB.Add(msgValue);
        }
    }

    /*private void StoreMessage(string eventMsg, string topic)
    {
        eventMessages.Add(eventMsg);
    }

    private void ProcessMessage(string msg, string topic)
    {
        //AddUiMessage("Received: " + msg);
        Debug.Log("Received: " + msg);
   
    }*/

    protected override void Update()
    {
        base.Update(); // call ProcessMqttEvents()

       /* if (eventMessages.Count > 0)
        {
            foreach (string msg in eventMessages)
            {
                ProcessMessage(msg);
            }
            eventMessages.Clear();
        }*/
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    private void OnValidate()
    {
        /*if (autoTest)
        {
            autoConnect = true;
        }*/
    }



}
