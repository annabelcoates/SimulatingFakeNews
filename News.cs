using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAttemptWPF
{
    public class News
    {
        public string ID;
        public bool isTrue;
        public News(string ID, bool isTrue)
        {
            this.ID=ID;
            this.isTrue = isTrue;
        }
    }
}