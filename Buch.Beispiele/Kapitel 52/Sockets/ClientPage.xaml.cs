using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace TcpCommunication
{
    public partial class ClientPage
    {
        private const string Port = "4832";

        public ClientPage()
        {
            InitializeComponent();
        }

        private async void StartReceiving_Click(object sender, RoutedEventArgs e)
        {
            Status.Text = "Verbinde mit Server...";

            // Socket erzeugen und mit Server anhand der IP verbinden
            var socket = new StreamSocket();
            await socket.ConnectAsync(new HostName(IpAddress.Text), Port);
            Status.Text = "Mit Server verbunen. Empfange Bild...";

            // Bild empfangen
            ReceiveImage(socket);
        }
        
        private async void ReceiveImage(StreamSocket socket)
        {
            // DataReader erzeugen, um arbeiten mit Bytes zu vereinfachen
            var reader = new DataReader(socket.InputStream);

            // Anzahl der Bytes abrufen, aus denen das Bild besteht
            // Anzahl = int = 4 Bytes => 4 Bytes vom Socket laden
            await reader.LoadAsync(4);
            int imageSize = reader.ReadInt32();

            // Bytearray des Bildes laden
            await reader.LoadAsync((uint)imageSize);
            byte[] imageBytes = new byte[imageSize];
            reader.ReadBytes(imageBytes);
            
            // Bytearray in Stream laden und anzeigen
            using (var ms = new MemoryStream(imageBytes))
            {
                var image = new BitmapImage();
                image.SetSource(ms);

                ReceivedImage.Source = image;
            }
            Status.Text = "Bild empfangen.";

            // Ressourcen freigeben
            reader.Dispose();
            socket.Dispose();
        }
    }
}