using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace LaserGRBL.AddIn.QrCode
{

    public static class ImageHelper
    {
        public static void SetComments(this Image image, string comments)
        {
            var propertyItem = (PropertyItem)Activator.CreateInstance(typeof(PropertyItem), BindingFlags.Instance | BindingFlags.NonPublic, null, new object[0], CultureInfo.InvariantCulture);
            propertyItem.Id = 0x9c9c;
            propertyItem.Type = 1;
            propertyItem.Value = Encoding.Unicode.GetBytes(comments);
            propertyItem.Len = propertyItem.Value.Length;
            image.SetPropertyItem(propertyItem);
        }

        public static string GetComments(this Image image)
        {
            PropertyItem item = image.GetPropertyItem(0x9c9c);
            return Encoding.Unicode.GetString(item.Value);
        }

        public static void SetCopyright(this Image image)
        {
            var propertyItem = (PropertyItem)Activator.CreateInstance(typeof(PropertyItem), BindingFlags.Instance | BindingFlags.NonPublic, null, new object[0], CultureInfo.InvariantCulture);
            propertyItem.Id = 0x8298;
            propertyItem.Type = 2;
            propertyItem.Value = Encoding.ASCII.GetBytes("LaserGRBL");
            propertyItem.Len = propertyItem.Value.Length;
            image.SetPropertyItem(propertyItem);
        }

        public static string GetCopyright(this Image image)
        {
            PropertyItem item = image.GetPropertyItem(0x8298);
            return Encoding.ASCII.GetString(item.Value);
        }

    }
}
