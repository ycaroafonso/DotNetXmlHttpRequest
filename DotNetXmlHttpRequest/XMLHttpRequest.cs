// License GNU LESSER GENERAL PUBLIC LICENSE - http://www.gnu.org/copyleft/lesser.html
// By Ycaro Afonso - http://ycaro.net/tag/dotnetxmlhttpRequest/

using System;
using System.Net;
using System.IO;
using System.Text;

namespace DotNetXmlHttpRequest
{
    public partial class XMLHttpRequest : IXMLHttpRequest
    {

        #region "variables"

        private Uri _pUri;
        private EnumMethod _pMethod;

        private HttpWebRequest _request;
        private HttpWebResponse _response;

        private int _pTimeOut = 20000;

        private string _pResponseText = string.Empty;

        #endregion

        public int TimeOut
        {
            get { return _pTimeOut; }
            set { _pTimeOut = value; }
        }

        public enum States
        {
            Unsent = 0,
            Opened = 1,
            HeadersReceived = 2,
            Loading = 3,
            Done = 4
        }

        public enum EnumMethod
        {
            Post,
            Get
        }

        public States Readystate
        {
            get;
            private set;
        }

        #region "Open"


        public void Open(EnumMethod method, string url)
        {
            Readystate = States.Unsent;
            _pMethod = method;
            _pUri = new Uri(url);
            _request = (HttpWebRequest)WebRequest.Create(_pUri);
            _request.Timeout = TimeOut;
        }

        public void Open(EnumMethod method, string url, string username, string password)
        {
            Open(method, url);
            _request.Credentials = new NetworkCredential(username, password);
        }

        #endregion

        public void SetRequestHeader(string header, string value)
        {
            if (_request.Headers[header] == null)
            {
                _request.Headers.Add(header, value);
            }
            else
            {
                _request.Headers[header] = value;
            }
        }


        #region "Send"

        public void Send(string data)
        {
            _pResponseText = string.Empty;
            try
            {
                if (_pMethod == EnumMethod.Post)
                {
                    _request.Method = "POST";

                    byte[] dataArray = System.Text.Encoding.Default.GetBytes(data);
                    _request.ContentType = "application/x-www-form-urlencoded";
                    _request.ContentLength = data.Length;
                    using (Stream newStream = _request.GetRequestStream())
                    {
                        newStream.Write(dataArray, 0, dataArray.Length);
                        newStream.Close();
                    }
                }

                _response = (HttpWebResponse)_request.GetResponse();


                Readystate = States.Done;
            }
            catch (WebException ex)
            {
                _response = (HttpWebResponse)ex.Response;
                Readystate = States.Done;

                if (_response == null)
                {
                    throw new Exception("Ocorreu um erro no Request que não é um HTTP ERROR");
                    // TODO: 
                }
            }
        }

        public void Send()
        {
            Send(string.Empty);
        }

        #endregion

        public void Abort()
        {
            _request.Abort();
        }

        #region "Response"

        #region "Status"

        public int Status
        {
            get { return (int)_response.StatusCode; }
        }

        public string StatusText
        {
            get { return _response.StatusDescription; }
        }

        #endregion

        #region "Header"

        public string GetResponseHeader(string header)
        {
            return string.Concat(header, ": ", _response.Headers[header]);
        }
        public string GetResponseHeader(int header)
        {
            return string.Concat(_response.Headers.Keys[header], ": ", _response.Headers[header]);
        }


        public string GetAllResponseHeaders()
        {
            StringBuilder retorna = new StringBuilder();
            for (int x = 0; x <= _response.Headers.Count - 1; x++)
            {
                retorna.AppendFormat("{0}: {1}; ", _response.Headers.Keys[x], _response.Headers[x]);
            }
            return retorna.ToString();
        }

        #endregion


        public string ResponseText()
        {
            if (_pResponseText == string.Empty)
            {
                StreamReader responseReader = null;
                try
                {
                    if (_response.GetResponseStream() != null)
                    {
                        responseReader = new StreamReader(_response.GetResponseStream());
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

        public object ResponseXML
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion


        public void Dispose()
        {
            var responseStream = _response.GetResponseStream();
            if (responseStream != null) responseStream.Close();

            var stream = _response.GetResponseStream();
            if (stream != null) stream.Dispose();

            _response.Close();
        }
    }
}