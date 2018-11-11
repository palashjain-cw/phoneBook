using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
   public interface IContactsDetailsDAL
    {
        bool AddContactDetail(ContactDetail contactDetail);
        bool DeleteContactDetail(string name);
        List<ContactDetail> GetAllContactDetail(char startingChar);
        bool UpdateContactDetail(string name, ContactDetail updatedDetail);
        IEnumerable<ContactDetail> SearchContactDetail(string searchString, int pageId);
        List<CharCountMapping> GetContactCountByChar();
    }
}
