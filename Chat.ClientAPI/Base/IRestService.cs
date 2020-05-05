using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.ClientAPI.Base
{
    public interface IRestService
    {
        Task<TResponseType> MakeRequest<TResponseType>(Request request);
    }
}
