<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Raw.aspx.cs" Inherits="PushNotificationsWeb.Raw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2"
             ContentPlaceHolderID="MainContent"
             runat="server">
    <h2>
        Raw Notifications
    </h2>

    <strong>Windows Live Notification Uri: </strong>
    <br />
    <asp:TextBox 
            ID="UriTextBox" 
            runat="server"
            Width="600px" />
    <br /><br />

    <strong>Wert1:</strong>
    <br />
    <asp:TextBox 
            ID="Value1TextBox" 
            runat="server" />
    <br /><br />

    <strong>Wert2: </strong>
    <br />
    <asp:TextBox 
            ID="Value2TextBox" 
            runat="server"/>
    <br /><br />

    <asp:Button ID="SendPushNotificationButton"
                    runat="server" 
                    OnClick="SendPushNotification"
                    Text="Nachricht senden" />
    <br /><br />
    <strong>Serverantwort:<br />
    </strong>
    <asp:TextBox ID="ResponseTextBox"
                 runat="server">
    </asp:TextBox>
</asp:Content>
