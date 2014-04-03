using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

// had to add references to System.Http, System.IO, SecretLabs.NetMF.Hardware.AnalogInput


namespace BCCIoT
{
    public class Program
    {
        const string WebServiceUrlBase = "http://192.168.1.3/NetduinoWebApp/NetduinoService.svc";
        const string ShouldTurnOnLedUrl = WebServiceUrlBase + "/ShouldTurnOnLed";
        const string UpdateLedStateUrl = WebServiceUrlBase + "/UpdateLedState?ledOn=";
        const string ButtonPushedUrl = WebServiceUrlBase + "/ButtonPushed";

        static InterruptPort onboardButton;

        
        public static void Main()
        {
            // handy little debugging method for out of memory tracking
            Debug.EnableGCMessages(true);

            //WhatIsMyIP();

            //FlashLed();
            //ReadButtonState();
            //FlashLedWithTimer();
            //ReadVoltage();
            //ReadVoltageWithRange();

            // launch a thread to see if we should change the LED state
            var ledControllerThread = new Thread(new ThreadStart(LedControllerThread));
            ledControllerThread.Start();

            // register an event for the button push
            onboardButton = new InterruptPort(Pins.ONBOARD_SW1, true, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeLow);
            onboardButton.OnInterrupt += OnboardButton_OnInterrupt;            
        }

        static void WhatIsMyIP()
        {
            string ipAddress = Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].IPAddress;
            Debug.Print(ipAddress);
        }

        static void FlashLedWithTimer()
        {
            var onboardLed = new OutputPort(Pins.ONBOARD_LED, false);
            var flasherEvent = new Timer(ToggleLed, onboardLed, new TimeSpan(0, 0, 10), new TimeSpan(0, 0, 1));

            while (true)
            {
                Thread.Sleep(1000);
            }
        }
        
        static void ToggleLed(object state)
        {
            OutputPort ledPort = (OutputPort)state;

            ledPort.Write(!ledPort.Read());
            
        }

        static void ReadButtonState()
        {
            var button = new InputPort(Pins.ONBOARD_SW1, true, Port.ResistorMode.Disabled);
            while (true)
            {
                bool pressed = button.Read();
                Debug.Print("pressed = " + pressed);
                Thread.Sleep(1000);
            }
        }

        static void ReadVoltage()
        {
            var analogInput = new SecretLabs.NETMF.Hardware.AnalogInput(Pins.GPIO_PIN_A0);
            while (true)
            {
                // this will give us a reading of a percentage of 3.3 v (0 - 1023 since 10 bit ADC)
                int reading = analogInput.Read();
                
                double voltage = (3.3 / 1023) * (double)reading;
                Debug.Print("Current reading = " + voltage);

                Thread.Sleep(1000);
            }           
        }

        static void ReadVoltageWithRange()
        {
            var analogInput = new SecretLabs.NETMF.Hardware.AnalogInput(Pins.GPIO_PIN_A0);
            analogInput.SetRange(0, 3300);
            while (true)
            {                
                int reading = analogInput.Read();
                double voltage = (double)reading / 1000;
                Debug.Print("Current reading = " + voltage);

                Thread.Sleep(1000);
            }
        }


        static void FlashLed()
        {
            var outputPort = new OutputPort(Pins.GPIO_PIN_D0, false);
            while (true)
            {
                outputPort.Write(!outputPort.Read());
                Thread.Sleep(1000);
            }
        }

        static void OnboardButton_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            Debug.Print("got interrupt: data1 = " + data1 + ", data2 = " + data2);

            PostUrl(ButtonPushedUrl, string.Empty);
        }

        static void LedControllerThread()
        {
            OutputPort ledPin = new OutputPort(Pins.ONBOARD_LED, false);
            while (true)
            {
                // check every few seconds if we should change the LED state
                Thread.Sleep(2000);
                
                string nextLedState = GetUrl(ShouldTurnOnLedUrl);
                Debug.Print(nextLedState);

                if (nextLedState == "true")
                {
                    ledPin.Write(true);
                }
                else
                {
                    ledPin.Write(false);
                }

                // and tell the server what our current led state is
                string updateUrl = UpdateLedStateUrl;
                if (ledPin.Read())
                {
                    updateUrl += "true";
                }
                else
                {
                    updateUrl += "false";
                }

                PostUrl(updateUrl, string.Empty);
            }
        }

        static string GetUrl(string url)
        {
            var request = HttpWebRequest.Create(url);
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using(var responseStream = response.GetResponseStream())
                {
                    using( var streamReader = new StreamReader(responseStream) )
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }

        static string PostUrl(string url, string data)
        {
            var request = HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            byte[] postData = System.Text.Encoding.UTF8.GetBytes(data);
            request.ContentLength = postData.Length;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(postData, 0, postData.Length);                
            }

            using (var responseStream = request.GetResponse().GetResponseStream())
            {                
                using (var streamReader = new StreamReader(responseStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
