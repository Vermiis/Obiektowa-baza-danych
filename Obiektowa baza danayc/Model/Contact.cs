using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obiektowa_baza_danayc.Model
{
    public class Contact
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surename { get; set; }
        public Address Address { get; set; }
        public List<Phone> PhoneNumbers { get; set; }
    }
}
