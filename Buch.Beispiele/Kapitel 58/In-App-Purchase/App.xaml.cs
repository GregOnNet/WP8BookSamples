using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MockIAPLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Navigation;

namespace InAppPurchase
{
    public partial class App : Application
    {
        /// <summary>
        /// Bietet einen einfachen Zugriff auf den Stammframe der Phone-Anwendung.
        /// </summary>
        /// <returns>Der Stammframe der Phone-Anwendung.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Konstruktor für das Application-Objekt.
        /// </summary>
        public App()
        {
            // Globaler Handler für nicht abgefangene Ausnahmen.
            UnhandledException += Application_UnhandledException;

            // Standard-XAML-Initialisierung
            InitializeComponent();

            // Phone-spezifische Initialisierung
            InitializePhoneApplication();
            
            // Während des Debuggens Profilerstellungsinformationen zur Grafikleistung anzeigen.
            if (Debugger.IsAttached)
            {
                // Zähler für die aktuelle Bildrate anzeigen.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Bereiche der Anwendung hervorheben, die mit jedem Bild neu gezeichnet werden.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Nicht produktiven Visualisierungsmodus für die Analyse aktivieren,
                // in dem Bereiche einer Seite angezeigt werden, die mit einer Farbüberlagerung an die GPU übergeben wurden.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Verhindert, dass der Bildschirm im Debugger ausgeschaltet wird, indem
                // die Leerlauferkennung der Anwendung deaktiviert wird.
                // Vorsicht: Nur im Debugmodus verwenden. Eine Anwendung mit deaktivierter Benutzerleerlauferkennung wird weiterhin ausgeführt
                // und verbraucht auch dann Akkuenergie, wenn der Benutzer das Handy nicht verwendet.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            SetupInAppMock();
        }
        
        [Conditional("DEBUG")]
        private void SetupInAppMock()
        {
            MockIAP.Init();

            MockIAP.RunInMockMode(true);

            MockIAP.SetListingInformation(1, "de-DE", "Verändere Bilder mit Filter", "1,99 €", "Image Filters");
            var streamInfo = GetResourceStream(new Uri("Assets/InAppMockProducts.xml", UriKind.Relative));
            using (var reader = new StreamReader(streamInfo.Stream))
            {
                string xml = reader.ReadToEnd();
                MockIAP.PopulateIAPItemsFromXml(xml);
            }
        }

        // Code, der beim Starten der Anwendung ausgeführt werden soll (z. B. über "Start")
        // Dieser Code wird beim Reaktivieren der Anwendung nicht ausgeführt
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code, der ausgeführt werden soll, wenn die Anwendung aktiviert wird (in den Vordergrund gebracht wird)
        // Dieser Code wird beim ersten Starten der Anwendung nicht ausgeführt
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code, der ausgeführt werden soll, wenn die Anwendung deaktiviert wird (in den Hintergrund gebracht wird)
        // Dieser Code wird beim Schließen der Anwendung nicht ausgeführt
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code, der beim Schließen der Anwendung ausgeführt wird (z. B. wenn der Benutzer auf "Zurück" klickt)
        // Dieser Code wird beim Deaktivieren der Anwendung nicht ausgeführt
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code, der bei einem Navigationsfehler ausgeführt wird
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // Navigationsfehler. Unterbrechen und Debugger öffnen
                Debugger.Break();
            }
        }

        // Code, der bei nicht behandelten Ausnahmen ausgeführt wird
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // Eine nicht behandelte Ausnahme ist aufgetreten. Unterbrechen und Debugger öffnen
                Debugger.Break();
            }
        }

        #region Initialisierung der Phone-Anwendung

        // Doppelte Initialisierung vermeiden
        private bool phoneApplicationInitialized = false;

        // Fügen Sie keinen zusätzlichen Code zu dieser Methode hinzu
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Frame erstellen, aber noch nicht als RootVisual festlegen. Dadurch kann der Begrüßungsbildschirm
            // aktiv bleiben, bis die Anwendung bereit für das Rendern ist.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Navigationsfehler behandeln
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Behandeln Sie Rücksetzanforderungen zum Löschen des Backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Sicherstellen, dass keine erneute Initialisierung erfolgt
            phoneApplicationInitialized = true;
        }

        // Fügen Sie keinen zusätzlichen Code zu dieser Methode hinzu
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Visuelle Stammkomponente festlegen, sodass die Anwendung gerendert werden kann
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Dieser Handler wird nicht mehr benötigt und kann entfernt werden
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // Wenn die App eine 'Reset'-Navigation empfangen hat, müssen wir prüfen
            // bei der nächsten Navigation, um festzustellen, ob der Seitenstapel zurückgesetzt werden muss
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Heben Sie die Registrierung des Ereignisses auf, damit es nicht erneut aufgerufen wird.
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Löschen Sie den Stapel nur bei den Navigationen "neu" (vorwärts) und "Aktualisieren"
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // Löschen Sie zur Sicherstellung der UI-Konsistenz den gesamten Seitenstapel
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // unternehmen Sie nichts
            }
        }

        #endregion

    }
}