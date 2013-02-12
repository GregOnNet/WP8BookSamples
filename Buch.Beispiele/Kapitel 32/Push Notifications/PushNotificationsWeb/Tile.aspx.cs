using System;
using System.IO;
using System.Net;
using System.Text;

namespace PushNotificationsWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SendPushNotification(object sender,
            EventArgs e)
        {
            try
            {
                string subscriptionUri = UriTextBox.Text;
                HttpWebRequest notificationReq =
                    (HttpWebRequest)WebRequest
                    .Create(subscriptionUri);
                notificationReq.Method = "Post";

                string tileMessage = 
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<wp:Notification xmlns:wp=\"WPNotification\">" +
                    "<wp:Tile>" +
                        "<wp:BackgroundImage>"+
                            PictureUriTextBox.Text +
                        "</wp:BackgroundImage>" +
                        "<wp:Count>" +
                            CountTextBox.Text +
                        "</wp:Count>" +
                        "<wp:Title>" + 
                            TitleTextBox.Text +
                        "</wp:Title>" +
                        "<wp:BackBackgroundImage>" +
                            BackPictureUriTextBox.Text +
                        "</wp:BackBackgroundImage>" +
                        "<wp:BackContent>" +
                            BackContentTextBox.Text +
                        "</wp:BackContent>" +
                        "<wp:BackTitle>" +
                            BackTitleTextBox.Text +
                        "</wp:BackTitle>" +
                    "</wp:Tile> " +
                "</wp:Notification>";

                byte[] notificationMessageBytes =
                    Encoding.Default.GetBytes(tileMessage);

                notificationReq.ContentLength = 
                    notificationMessageBytes.Length;
                notificationReq.ContentType = "text/xml";
                notificationReq.Headers
                    .Add("X-WindowsPhone-Target", "token");
                notificationReq.Headers
                    .Add("X-NotificationClass", "1");
                
                using (Stream requestStream = 
                    notificationReq.GetRequestStream())
                {
                    requestStream.Write(
                        notificationMessageBytes,
                        0, 
                        notificationMessageBytes.Length);
                }

                HttpWebResponse response = 
                    (HttpWebResponse)
                    notificationReq.GetResponse();

                string notificationStatus = 
                    response.Headers
                    ["X-NotificationStatus"];

                string notificationChannelStatus = 
                    response.Headers
                    ["X-SubscriptionStatus"];

                string deviceConnectionStatus = 
                    response.Headers
                    ["X-DeviceConnectionStatus"];

                ResponseTextBox.Text =
                    notificationStatus + "|" +
                    notificationChannelStatus + "|" +
                    deviceConnectionStatus;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text =
                    "Es ist ein Fehler aufgetreten: " +
                    ex.Message;
            }
        }
    }
}
