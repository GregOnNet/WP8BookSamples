using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Devices;
using Windows.Foundation;
using Windows.Phone.Media.Capture;

namespace BlackWhiteImageStyler.PhotoCapture
{
    public class PhoneCamera : IDisposable
    {
        private CameraCaptureSequence _captureSequence;
        private MemoryStream _captureStream;
        private bool _isCapturing;
        private bool _isFocused;
        private bool _isInitialized;

        public PhotoCaptureDevice Device { get; private set; }
        public ReadOnlyCollection<Parameter> Parameters { get; private set; }
        public byte[] ImageBytes { get; private set; }

        public void Dispose()
        {
            CameraButtons.ShutterKeyHalfPressed -= CameraButtons_ShutterKeyHalfPressed;
            CameraButtons.ShutterKeyPressed -= CameraButtons_ShutterKeyPressed;
            CameraButtons.ShutterKeyReleased -= CameraButtons_ShutterKeyReleased;

            if (Device != null)
                Device.Dispose();
            Device = null;

            if (_captureStream != null)
                _captureStream.Dispose();
            _captureStream = null;
        }

        public event EventHandler<FocusedEventArgs> Focused;

        protected void SendFocused(FocusedEventArgs e)
        {
            EventHandler<FocusedEventArgs> handler = Focused;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler PhotoCaptured;

        protected void SendPhotoCaptured()
        {
            EventHandler handler = PhotoCaptured;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized)
            {
                return;
            }

            IReadOnlyList<Size> availableCaptureResulotions =
                PhotoCaptureDevice.GetAvailableCaptureResolutions(CameraSensorLocation.Back);
            Device = await PhotoCaptureDevice.OpenAsync(CameraSensorLocation.Back, availableCaptureResulotions.First());
            
            InitParameters();
            Device.SetProperty(KnownCameraGeneralProperties.PlayShutterSoundOnCapture, true);

            _captureStream = new MemoryStream();
            _captureSequence = Device.CreateCaptureSequence(1);
            _captureSequence.Frames[0].CaptureStream = _captureStream.AsOutputStream();
            await Device.PrepareCaptureSequenceAsync(_captureSequence);

            CameraButtons.ShutterKeyHalfPressed += CameraButtons_ShutterKeyHalfPressed;
            CameraButtons.ShutterKeyPressed += CameraButtons_ShutterKeyPressed;
            CameraButtons.ShutterKeyReleased += CameraButtons_ShutterKeyReleased;

            _isInitialized = true;
        }

        private async void CameraButtons_ShutterKeyHalfPressed(object sender, EventArgs e)
        {
            await AutoFocusAsync();
        }

        private async void CameraButtons_ShutterKeyPressed(object sender, EventArgs e)
        {
            await CaptureAsync();
        }

        private void CameraButtons_ShutterKeyReleased(object sender, EventArgs e)
        {
            _isFocused = false;
            SendFocused(new FocusedEventArgs(CameraFocusStatus.NotLocked));
        }

        private void InitParameters()
        {
            var supportedParameters = new List<Parameter>();

            AddParameter(new SceneModeParameter(Device), supportedParameters);
            AddParameter(new CaptureResolutionParameter(Device), supportedParameters);
            AddParameter(new IsoParameter(Device), supportedParameters);
            AddParameter(new ExposureTimeParameter(Device), supportedParameters);
            AddParameter(new ExposureCompensationParameter(Device), supportedParameters);
            AddParameter(new FlashPowerParameter(Device), supportedParameters);

            Parameters = new ReadOnlyCollection<Parameter>(supportedParameters);
        }

        private void AddParameter(Parameter parameter, List<Parameter> parametersList)
        {
            if (parameter.IsSupported && parameter.IsModifiable)
            {
                try
                {
                    parameter.ReadCurrent();
                    parameter.SetDefaultValue();

                    parametersList.Add(parameter);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
        }

        public void ResetParameters()
        {
            foreach (Parameter parameter in Parameters)
            {
                parameter.SetDefaultValue();
            }
        }

        public async Task AutoFocusAsync()
        {
            if (!_isInitialized || !_isFocused && PhotoCaptureDevice.IsFocusSupported(Device.SensorLocation))
            {
                SendFocused(new FocusedEventArgs(CameraFocusStatus.NotLocked));

                CameraFocusStatus focusState = await Device.FocusAsync();
                _isFocused = focusState == CameraFocusStatus.Locked;

                SendFocused(new FocusedEventArgs(focusState));
            }
        }

        public async Task CaptureAsync()
        {
            // Status prüfen
            if (!_isInitialized)
            {
                return;
            }

            if (_isCapturing)
            {
                return;
            }

            _isCapturing = true;

            // Fokus prüfen
            if (!_isFocused)
            {
                await AutoFocusAsync();
            }

            // Bild aufnehmen
            await _captureSequence.StartCaptureAsync();

            // Aufgenommene Bytes in Eigenschaft speichern
            ImageBytes = _captureStream.ToArray();
            SendPhotoCaptured();

            // CaptureStream zurücksetzen für nächste Aufnahme
            _captureStream = new MemoryStream();
            _captureSequence.Frames[0].CaptureStream = _captureStream.AsOutputStream();

            _isCapturing = false;
            _isFocused = false;
        }
    }
}