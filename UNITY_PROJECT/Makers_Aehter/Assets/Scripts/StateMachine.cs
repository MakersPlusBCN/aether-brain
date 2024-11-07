using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using M2MqttUnity;

[Serializable]
public class Config
{
    [SerializeField] protected string arduinoPort;
    [SerializeField] protected string mqtt_topic_0;
    [SerializeField] protected string mqtt_topic_1;
    [SerializeField] protected string mqtt_username;

    [SerializeField] protected string mqtt_password;
    [SerializeField] protected string mqtt_broker_address;
    [SerializeField] protected int mqtt_broker_port;
    [SerializeField] protected bool mqtt_active;



    public Config(string _arduinoPort, string _mqtt_topic_0, string _mqtt_topic_1, string _mqtt_username, string _mqtt_password,string _mqtt_broker_address, int _mqtt_broker_port, bool _mqtt_active)
    {
        arduinoPort = _arduinoPort;
        mqtt_topic_0 = _mqtt_topic_0;
        mqtt_topic_1 = _mqtt_topic_1;
        mqtt_username = _mqtt_username;
        mqtt_password = _mqtt_password;
        mqtt_broker_address = _mqtt_broker_address;
        mqtt_broker_port = _mqtt_broker_port;
        mqtt_active = _mqtt_active;
    }

    public string getArduinoPort()
    {
        return arduinoPort;
    }

    public string getMqttTopic0()
    {
        return mqtt_topic_0;
    }

    public string getMqttTopic1()
    {
        return mqtt_topic_1;
    }

    public string getMqttUsername()
    {
        return mqtt_username;
    }

    public string getMqttPassword ()
    {
        return mqtt_password;
    }

    public string getMqttAdress()
    {
        return mqtt_broker_address;
    }

    public int getMqttBrokerPort()
    {
        return mqtt_broker_port;
    }

    public bool getMqttActive()
    {
        return mqtt_active;
    }

}




public class StateMachine : Singleton<StateMachine>
{
    public List<StateBase> states;
    public StatesEnum currentState;
    public StatesEnum prevState;
    public StatesEnum nextState;

    public Config config;
    public GameObject MQTTManagerRef;


    private string configPath = Application.streamingAssetsPath + "/config.json";



    public void Awake()
    {


        StartCoroutine(StartApp());

        Application.runInBackground = true;
    }

    private IEnumerator StartApp()
    {
        //Read config file
        GameManager.Instance.appIsReady = false;
        string jsonString = File.ReadAllText(configPath);
        config = JsonUtility.FromJson<Config>(jsonString);

        yield return new WaitForSeconds(1f);

        Debug.Log("CONFIG Arduino port --> " + config.getArduinoPort());
        ArduinoManager.Instance.serialController.portName = config.getArduinoPort();
        GameManager.Instance.isMQTTActive = config.getMqttActive();

        if (GameManager.Instance.isMQTTActive)
        {
            MQTTManagerRef.GetComponent<M2MqttUnityClient>().brokerPort = config.getMqttBrokerPort();
            MQTTManagerRef.GetComponent<M2MqttUnityClient>().brokerAddress = config.getMqttAdress();
            MQTTManagerRef.GetComponent<M2MqttUnityClient>().mqttPassword = config.getMqttPassword();
            MQTTManagerRef.GetComponent<M2MqttUnityClient>().mqttUserName = config.getMqttUsername();
            MQTTManagerRef.GetComponent<MQTTManager>().topicSubscribe[0] = config.getMqttTopic0();
            MQTTManagerRef.GetComponent<MQTTManager>().topicSubscribe[1] = config.getMqttTopic1(); 
        }

        //Active Receive info Arduino - sensor pulseras -
        if (ArduinoManager.Instance.gameObject.activeSelf)
        {
            ArduinoManager.Instance.SetupArduinoManager();
            ArduinoManager.Instance.StartReceivingData();
        }


        Debug.LogError("Trying to connect to arduino board...");

        yield return new WaitUntil(() => ArduinoManager.Instance.connected);
        
        GameManager.Instance.appIsReady = true;
       
 
    }

    public void Start()
    {
        foreach (StateBase state in states)
        {
            state.OnExitState();
        }
        
        ChangeState(currentState);
    }

    public void ChangeState(StatesEnum _newState)
    {
        nextState = _newState;

        if (_newState != currentState)
        {
            states[(int)currentState].OnExitState();
        }

        prevState = currentState;
        currentState = _newState;
        states[(int)currentState].OnEnterState();
    }

    public void Update()
    {
        if (states.Count > 0) {
            states[(int)currentState].UpdateState();
        }
    }

    public void BackScene()
    {
        
    }
    
    public void ResetState(StatesEnum state)
    {
        try {
            Debug.LogWarning("Reset " + state);
            states[(int)state].Reset();
        } catch(System.Exception e) {
            Debug.LogError(e);
        }
    }

}