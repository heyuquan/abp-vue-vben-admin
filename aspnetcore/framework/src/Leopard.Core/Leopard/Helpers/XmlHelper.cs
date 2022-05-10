using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Leopard.Helpers
{
    // xml文档的操作类封装 (文档操作、节点操作)
    // https://www.cnblogs.com/Can-daydayup/p/16058817.html

    // 介绍：xml特性如何与xml文件对应的文章，以及去掉命名空间的一些方法
    // https://wenku.baidu.com/view/f6df3f39ff4ffe4733687e21af45b307e871f98a.html


    // xmlns:前缀="命名空间"
    // XML命名空间详解
    // https://blog.csdn.net/yi412/article/details/70158876

    /// <summary>
    /// xml帮助类
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// Converts to xml string and returns
        /// </summary>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            string result = string.Empty;
            Encoding encode = Encoding.UTF8;
            try
            {
                using (MemoryStream output = new MemoryStream())
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                    serializer.Serialize(output, obj);
                    output.Seek(0, SeekOrigin.Begin);
                    result = encode.GetString(output.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("对象序列化成xml时发⽣错误：" + ex.Message);
            }
            return result;
        }


        /// <summary>
        /// 序列化XML时不带默认的命名空间xmlns
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeNoneNamespaces(object obj)
        {
            string result = string.Empty;
            try
            {
                var xs = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                MemoryStream ms = new MemoryStream();
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");//Add an empty namespace and empty value
                xs.Serialize(ms, obj, ns);
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                result = sr.ReadToEnd();
                ms.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("对象序列化成xml时发⽣错误：" + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 序列化时不⽣成XML声明和XML命名空间
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeNoNamespacesNoXmlDeclaration(object obj)
        {
            string result = string.Empty;
            try
            {
                var xs = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                MemoryStream ms = new MemoryStream();
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;//不编写XML声明
                XmlWriter xmlWriter = XmlWriter.Create(ms, settings);
                ns.Add("", "");//Add an empty namespace and empty value
                xs.Serialize(xmlWriter, obj, ns);
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                result = sr.ReadToEnd();
                ms.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("对象序列化成xml时发⽣错误：" + ex.Message);
            }
            return result;
        }

        public static T Deserialize<T>(string content) where T : class
        {
            T result = null;
            Encoding encode = Encoding.UTF8;
            try
            {
                using (MemoryStream input = new MemoryStream(encode.GetBytes(content)))
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    result = serializer.Deserialize(input) as T;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("xml反序列化成对象时发⽣错误：" + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 特殊符号转换为转义字符
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static string XmlSpecialSymbolConvert(string xmlStr)
        {
            return xmlStr.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\'", "&apos;").Replace("\"", "&quot;");
        }

        #region 去掉XML文本中的命名空间，和字段命名空间的前缀

        // 示例
        //<?xml version = "1.0" encoding="utf-8"?>
        //<root xmlns = "dotnet" xmlns:w="wpf">
        //  <a>data in a</a>                
        //  <w:b>data in b</w:b>         
        //  <c xmlns = "silverlight" >
        //    <w:d>                             
        //      <e>data in e</e>              
        //    </w:d>
        //  </c>
        //</root>

        //输出：

        //<root>
        //  <a>data in a</a>                
        //  <b>data in b</b>         
        //  <c>
        //    <d>                             
        //      <e>data in e</e>              
        //    </d>
        //  </c>
        //</root>

        /// <summary>
        /// 去掉XML文本中的命名空间，和字段命名空间的前缀
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static string StripXmlNamespaces(string xmlStr)
        {
            return stripNS(XElement.Parse(xmlStr)).ToString();
        }

        private static XElement stripNS(XElement root)
        {
            return new XElement(
                root.Name.LocalName,
                root.HasElements ?
                    root.Elements().Select(el => stripNS(el)) :
                    (object)root.Value
            );
        }
        #endregion
    }
}
