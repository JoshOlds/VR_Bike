using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class bikeController : MonoBehaviour {

    public string ComString;
    public SerialPort sp;
    public float deadZone = 0.2f;
    public float LeftMax = 0.0f;
    public float RightMax = 0.0f;
    public bool calibrationMode;

    public static float Speed;
    public static float Heading;

    [SerializeField]
    private float headingCenter = 0.0f;

    void Awake()
    {
        sp = new SerialPort(ComString, 115200, Parity.None, 8, StopBits.One);
    }

    //Setup parameters to connect to Arduino
    
    public static string strIn;

    // Use this for initialization
    void Start()
    {
        OpenConnection();
    }

    void Update()
    {
        //Read incoming data
        strIn = sp.ReadLine();
        broadcastData(strIn);
        //print(strIn);
        //You can also send data like this
        //sp.Write("1");

    }

    //Function connecting to Arduino
    public void OpenConnection()
    {
        if (sp != null)
        {
            if (sp.IsOpen)
            {
                sp.Close();
                print("Closing port, because it was already open!");
            }
            else
            {
                sp.Open();  // opens the connection
                sp.ReadTimeout = 50;  // sets the timeout value before reporting error
                print("Port Opened!");
            }
        }
        else
        {
            if (sp.IsOpen)
            {
                print("Port is already open");
            }
            else
            {
                print("Port == null");
            }
        }
    }

    void OnApplicationQuit()
    {
        sp.Close();
    }

    float lastSpeed = 0;
    void broadcastData(string serialString)
    {
        try
        {
            float speed = 0;
            float heading = 0;
            string[] tempArr = serialString.Split(',');
            if (tempArr.Length != 2) return;

            float inSpeed = float.Parse(tempArr[0]);
            float inHeading = float.Parse(tempArr[1]);

            if (headingCenter == 0.0f)
            {
                headingCenter = inHeading;
            }

            if (inSpeed > 50000)
            {
                speed = 0;
            }
            else
            {
                speed = Mathf.Abs(inSpeed - 50000);
                speed = speed - 25000;
                speed = speed / 20000;
                if (speed > 1.0f)
                {
                    speed = 1.0f;
                }
            }

            float offsetHeading = inHeading - headingCenter;
            heading = offsetHeading;// / 60;

            if ((heading < deadZone && heading > 0) || (heading > -deadZone && heading < 0))
            {
                heading = 0.0f;
            }
            if (!calibrationMode)
            {
                if (heading < 0)//Adjustment for compass maximums
                {
                    heading = heading / LeftMax;
                }
                if (heading > 0) //Adjustment for compass maximums
                {
                    heading = heading / RightMax;
                }

            }
            if (heading < -1.0f)
            {
                heading = -1.0f;
            }
            if (heading > 1.0f)
            {
                heading = 1.0f;
            }

            //Speed = speed;
            var targetSpeed = inSpeed / 200000;
            Speed = (1 - targetSpeed) / (1 + targetSpeed);

            Heading = heading;

             if (calibrationMode)
             {
                 print(heading);
             }
            //print(heading);
        }
        catch
        {
            Debug.Log("Exception in broadcast Data!");
            return;
        }


    }


}
