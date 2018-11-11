using DAL;
using Entities;
using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Cache
{
    public class ContactDetailsCache : IContactDetailsCache
    {
        private readonly IContactsDetailsDAL _contactDetailsDAL;

        public ContactDetailsCache (IContactsDetailsDAL contactDAL)
        {
            _contactDetailsDAL = contactDAL;
        }
        
        public List<CharCountMapping> GetContactCountByChar()
        {
            return CacheManager.GetFromCache<List<CharCountMapping>>("charMappingCount_" + "v1", TimeSpan.FromDays(1),
                               () => _contactDetailsDAL.GetContactCountByChar());
        }

        public List<ContactDetail> GetContactDetailByChar(char startingChar)
        {
            return CacheManager.GetFromCache<List<ContactDetail>>("AllContactsByChar_" + + startingChar +"_v1", TimeSpan.FromDays(1),
                               () => _contactDetailsDAL.GetAllContactDetail(startingChar));
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
