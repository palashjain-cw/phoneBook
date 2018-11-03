using DAL;
using Entities;
using System;
using System.Collections.Generic;

namespace BL
{
    public class ContactDetails
    {
        public bool AddContactDetail(ContactDetail contactDetail)
        {
            try
            {
                ContactsDAL contactDAl = new ContactsDAL();
                return contactDAl.AddContactDetail(contactDetail);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteContactDetail(string mobileNumber)
        {
            try
            {
                ContactsDAL contactDAl = new ContactsDAL();
                return contactDAl.DeleteContactDetail(mobileNumber);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public IEnumerable<ContactDetail> GetAllContactDetail(int pageId)
        {
            try
            {
                
                ContactsDAL contactDAl = new ContactsDAL();
                return contactDAl.GetAllContactDetail(pageId);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool UpdateContactDetail(string name , ContactDetail updatedDetails)
        {
            try
            {
                ContactsDAL contactDAl = new ContactsDAL();
                return contactDAl.UpdateContactDetail(name, updatedDetails);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<ContactDetail> SearcheContactDetail(string searchString, int pageId)
        {
            try
            {
                ContactsDAL contactDAl = new ContactsDAL();
                return contactDAl.SearchContactDetail(searchString, pageId);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
