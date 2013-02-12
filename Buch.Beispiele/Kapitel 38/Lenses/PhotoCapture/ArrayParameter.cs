using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Phone.Media.Capture;

namespace BlackWhiteImageStyler.PhotoCapture
{
    public abstract class ArrayParameter<T> : Parameter, IReadOnlyCollection<ArrayParameterOption<T>>
    {
        private bool _refreshing;
        private ArrayParameterOption<T> _selectedOption;

        protected ArrayParameter(string name, Guid parameterId, PhotoCaptureDevice device)
            : base(name, parameterId, device)
        {
            Options = new List<ArrayParameterOption<T>>();
            ReadCurrent();
        }

        protected List<ArrayParameterOption<T>> Options { get; private set; }

        public ArrayParameterOption<T> this[int index]
        {
            get { return Options[index]; }
        }

        public ArrayParameterOption<T> SelectedOption
        {
            get { return _selectedOption; }
            set
            {
                if (_selectedOption != value && value != null && !_refreshing)
                {
                    _selectedOption = value;

                    // neuen Wert auf der Kamera setzen
                    SetOption(value);
                }
            }
        }

        #region IReadOnlyCollection

        public int Count
        {
            get { return Options.Count; }
        }

        public IEnumerator<ArrayParameterOption<T>> GetEnumerator()
        {
            foreach (var option in Options)
            {
                yield return option;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public override sealed void ReadCurrent()
        {
            _refreshing = true;
            Options.Clear();
            _selectedOption = null;

            try
            {
                PopulateOptions();
                IsSupported = Options.Count > 0;
            }
            catch (Exception)
            {
                IsSupported = false;
                Debug.WriteLine("Einstellung {0} konnte nicht abgerufen werden.", Name);
            }

            IsModifiable = IsSupported && Options.Count > 1;

            _refreshing = false;
        }

        /// <summary>
        ///     Diese Methode ruft alle unterstützten Optionen für diesen Parameter ab.
        /// </summary>
        protected abstract void PopulateOptions();

        /// <summary>
        ///     Setzt den angegebenen Optionswert für diesen Kameraparameter.
        /// </summary>
        /// <param name="option"></param>
        protected abstract void SetOption(ArrayParameterOption<T> option);
    }
}