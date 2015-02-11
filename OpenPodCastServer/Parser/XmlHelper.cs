using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace OpenPodCastServer.Parser
{
    public static class XmlHelper
    {
        public static XElement Parse(string xmlText)
        {
            const int limit = 10;
            var error= "";
            var cError = "";
            var count = 0;
            do
            {
                cError = error;
                try
                {
                    return XElement.Parse(xmlText);
                }
                catch (XmlException ex)
                {
                    error = string.Format("{0}#{1}", ex.LineNumber, ex.LinePosition);
                    xmlText = NormalizeXml(xmlText, ex.LineNumber);
                }
                count++;
            } while (error != cError && count <= limit);

            return null;
        }

        private static string NormalizeXml(string xmlText, long linenumber)
        {
            var readStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlText));
            var writeStream = new MemoryStream();
            var strmwriter = new StreamWriter(writeStream) { AutoFlush = true };
            var strm = new StreamReader(readStream);
            long i = 0;

            string strline;
            var strreplace = " ";

            while (i < linenumber - 1)
            {
                strline = strm.ReadLine();
                strmwriter.WriteLine(strline);
                i = i + 1;
            }

            strline = strm.ReadLine();

            if (strline != null)
            {
                var lineposition = strline.IndexOf("&", StringComparison.Ordinal);
                if (lineposition > 0)
                {
                    strreplace = "&amp;";
                }
                else
                {
                    lineposition = strline.IndexOf("<", 1, StringComparison.Ordinal);
                    if (lineposition > 0)
                    {
                        strreplace = "<";
                    }

                }
                strline = strline.Substring(0, lineposition - 1) + strreplace + strline.Substring(lineposition + 1);
            }
            strmwriter.WriteLine(strline);

            strline = strm.ReadToEnd();
            strmwriter.WriteLine(strline);

            strm.Close();
            
            writeStream.Position = 0;
            var sr = new StreamReader(writeStream);

            return sr.ReadToEnd();
        }
    }
}