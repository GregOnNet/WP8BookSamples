<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Toast.aspx.cs" Inherits="PushNotificationsWeb.Toast" %>

<asp:Content ID="Content1"
             ContentPlaceHolderID="HeadContent" 
             runat="server">
</asp:Content>
<asp:Content ID="Content2"
             ContentPlaceHolderID="MainContent" 
             runat="server">
    <h2>
        Toast Notifications
    </h2>

    <strong>Windows Live Notification Uri: </strong>
    <br />
    <asp:TextBox 
            ID="UriTextBox" 
            runat="server"
            Width="600px" />
    <br /><br />

    <strong>Titel:</strong>
    <br />
    <asp:TextBox 
            ID="TitleTextBox" 
            runat="server" />
    <br /><br />

    <strong>Nachricht: </strong>
    <br />
    <asp:TextBox 
            ID="MessageTextBox" 
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
