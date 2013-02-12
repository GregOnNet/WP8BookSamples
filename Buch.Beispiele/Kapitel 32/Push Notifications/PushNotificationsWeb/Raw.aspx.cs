using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;

namespace PushNotificationsWeb
{
    public partial class Raw : System.Web.UI.Page
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

                string rawMessage =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<root>" +
                    "<wp:Value1>" +
                        Value1TextBox.Text +
                    "</wp:Value1>" +
                    "<wp:Value2>" +
                        Value2TextBox.Text +
                    "</wp:Value2>" +
                 "</root> ";

                byte[] notificationMessageBytes =
                    Encoding.Default.GetBytes(rawMessage);

                notificationReq.ContentLength =
                    notificationMessageBytes.Length;
                notificationReq.ContentType = "text/xml";
                notificationReq.Headers
                    .Add("X-NotificationClass", "3");

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