using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Chat.ClientAPI.Base
{
    public interface IRestService
    {
        Task<TResponseType> MakeRequest<TResponseType>(Request request) where TResponseType : class;

        Task<HttpResponseMessage> MakeRequest(Request request);
    }
}
