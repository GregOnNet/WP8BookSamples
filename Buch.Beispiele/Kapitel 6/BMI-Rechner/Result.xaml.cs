using System.Windows.Media;
using Microsoft.Phone.Controls;

namespace BmiRechner
{
    public partial class Result : PhoneApplicationPage
    {
        public Result()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Daten auslesen von QueryString und casten

            string heightString = this.NavigationContext.QueryString["Height"];
            double height = double.Parse(heightString);

            string weightString = this.NavigationContext.QueryString["Weight"];
            double weight = double.Parse(weightString);

            // BMI berechnen
            double bmi = CalcBmi(weight, height);

            // Ausgeben
            Output(bmi);
        }

        private double CalcBmi(double weight, double height)
        {
            // BMI = Gewicht / Größe²
            return weight / (height * height);
        }

        private void Output(double bmi)
        {
            // Berechneten BMI-Index mit einer Nachkommastelle ausgeben
            this.BmiOutput.Text = string.Format("Ihr BMI beträgt: {0:F1}", bmi);

            // Erläuterungstext ausgeben
            if (bmi < 18.5)
            {
                // Untergewicht
                this.DescriptionOutput.Text = "Sie haben Untergewicht";
                this.DescriptionOutput.Foreground = new SolidColorBrush(Colors.Yellow);
            }
            else if (bmi < 25)
            {
                // Normalgewicht
                this.DescriptionOutput.Text = "Sie haben Normalgewicht";
                this.DescriptionOutput.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                // Übergewicht
                this.DescriptionOutput.Text = "Sie haben Übergewicht";
                this.DescriptionOutput.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}