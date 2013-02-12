using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Runtime.Serialization;

namespace ISDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void WriteFile_Click(object sender, RoutedEventArgs e)
        {
            this.WriteFile("Text.txt", this.WriteContent.Text);

            var bart = new Person() { FirstName = "Bart", Age = 17 };
            this.SerializeObject(bart, "Bart.ser");
            var p2 = this.DeserializeObject<Person>("Bart.ser");
        }

        private void ReadFile_Click(object sender, RoutedEventArgs e)
        {
            this.ReadContent.Text = this.ReadFile("Text.txt");

            
        }

        private void WriteFile(string file, string text)
        {
            var iso = IsolatedStorageFile.GetUserStoreForApplication();
            var fs = iso.OpenFile(file, FileMode.Create);
            var wr = new StreamWriter(fs, Encoding.UTF8);

            wr.Write(text);

            wr.Dispose();
            fs.Dispose();
            iso.Dispose();
        }

        private string ReadFile(string file)
        {
            var iso = IsolatedStorageFile.GetUserStoreForApplication();
            var fs = iso.OpenFile(file, FileMode.OpenOrCreate);
            var r = new StreamReader(fs, Encoding.UTF8);

            string text = r.ReadToEnd();
            
            r.Dispose();
            fs.Dispose();
            iso.Dispose();

            return text;
        }

        private void SerializeObject<T>(T obj, string file)
        {
            var iso = IsolatedStorageFile.GetUserStoreForApplication();
            var fs = iso.OpenFile(file, FileMode.Create);

            var ser = new DataContractSerializer(typeof(T));
            ser.WriteObject(fs, obj);

            fs.Close();
            iso.Dispose();
        }

        private T DeserializeObject<T>(string file)
        {
            // Benötigte Objekte erzeugen
            var iso = IsolatedStorageFile.GetUserStoreForApplication();
            var fs = iso.OpenFile(file, FileMode.OpenOrCreate);

            T result;

            try
            {
                // Deserialisieren
                var ser = new DataContractSerializer(typeof(T));
                result = (T)ser.ReadObject(fs);
            }
            catch (Exception)
            {
                // Bei Fehler Standardwert des Typs setzen
                result = default(T);
            }

            // Ressourcen freigeben
            fs.Close();
            iso.Dispose();

            return result;
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public int Age { get; set; }
    }
}