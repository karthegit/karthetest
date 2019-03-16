using Ceb.MerlinTool.WebAPI.Services.Relay.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceb.MerlinTool.WebAPI.Services.Relay
{
    /// <summary>
    /// This class will be used to pass any data from BLL to API. If there is any exception or error during CoBRA Admin BLL-DAL / BLL-Service communication, then
    /// this class will represent the exact state and BLL will not further throw the exception to API.
    /// </summary>
    /// <typeparam name="T">Data to be tranported from BLL to API.</typeparam>
    public class Response<T>
    {
        private Exception _ex;
        private T _data;
        private ServiceStatusCode _status = ServiceStatusCode.Unknown;

        public Response() { }

        public Response(T data)
        {
            _data = data;
        }

        public string ErrorMessage { get; set; }
        public string Message { get; set; }

        public ServiceStatusCode Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public Exception Exception
        {
            get
            {
                if (Status != ServiceStatusCode.Failure)
                    _ex = null;
                return _ex;
            }
            set
            {
                _ex = value;
                if (_ex != null)
                    _status = ServiceStatusCode.Failure;
            }
        } 

        public T Data
        {
            get { return _data; }
            set
            {
                _data = value;
                if (_data != null)
                    Status = ServiceStatusCode.Success;
            }
        }
    }
    
}
