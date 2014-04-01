using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetduinoWebApp
{
    /// <summary>
    /// a totally lazy simple class to keep track of the app status at runtime
    /// </summary>
    public static class AppState
    {
        const string NextLedState = "SetLedState";
        const string CurrentLedState = "CurrentLedState";
        const string ButtonPushCount = "ButtonPushCount";

        public static void SetNextLedState(bool turnOn)
        {
            HttpContext.Current.Application[NextLedState] = turnOn;
        }

        public static bool GetNextLedState()
        {
            if (HttpContext.Current.Application[NextLedState] == null)
            {
                return false;
            }
            else
            {
                return (bool)HttpContext.Current.Application[NextLedState];
            }
        }

        
        public static bool GetCurrentLedState()
        {
            if (HttpContext.Current.Application[CurrentLedState] == null)
            {
                return false;
            }
            else
            {
                return (bool)HttpContext.Current.Application[CurrentLedState];
            }
        }
        
        public static void SetCurrentLedState(bool ledOn)
        {
            HttpContext.Current.Application[CurrentLedState] = ledOn;
        }


        public static void IncrementButtonPushCount()
        {
            if (HttpContext.Current.Application[ButtonPushCount] == null)
            {
                HttpContext.Current.Application[ButtonPushCount] = 1;
            }
            else
            {
                HttpContext.Current.Application[ButtonPushCount] = (int)HttpContext.Current.Application[ButtonPushCount] + 1;
            }
        }

        public static int GetButtonPushCount()
        {
            if (HttpContext.Current.Application[ButtonPushCount] == null)
            {
                return 0;
            }
            else
            {
                return (int)HttpContext.Current.Application[ButtonPushCount];
            }
        }
    }
}