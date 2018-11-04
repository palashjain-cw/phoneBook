using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    [Serializable]
    public class CharCountMapping
    {
        public char InitialChar { get; set; }
        public int Total { get; set; }
    }
}
