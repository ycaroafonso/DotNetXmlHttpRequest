
using System.Collections.Generic;
using System.IO;

namespace DotNetXmlHttpRequest
{
    public partial class XMLHttpRequest
    {


        public void setRequestHeader(DotNetXmlHttpRequest.plus.Enums.Header header, string value)
        {
            switch (header)
            {
                case DotNetXmlHttpRequest.plus.Enums.Header.ContentType:
                    Request.ContentType = value;
                    break;
                case DotNetXmlHttpRequest.plus.Enums.Header.Cookie:
                    Request.Headers.Add("Cookie", value);
                    break;
                case DotNetXmlHttpRequest.plus.Enums.Header.Host:
                    Request.Host = value;
                    break;
                case DotNetXmlHttpRequest.plus.Enums.Header.Referer:
                    Request.Referer = value;
                    break;
                case DotNetXmlHttpRequest.plus.Enums.Header.UserAgent:
                    Request.UserAgent = value;
                    break;
            }
        }




        #region "Send"

        public void Send(Dictionary<string, string> parameter)
        {
            string str = string.Empty;
            foreach (KeyValuePair<string, string> item in parameter)
            {
                str = string.Concat(str, item.Key, "=", item.Value, "&");
            }
            Send(str.ToString().Trim('&'));
        }

        #endregion



        #region "other"

        public string ResponseContentType
        {
            get { return Response.ContentType; }
        }

        public string Url
        {
            get { return pUri.AbsolutePath; }
        }

        public string Domain
        {
            get { return pUri.Host; }
        }

        public Stream responseStream
        {
            get { return Response.GetResponseStream(); }
        }

        #endregion
    }
}


namespace DotNetXmlHttpRequest.plus
{
    public class Enums
    {
        public enum Header
        {
            Cookie,
            Referer,
            ContentType,
            Host,
            UserAgent
        }

    }
}