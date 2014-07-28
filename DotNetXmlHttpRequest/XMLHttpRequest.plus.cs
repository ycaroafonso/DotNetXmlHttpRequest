using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotNetXmlHttpRequest
{
    public partial class XMLHttpRequest
    {
        public enum Header
        {
            ContentType,
            Cookie,
            Host,
            Referer,
            UserAgent
        }

        public void SetRequestHeader(Header header, string value)
        {
            switch (header)
            {
                case Header.ContentType:
                    _request.ContentType = value;
                    break;
                case Header.Cookie:
                    _request.Headers.Add("Cookie", value);
                    break;
                case Header.Host:
                    _request.Host = value;
                    break;
                case Header.Referer:
                    _request.Referer = value;
                    break;
                case Header.UserAgent:
                    _request.UserAgent = value;
                    break;
            }
        }


        public String ResponseText(Encoding enconding)
        {
            if (_pResponseText == string.Empty)
            {
                StreamReader responseReader = null;
                try
                {
                    if (_response.GetResponseStream() != null)
                    {
                        responseReader = new StreamReader(_response.GetResponseStream(), enconding);
                        _pResponseText = responseReader.ReadToEnd();
                    }
                    else
                        _pResponseText = string.Empty;
                }
                finally
                {
                    if (responseReader != null) responseReader.Close();
                }
            }
            return _pResponseText;
        }



        #region "Send"

        public void Send(Dictionary<string, string> parameter)
        {
            string str = string.Empty;
            foreach (KeyValuePair<string, string> item in parameter)
            {
                str = string.Concat(str, item.Key, "=", item.Value, "&");
            }
            Send(str.Trim('&'));
        }

        #endregion



        #region "other"

        public string ResponseContentType
        {
            get { return _response.ContentType; }
        }

        public string Url
        {
            get { return _pUri.AbsolutePath; }
        }

        public string Domain
        {
            get { return _pUri.Host; }
        }

        public Stream ResponseStream
        {
            get { return _response.GetResponseStream(); }
        }

        #endregion
    }
}
