
using System.Collections.Generic;
using System.Net;

namespace App.Models
{
    public class RespuestaModel<T>
    {
        public int Id { get; set; }
        public List<string> Message { get; set; }
        public HttpStatusCode Codigo { get; set; }
        public T Data { get; set; }
        public bool Success { get; set; }

        public RespuestaModel()
        {
            Id = 0;
            Message = new List<string>();
            Codigo = HttpStatusCode.OK;
            Data = default(T);
            Success = true;
        }
    }

    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

}
