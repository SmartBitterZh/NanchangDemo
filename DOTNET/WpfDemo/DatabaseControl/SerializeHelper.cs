using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace DBControl
{
  public sealed class SerializeHelper
  {
    public static string JsonSerializer<T>(T t)
    {
      DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
      using (MemoryStream ms = new MemoryStream())
      {
        ser.WriteObject(ms, t);
        string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        return jsonString;
      }
    }

    /// <summary>
    /// JSON反序列化
    /// </summary>
    public static T JsonDeserialize<T>(string jsonString)
    {
      DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
      using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
      {
        T obj = (T)ser.ReadObject(ms);
        return obj;
      }
    }

    public static string XMLSerializer<T>(T t)
    {
      XmlSerializer formatter = new XmlSerializer(typeof(T));
      using (MemoryStream stream = new MemoryStream())
      {
        formatter.Serialize(stream, t);
        string result = System.Text.Encoding.UTF8.GetString(stream.ToArray());//转换成xml字符串
        return result;
      }
    }

    /// <summary>
    /// JSON反序列化
    /// </summary>
    public static T XMLDeserialize<T>(string xmlString)
    {
      XmlSerializer formatter = new XmlSerializer(typeof(T));
      using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
      {
        //XmlReader xmlReader = new XmlTextReader(ms);
        T obj = (T)formatter.Deserialize(stream);
        return obj;
      }
    }


    public static string SoapSerialize<T>(T t)
    {
      SoapFormatter formatter = new SoapFormatter();
      using (MemoryStream stream = new MemoryStream())
      {
        formatter.Serialize(stream, t);
        string result = System.Text.Encoding.UTF8.GetString(stream.ToArray());//转换成xml字符串
        return result;
      }
    }

    public static T SoapDeSerialize<T>(string soapString)
    {
      SoapFormatter formatter = new SoapFormatter();
      using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(soapString)))
      {
        //XmlReader xmlReader = new XmlTextReader(ms);
        T obj = (T)formatter.Deserialize(stream);
        return obj;
      }
    }

    public static string BinarySerialize<T>(T t)
    {
      BinaryFormatter formatter = new BinaryFormatter();
      using (MemoryStream stream = new MemoryStream())
      {
        formatter.Serialize(stream, t);
        string result = System.Text.Encoding.UTF8.GetString(stream.ToArray());//转换成xml字符串
        return result;
      }
    }
    public static T BinaryDeSerialize<T>(string binaryString)
    {
      BinaryFormatter formatter = new BinaryFormatter();
      using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(binaryString)))
      {
        //XmlReader xmlReader = new XmlTextReader(ms);
        T obj = (T)formatter.Deserialize(stream);
        return obj;
      }
    }
  }
}
