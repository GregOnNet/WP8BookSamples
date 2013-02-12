using System;
using Windows.Phone.Media.Capture;

namespace BlackWhiteImageStyler.PhotoCapture
{
    /// <summary>
    /// Stellt einen Parameter der Kameraeinstellungen dar.
    /// </summary>
    public abstract class Parameter
    {
        protected Parameter(string name, Guid propertyId, PhotoCaptureDevice device)
        {
            Name = name;
            ParameterId = propertyId;
            Device = device;
        }

        /// <summary>
        /// Der Name des Parameters.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Ruft einen Wert ab, ob der Parameter unterstützt wird.
        /// </summary>
        public bool IsSupported { get; internal set; }

        /// <summary>
        /// Ruft einen Wert ab, ob der Parameter verändert werden kann.
        /// </summary>
        public bool IsModifiable { get; internal set; }

        /// <summary>
        /// Ruft die Kamera ab zu dieser Einstellung.
        /// </summary>
        public PhotoCaptureDevice Device { get; internal set; }

        /// <summary>
        /// Ruft die ID der Kameraeinstellung ab.
        /// </summary>
        public Guid ParameterId { get; protected set; }

        /// <summary>
        /// Liest den aktuellen Parameterwert von der Kamera.
        /// </summary>
        public abstract void ReadCurrent();

        /// <summary>
        /// Setzt den Standardwert für diesen Parameter auf der Kamera.
        /// </summary>
        public abstract void SetDefaultValue();
    }
}