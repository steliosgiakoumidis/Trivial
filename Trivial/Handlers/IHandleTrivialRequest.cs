using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Trivial.DatabaseAccessLayer;
using Trivial.DataModels;

namespace Trivial.Handlers
{
    public interface IHandleTrivialRequest
    {
        Task<ResponseModel> Handle(RequestModel model, HttpClient client,
            IDatabaseAccess databaseAccess);
    }
}
