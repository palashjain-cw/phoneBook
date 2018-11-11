using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public interface IContactDetailsCache
    {
        List<CharCountMapping> GetContactCountByChar();
        List<ContactDetail> GetContactDetailByChar(char startingChar);
        void RefreshKeyByChar(char startingChar);
        void RefreshContactCountByChar();
    }
}
