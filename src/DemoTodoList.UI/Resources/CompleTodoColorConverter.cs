using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DemoTodoList.UI.Resources
{
   public class CompletedTodoColorConverterr : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if((bool)value)
            {
                return Color.LightGreen;
            }
            return Color.Transparent;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (((Color)value)== Color.LightGreen)
            {
                return true;
            }

            return false;
        }
    }
}
