using System;
using EFAPITestAPI.JobModule.v1.Model;

namespace EFAPITestAPI.JobModule.v1.Client
{
    public class ClientResult<T>
    {
        public T Data { get; set; }
        public ApiError Error { get; set; }
        public bool Success { get { return Error == null; } }
    }
}