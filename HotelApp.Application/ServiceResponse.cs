using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application
{
    public class ServiceResponse<T> where T : class
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public T? Data { get; set; }
        public object? AdditionalData { get; set; } 

        public ServiceResponse(bool _success, string _message, T _Data, object _additionalData = null)
        {
            Success = _success;
            Message = _message;
            Data = _Data;
            AdditionalData = _additionalData; 
        }

        public ServiceResponse(bool _success, string _message)
        {
            Success = _success;
            Message = _message;
            Data = null;
            AdditionalData = null; 
        }

        public static ServiceResponse<T> ResponseSuccess(string message = "You Data Saved Successfully", T Data = null, object additionalData = null)
        {
            return new ServiceResponse<T>(true, message, Data, additionalData);
        }

        public static ServiceResponse<T> ResponseFailure(string message = "An error occurred", T Data = null, object additionalData = null)
        {
            return new ServiceResponse<T>(false, message, Data, additionalData);
        }
    }
}
