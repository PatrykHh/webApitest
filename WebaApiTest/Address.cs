using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebaApiTest
{
    public class Address
    {
        private readonly string Url;

        public static readonly Address Get = new Address("/get");
        public static readonly Address Post = new Address("/post");
        public static readonly Address Put = new Address("/put");
        public static readonly Address Delete = new Address("/delete");

        public Address(string url)
        {
            Url = url;
        }

        public override string ToString()
        {
            return Url;
        }
    }
}
