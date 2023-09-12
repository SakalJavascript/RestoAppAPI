using System;
using System.Collections.Generic;
using System.Reflection;

namespace RestoAppAPI.Helpers
{
    public class XmlConverter
    {
       

     public  static string GenerateXml<T>(List<T> items)
    {
        Type itemType = typeof(T);
        PropertyInfo[] properties = itemType.GetProperties();

        string xml = "";

        foreach (T item in items)
        {
            xml += "<DetailsRow>";
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(item);
                xml += $"    <{property.Name}>{value}</{property.Name}>";
            }
            xml += "</DetailsRow>";
        }

    
        return xml;
    }

        
    }
}