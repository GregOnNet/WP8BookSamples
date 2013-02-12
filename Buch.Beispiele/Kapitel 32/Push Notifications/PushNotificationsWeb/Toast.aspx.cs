using System;
using System.IO;
using System.Net;
using System.Text;

namespace PushNotificationsWeb
{
    public partial class Toast : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SendPushNotification(object sender, EventArgs e)
        {
            try
            {
                string subscriptionUri = UriTextBox.Text;
                HttpWebRequest notificationReq =
                    (HttpWebRequest)WebRequest
                    .Create(subscriptionUri);
                notificationReq.Method = "Post";

                string toastMessage =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<wp:Notification xmlns:wp=\"WPNotification\">" +
                    "<wp:Toast>" +
                        "<wp:Text1>" + 
                            TitleTextBox.Text +
                        "</wp:Text1>" +
                        "<wp:Text2>" +
                            MessageTextBox.Text +
                        "</wp:Text2>" +
                    "</wp:Toast> " +
                "</wp:Notification>";

                byte[] notificationMessageBytes =
                    Encoding.Default.GetBytes(toastMessage);

                notificationReq.ContentLength =
                    notificationMessageBytes.Length;
                notificationReq.ContentType = "text/xml";
                notificationReq.Headers
                    .Add("X-WindowsPhone-Target", "toast");
                notificationReq.Headers
                    .Add("X-NotificationClass", "2");

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