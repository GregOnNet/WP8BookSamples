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

namespace HandleManipulationEvents
{
    public partial class MainPage : PhoneApplicationPage
    {
        //private Felder
        //aktuelle Koordinaten des Stackpanels während der Manipulation
        private double _currentX;
        private double _currentY;

        //breite und höhe des Containers, in dem Objekt bewegt wird
        private double _containerHeight;
        private double _containerWidth;

        //deklarieren einer Transformgroup, die als Container für mehrere Transformationen dienen kann
        private TransformGroup _transformGroup;
        private TranslateTransform _translation;

        //Farbdefinitionen für unterschiedliche Zustände der Manipulation
        private SolidColorBrush _manipulationInProgress;
        private SolidColorBrush _manipulationCompleted;

        // Konstruktor
        public MainPage()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //erstellen der Instanzen für die Transformation
            _transformGroup = new TransformGroup();
            _translation = new TranslateTransform();

            //initialisieren der  Höhe und Breite des Containers
            _containerHeight = canvasDragBoard.ActualHeight;
            _containerWidth = canvasDragBoard.ActualWidth;

            //Startkoordinaten setzen
            _currentX = 0;
            _currentY = 0;

            _transformGroup.Children.Add(_translation);
            this.stackForDrag.RenderTransform = _transformGroup;

            //definieren der Farben für unterschiedliche Zuständes der Manipulation
            _manipulationInProgress = new SolidColorBrush();
            _manipulationInProgress.Color = Colors.Orange;

            _manipulationCompleted = new SolidColorBrush();
            _manipulationCompleted.Color = Colors.Green;
        }

        void PhoneApplicationPage_ManupualtionStarted(object sender, ManipulationStartedEventArgs e)
        {
            //Ändern der Kantenfarbe und des Textes, um Bewegung des Stackpanels zu visualisieren
            this.borderInStack.BorderBrush = _manipulationInProgress;
            this.stateDescription.Text = "Bewegung!";
        }

        //Behandeln der Manipulationsdaten, die während der des Ziehens des Stackpanels auftreten
        void PhoneApplicationPage_ManupualtionDelta(object sender, ManipulationDeltaEventArgs e)
        {
            e.Handled = true;

            //aktuelle Koordinaten des Stackpanels aktualisieren
            _currentX += e.DeltaManipulation.Translation.X;
            _currentY += e.DeltaManipulation.Translation.Y;


            //ermitteln wo sich die Ränder des bewegten Stackpanels befinden
            double rightCorner = _currentX + stackForDrag.ActualWidth;
            double leftCorner = _currentX;
            double bottomCorner = _currentY + stackForDrag.ActualHeight;
            double upperCorner = _currentY;

            //prüfen, ob Stackpanels außerhalb seines Containers gezogen wurde
            if (rightCorner <= _containerWidth && bottomCorner <= _containerHeight &&
                leftCorner >= 0 && upperCorner >= 0)
            {
                //stackpanel in X-Richtung bewegen
                _translation.X += e.DeltaManipulation.Translation.X;
                _translation.Y += e.DeltaManipulation.Translation.Y;
            }
            else
            {
                //Stackpanel auf den Startpunkt zurücksetzen
                _translation.X = stackForDrag.RenderTransformOrigin.X;
                _translation.Y = stackForDrag.RenderTransformOrigin.Y;

                //aktuelle Position zurücksetzen
                _currentX = 0;
                _currentY = 0;
                
                //setzen des Status, dass Stackpanel außerhalb seines Containers gezogen wurde
                e.Complete();
            }

            //Ausgabe der aktuellen Koordinaten des Stackpanels
            txt_mouseX.Text = _currentX.ToString();
            txt_mouseY.Text = _currentY.ToString();
        }

        //Methode, die nach Beendigung der Manipulation ausgeführt wird.
        void PhoneApplicationPage_ManupualtionCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            this.borderInStack.BorderBrush = _manipulationCompleted;
            this.stateDescription.Text = "Angekommen!";
        }
   }
}