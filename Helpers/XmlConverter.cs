using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RestoAppAPI.Helpers
{
    public static class XmlConverter
    {
       

     public  static string GenerateXml<T>(this List<T> items)
    {
        Type itemType = typeof(T);
        PropertyInfo[] properties = itemType.GetProperties();

        StringBuilder xml = new StringBuilder();

        foreach (T item in items)
        {
            xml.Append( "<DetailsRow>");
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(item);
                xml.Append(  $"    <{property.Name}>{value}</{property.Name}>");
            }
            xml.Append("</DetailsRow>");
        }
        return xml.ToString();
    }

    public  static string GenerateXml(this string Ids)
    {
         StringBuilder xml = new StringBuilder();
        xml.Append("<DetailsRow>");
        foreach (var Id in Ids.Split(','))
        {
            xml.Append($"<Id>{Id}</Id>");            
         
        }
           xml.Append( "</DetailsRow>");
        return xml.ToString();
    }

        
    }
}