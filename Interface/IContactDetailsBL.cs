using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public interface IContactDetailsBL
    {
        bool AddContactDetail(ContactDetail contactDetail);
        bool DeleteContactDetail(string Name);
        IEnumerable<ContactDetail> GetAllContactDetail(int pageId);
        bool UpdateContactDetail(string name, ContactDetail updatedDetails);
        IEnumerable<ContactDetail> SearcheContactDetail(string searchString, int pageId);
    }
}
