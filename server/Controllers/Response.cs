using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server_mvc.Controllers
{
    public class Response
    {
        public string User { get; set; }
        public List<string> Autorizacoes { get; set; }
    }
}