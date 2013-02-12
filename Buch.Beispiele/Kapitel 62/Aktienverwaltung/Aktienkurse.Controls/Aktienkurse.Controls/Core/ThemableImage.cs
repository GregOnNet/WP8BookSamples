using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace Aktienkurse.Controls.Core
{
    public class ThemeableImage : Grid
    {
        private readonly Rectangle _rectangle;

        public ThemeableImage()
        {
            _rectangle = new Rectangle();

            var bgBrush = Application
                            .Current
                            .Resources["PhoneBackgroundBrush"]
                             as SolidColorBrush;

            if (bgBrush.Color == 
                            Color.FromArgb
                                  (0xFF, 0xFF, 0xFF, 0xFF))
            {
                _rectangle.Fill = 
                    new SolidColorBrush(Colors.Black);
            }
            else
            {
                _rectangle.Fill = 
                    new SolidColorBrush(Colors.White);
            }

            Children.Add(_rectangle);
        }

        public ImageSource Source
        {
            get
            {
                if (_rectangle.OpacityMask != null)
                {
                    return
                        (_rectangle.OpacityMask as ImageBrush)
                        .ImageSource;
                }
                
                return null;
            }
            set
            {
                _rectangle.OpacityMask = 
                    new ImageBrush { ImageSource = value };
            }
        }
    }
}
