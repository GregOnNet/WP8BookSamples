namespace BlackWhiteImageStyler.PhotoCapture
{
    /// <summary>
    ///     Speichert einen einzelnen Wert für einen <see cref="ArrayParameter" />
    /// </summary>
    /// <typeparam name="T">Der Typ, den ein Optionswert annehmen soll.</typeparam>
    public class ArrayParameterOption<T>
    {
        public ArrayParameterOption(T value, string name)
        {
            Value = value;
            Name = name;
        }

        /// <summary>
        ///     Der Wert für diese Option.
        /// </summary>
        public T Value { get; internal set; }

        /// <summary>
        ///     Der Name für diese Option.
        /// </summary>
        public string Name { get; internal set; }
    }
}