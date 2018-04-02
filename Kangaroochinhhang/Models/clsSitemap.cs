using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace Kangaroochinhhang.Models
{
    public class clsSitemap
    {
          static string urlparent = "http://Kangaroochinhhang.com/";
        public static void CreateSitemap(string urls, string ids,string types)
        {

            XmlDocument xmlEmloyeeDoc = new XmlDocument();
            xmlEmloyeeDoc.Load(System.Web.HttpContext.Current.Server.MapPath("~/sitemap.xml"));
            XmlElement url = xmlEmloyeeDoc.CreateElement("url");
            XmlElement id = xmlEmloyeeDoc.CreateElement("id");
            id.InnerText = ids;
            XmlElement type = xmlEmloyeeDoc.CreateElement("type");
            type.InnerText = types;
            XmlElement loc = xmlEmloyeeDoc.CreateElement("loc");
            loc.InnerText = urlparent + urls;
            url.AppendChild(loc);
            url.AppendChild(id);
            url.AppendChild(type);
            xmlEmloyeeDoc.DocumentElement.AppendChild(url);
            xmlEmloyeeDoc.Save(System.Web.HttpContext.Current.Server.MapPath("~/sitemap.xml")); 
        }
        public static void UpdateSitemap(string urls, string ids,string types)
        {

            XmlDocument xmlEmloyeeDoc = new XmlDocument();
            DataSet ds = new DataSet();
            ds.ReadXml(System.Web.HttpContext.Current.Server.MapPath("~/sitemap.xml"));
            xmlEmloyeeDoc.Load(System.Web.HttpContext.Current.Server.MapPath("~/sitemap.xml"));
            int dem = 0;
             for (int i = 0; i < xmlEmloyeeDoc.ChildNodes.Count; i++)
            {
                try { 
                 if (ds.Tables[0].Rows[i]["id"].ToString() == ids && ds.Tables[0].Rows[i]["type"].ToString()==types)
                {
                     ds.Tables[0].Rows[i]["loc"] = urlparent + urls;
                     ds.WriteXml(System.Web.HttpContext.Current.Server.MapPath("~/sitemap.xml"));
                }
                 else
                 {
                     if(dem<1)
                     { 
                     dem += 1;
                     XmlElement url = xmlEmloyeeDoc.CreateElement("url");
                     XmlElement id = xmlEmloyeeDoc.CreateElement("id");
                     id.InnerText = ids;
                     XmlElement type = xmlEmloyeeDoc.CreateElement("type");
                     type.InnerText = types;
                     XmlElement loc = xmlEmloyeeDoc.CreateElement("loc");
                     loc.InnerText = urlparent + urls;
                     url.AppendChild(loc);
                     url.AppendChild(id);
                     url.AppendChild(type);
                     xmlEmloyeeDoc.DocumentElement.AppendChild(url);
                     xmlEmloyeeDoc.Save(System.Web.HttpContext.Current.Server.MapPath("~/sitemap.xml"));
                     }
                 }
                    }
                catch(Exception ex)
                {
                    CreateSitemap(urls, ids, types);
                }

            }
           
           
        }
        public static void DeteleSitemap(string ids,string types)
        {
            XmlDocument doc = new XmlDocument();
            DataSet ds = new DataSet();
            ds.ReadXml(System.Web.HttpContext.Current.Server.MapPath("~/sitemap.xml"));
            doc.Load(System.Web.HttpContext.Current.Server.MapPath("~/sitemap.xml"));
            XmlNodeList nodes = doc.SelectNodes("//url");
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                if (ds.Tables[0].Rows[i]["id"].ToString() == ids && ds.Tables[0].Rows[i]["type"].ToString() == types)
                {
                    nodes[i].ParentNode.RemoveChild(nodes[i]);
                }

            }
            doc.Save(System.Web.HttpContext.Current.Server.MapPath("~/sitemap.xml"));
        }
    }
}