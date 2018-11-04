using Cache;
using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace BL
{
    public class ContactDetailsBL
    {
        private int _pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
        public bool AddContactDetail(ContactDetail contactDetail)
        {
            try
            {
                ContactsDAL contactDAl = new ContactsDAL();
                bool added =  contactDAl.AddContactDetail(contactDetail);
                if(added)
                {
                    ContactDetailsCache contactDetailCache = new ContactDetailsCache();
                    contactDetailCache.RefreshKeyByChar(contactDetail.Name[0]);
                    contactDetailCache.RefreshContactCountByChar();
                }
                return added;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteContactDetail(string Name)
        {
            try
            {
                ContactsDAL contactDAl = new ContactsDAL();
                bool deleted=  contactDAl.DeleteContactDetail(Name);
                if (deleted)
                {
                    ContactDetailsCache contactDetailCache = new ContactDetailsCache();
                    contactDetailCache.RefreshKeyByChar(Name[0]);
                    contactDetailCache.RefreshContactCountByChar();
                }
                return deleted;
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
                List<CharCountMapping> charCount = GetContactCountByChar();
                int recordsFrom = (pageId - 1) * _pageSize;
                int count = 0, prevCount = 0, index = -1;
                
                foreach(var mapping in charCount)
                {
                    index++;
                    prevCount = count;
                    count += mapping.Total;
                    if(count > recordsFrom)
                    {
                        break;
                    }
                }

                if (count <= recordsFrom) return null;

                ContactDetailsCache contactDetailCache = new ContactDetailsCache();

                List<ContactDetail> contactDetail = contactDetailCache.GetContactDetailByChar(charCount[index].InitialChar);
                contactDetail = contactDetail.Skip(recordsFrom - prevCount).ToList();
                
                while(contactDetail.Count < _pageSize && index < (charCount.Count-1))
                {
                    int currentCount = contactDetail.Count;
                    List<ContactDetail> nextIndexContactDetail = contactDetailCache.GetContactDetailByChar(charCount[index+1].InitialChar);
                    int remainingCount = _pageSize - currentCount;
                    if (remainingCount < nextIndexContactDetail.Count)
                    {
                        contactDetail.AddRange(nextIndexContactDetail.Take(remainingCount));
                    }
                    else
                    {
                        contactDetail.AddRange(nextIndexContactDetail);
                    }
                    index++;
                }

                return contactDetail;
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
                bool updated=  contactDAl.UpdateContactDetail(name, updatedDetails);
                
                if (updated)
                {
                    ContactDetailsCache contactDetailCache = new ContactDetailsCache();
                    contactDetailCache.RefreshKeyByChar(name[0]);
                    if(updatedDetails.Name[0] != name[0])
                    {
                        contactDetailCache.RefreshKeyByChar(updatedDetails.Name[0]);
                        contactDetailCache.RefreshContactCountByChar();
                    }
                }
                return updated;
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
        private List<CharCountMapping> GetContactCountByChar()
        {
            ContactDetailsCache contactDetailCache = new ContactDetailsCache();
            return contactDetailCache.GetContactCountByChar();

        }
    }
}
