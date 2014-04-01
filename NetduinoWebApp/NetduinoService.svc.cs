using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace NetduinoWebApp
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class NetduinoService
    {        
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public bool ShouldTurnOnLed()
        {
            return AppState.GetNextLedState();
        }

        [OperationContract]
        [WebInvoke(UriTemplate="ButtonPushed")]
        public void NetduinoButtonPushed()
        {
            AppState.IncrementButtonPushCount();
        }

        [OperationContract]
        [WebInvoke(UriTemplate="UpdateLedState?ledOn={ledOn}")]
        public void UpdateLedState(bool ledOn)
        {
            AppState.SetCurrentLedState(ledOn);
        }
    }
}
