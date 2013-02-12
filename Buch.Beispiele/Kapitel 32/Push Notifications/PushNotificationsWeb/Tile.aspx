<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Tile.aspx.cs" Inherits="PushNotificationsWeb._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" 
             runat="server" 
             ContentPlaceHolderID="MainContent">
    <h2>
        Tile Notifications
    </h2>
        <strong>Windows Live Notification Uri:
        </strong>
        <br />
        <asp:TextBox ID="UriTextBox"
        runat="server"
        Width="600px" />
        <br /><br />
        <strong>Titel</strong><br />
        <asp:TextBox ID="TitleTextBox" runat="server" />
        <br /><br />
        <strong>Anzahl der Neuigkeiten</strong><br />
        <asp:TextBox ID="CountTextBox" runat="server"/>
        <br /><br />
        <strong>Tile- Bild Uri
        </strong>
        <br />
        <asp:TextBox ID="PictureUriTextBox"
        runat="server" />
        <br /><br />
    --------------------------------------<br />
    <strong>| BACKGRUND INFORMATION |</strong><br />
    --------------------------------------<br />
        <strong>Backgrund Titel</strong><br />
        <asp:TextBox ID="BackTitleTextBox"
                     runat="server" />
        <br /><br />
        <strong>Backgrund Tile- Bild Uri</strong><br />
        <asp:TextBox ID="BackPictureUriTextBox"
                     runat="server" />
        <br /><br />
        <strong>BackgrundContent<br />
        <asp:TextBox ID="BackContentTextBox"
                     runat="server" />
        </strong>
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
