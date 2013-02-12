using System;
using Windows.Phone.Media.Capture;

namespace BlackWhiteImageStyler.PhotoCapture
{
    public class FocusedEventArgs : EventArgs
    {
        public FocusedEventArgs(CameraFocusStatus state)
        {
            State = state;
        }

        public CameraFocusStatus State { get; private set; }
    }
}