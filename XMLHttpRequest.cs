// License GNU LESSER GENERAL PUBLIC LICENSE - http://www.gnu.org/copyleft/lesser.html
// By Ycaro Afonso - http://ycaro.net/tag/dotnetxmlhttpRequest/

using System;
using System.Collections.Generic;

using System.Net;
using System.IO;

namespace DotNetXmlHttpRequest
{
    public partial class XMLHttpRequest : System.IDisposable
    {

        #region "variables"

        private string pSend = string.Empty;

        public Uri pUri;
        private EnumMethod pMethod;

        public HttpWebRequest Request;
        public HttpWebResponse Response = null;

        private int pTimeOut = 20000;

        private string pResponseText = string.Empty;

        #endregion


        public int TimeOut
        {
            get { return pTimeOut; }
            set { pTimeOut = value; }
        }

        public XMLHttpRequest() { }

        public enum States
        {
            UNSENT = 0,
            OPENED = 1,
            HEADERS_RECEIVED = 2,
            LOADING = 3,
            DONE = 4
        }

        public enum EnumMethod
        {
            POST,
            GET
        }

        public States readystate
        {
            get;
            private set;
        }

        #region "Open"

        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-open-method
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <remarks></remarks>
        public void Open(EnumMethod method, string url)
        {
            readystate = States.UNSENT;
            pMethod = method;
            pUri = new Uri(url);
            Request = (HttpWebRequest)WebRequest.Create(pUri);
            Request.Timeout = TimeOut;
        }

        public void Open(EnumMethod _Method, string _Url, string Username, string Password)
        {
            Open(_Method, _Url);
            Request.Credentials = new NetworkCredential(Username, Password);
        }

        #endregion

        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-setRequestheader-method
        /// </summary>
        /// <param name="header"></param>
        /// <remarks></remarks>
        public void setRequestHeader(string header, string value)
        {
            if (Request.Headers[header] == null)
            {
                Request.Headers.Add(header, value);
            }
            else
            {
                Request.Headers[header] = value;
            }
        }


        #region "Send"

        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-send-method
        /// </summary>
        /// <param name="Data"></param>
        /// <remarks></remarks>
        public void Send(string Data)
        {
            pSend = Data;
            pResponseText = string.Empty;
            try
            {
                if (pMethod == EnumMethod.POST)
                {
                    Request.Method = "POST";

                    byte[] _data = System.Text.Encoding.Default.GetBytes(pSend);
                    Request.ContentType = "application/x-www-form-urlencoded";
                    Request.ContentLength = Data.Length;
                    using (Stream newStream = Request.GetRequestStream())
                    {
                        newStream.Write(_data, 0, _data.Length);
                        newStream.Close();
                    }
                }

                Response = (HttpWebResponse)Request.GetResponse();


                readystate = States.DONE;


            }
            catch (WebException ex)
            {
                Response = (HttpWebResponse)ex.Response;
                readystate = States.DONE;

                if (Response == null)
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

        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-abort-method
        /// </summary>
        /// <remarks></remarks>
        public void Abort()
        {
            Request.Abort();
        }

        #region "Response"

        #region "Status"

        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-status-attribute
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Status
        {
            get { return (int)Response.StatusCode; }
        }

        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-statustext-attribute
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string StatusText
        {
            get { return Response.StatusDescription; }
        }

        #endregion

        #region "Header"

        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-getresponseheader-method
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string getResponseHeader(string header)
        {
            return string.Concat(header, ": ", Response.Headers[header]);
        }
        public string getResponseHeader(int header)
        {
            return string.Concat(Response.Headers.Keys[header], ": ", Response.Headers[header]);
        }


        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-getallresponseheaders-method
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string getAllResponseHeaders()
        {
            System.Text.StringBuilder retorna = new System.Text.StringBuilder();
            for (int x = 0; x <= Response.Headers.Count - 1; x++)
            {
                retorna.AppendFormat("{0}: {1}; ", Response.Headers.Keys[x], Response.Headers[x]);
            }
            return retorna.ToString();
        }

        #endregion

        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-responsetext-attribute
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public String responseText
        {
            get
            {
                if (pResponseText == string.Empty)
                    if (Response.ContentType.ToString().Split('/')[0].Split('/')[0] == "text")
                    {
                        System.IO.StreamReader responseReader = null;
                        try
                        {
                            if (Response.GetResponseStream() != null)
                            {
                                responseReader = new System.IO.StreamReader(Response.GetResponseStream());
                                pResponseText = responseReader.ReadToEnd();
                            }
                            else
                            {
                                pResponseText = string.Empty;
                            }
                            // Catch ex As Exception
                        }
                        finally
                        {
                            if (responseText != string.Empty)
                                responseReader.Close();
                        }
                    }
                return pResponseText;
            }
        }

        /// <summary>
        /// TODO: 
        /// http://www.w3.org/TR/XMLHttpRequest/#the-responsexml-attribute
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public object responseXML()
        {
            throw new Exception();
        }

        #endregion

        #region "IDisposable"

        // To detect redundant calls
        private bool disposedValue = false;

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    Response.GetResponseStream().Close();
                    Response.GetResponseStream().Dispose();
                    Response.Close();
                }

                // TODO: free your own state (unmanaged objects).
                // TODO: set large fields to null.
            }
            this.disposedValue = true;
        }

        #region " IDisposable Support "
        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #endregion

    }
}