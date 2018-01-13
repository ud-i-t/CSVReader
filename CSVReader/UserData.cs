using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLF3
{
    public class UserData
    {
        private Dictionary<string, string> dataDic;

        public UserData()
        {
            dataDic = new Dictionary<string, string>();
        }

        public string this[string key] {
            get { return dataDic[key]; }
            private set { dataDic[key] = value; }
        }

        public void Add(string key, string data)
        {
            dataDic.Add(key, data);
        }

        public bool ContainsKey(string key)
        {
            return dataDic.ContainsKey(key);
        }
    }
}
