using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchibstedBackendTest.EFModels
{
    public class User
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string roles { get; set; }
        public string password { get; set; }
    }
}
