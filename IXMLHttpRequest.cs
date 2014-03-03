using System;

namespace DotNetXmlHttpRequest
{
    interface IXMLHttpRequest : IDisposable
    {
        int TimeOut { get; set; }

        #region "Open"

        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-open-method
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <remarks></remarks>
        void Open(XMLHttpRequest.EnumMethod method, string url);

        void Open(XMLHttpRequest.EnumMethod method, string url, string username, string password);

        #endregion

        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-setRequestheader-method
        /// </summary>
        /// <param name="header"></param>
        /// <param name="value"> </param>
        /// <remarks></remarks>
        void SetRequestHeader(string header, string value);


        #region "Send"

        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-send-method
        /// </summary>
        /// <param name="data"></param>
        /// <remarks></remarks>
        void Send(string data);

        void Send();

        #endregion

        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-abort-method
        /// </summary>
        /// <remarks></remarks>
        void Abort();

        #region "Response"

        #region "Status"

        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-status-attribute
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        int Status { get; }

        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-statustext-attribute
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        string StatusText { get; }

        #endregion

        #region "Header"

        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-getresponseheader-method
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        string GetResponseHeader(string header);

        string GetResponseHeader(int header);


        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-getallresponseheaders-method
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        string GetAllResponseHeaders();

        #endregion

        /// <summary>
        /// http://www.w3.org/TR/XMLHttpRequest/#the-responsetext-attribute
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        string ResponseText();

        /// <summary>
        /// TODO: 
        /// http://www.w3.org/TR/XMLHttpRequest/#the-responsexml-attribute
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        object ResponseXML { get; }

        #endregion

        #region "IDisposable"

        #endregion
    }
}
