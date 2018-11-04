using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Cache
{
    public class ContactDetailsCache
    {
        ContactsDAL DAL = new ContactsDAL();
        public List<CharCountMapping> GetContactCountByChar()
        {
            return CacheManager.GetFromCache<List<CharCountMapping>>("charMappingCount_" + "v1", TimeSpan.FromDays(1),
                               () => DAL.GetContactCountByChar());
        }

        public List<ContactDetail> GetContactDetailByChar(char startingChar)
        {
            return CacheManager.GetFromCache<List<ContactDetail>>("AllContactsByChar_" + + startingChar +"_v1", TimeSpan.FromDays(1),
                               () => DAL.GetAllContactDetail(startingChar));
        }

        public void RefreshKeyByChar(char startingChar)
        {
            CacheManager.ExpireCache("AllContactsByChar_" + +startingChar + "_v1");
        }
        public void RefreshContactCountByChar()
        {
            CacheManager.ExpireCache("charMappingCount_" + "v1");
        }
    }
}
