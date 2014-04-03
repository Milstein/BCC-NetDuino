using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NetduinoWebApp
{
    public partial class Default : System.Web.UI.Page
    {
        protected string LedState
        {
            get;
            set;
        }

        protected int ButtonPressCount
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LedState = AppState.GetCurrentLedState() ? "On":"Off";
            ButtonPressCount = AppState.GetButtonPushCount();
        }

        protected void TurnOnButton_Click(object sender, EventArgs e)
        {
            AppState.SetNextLedState(true);
        }

        protected void TurnOffButton_Click(object sender, EventArgs e)
        {
            AppState.SetNextLedState(false);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (Page.IsPostBack)
            {
                Response.Redirect(Request.UrlReferrer.ToString());
            }

            base.OnPreRender(e);
        }

    }
}