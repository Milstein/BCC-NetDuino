<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NetduinoWebApp.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Simple Netduino Web app</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>Netduino web controller</h1>
    <div>
        <asp:Button ID="TurnOnButton" runat="server" Text="Turn On LED" onclick="TurnOnButton_Click" />
        <asp:Button ID="TurnOffButton" runat="server" Text="Turn Off LED" onclick="TurnOffButton_Click" />
    </div>
    <div>LED State: <%:LedState%></div>
    <div>Button press count: <%:ButtonPressCount %></div>
        
    </div>
    </form>
</body>
</html>
