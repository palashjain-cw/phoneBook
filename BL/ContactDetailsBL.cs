using Cache;
using DAL;
using Entities;
using Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace BL
{
    public class ContactDetailsBL : IContactDetailsBL
    {
        private readonly IContactDetailsCache _contactDetailsCache;
        private readonly IContactsDetailsDAL _contactDetailsDAL;
        public ContactDetailsBL(IContactDetailsCache contactDetailsCache, IContactsDetailsDAL contactDetailsDAL)
        {
            _contactDetailsCache = contactDetailsCache;
            _contactDetailsDAL = contactDetailsDAL;
        }
        private int _pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
        public bool AddContactDetail(ContactDetail contactDetail)
        {
            try
            {
                ContactsDAL contactDAl = new ContactsDAL();
                bool added =  contactDAl.AddContactDetail(contactDetail);
                if(added)
                {
                    _contactDetailsCache.RefreshKeyByChar(contactDetail.Name[0]);
                    _contactDetailsCache.RefreshContactCountByChar();
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
                    _contactDetailsCache.RefreshKeyByChar(Name[0]);
                    _contactDetailsCache.RefreshContactCountByChar();
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


                List<ContactDetail> contactDetail = _contactDetailsCache.GetContactDetailByChar(charCount[index].InitialChar);
                contactDetail = contactDetail.Skip(recordsFrom - prevCount).ToList();
                
                while(contactDetail.Count < _pageSize && index < (charCount.Count-1))
                {
                    int currentCount = contactDetail.Count;
                    List<ContactDetail> nextIndexContactDetail = _contactDetailsCache.GetContactDetailByChar(charCount[index+1].InitialChar);
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
                    _contactDetailsCache.RefreshKeyByChar(name[0]);
                    if(updatedDetails.Name[0] != name[0])
                    {
                        _contactDetailsCache.RefreshKeyByChar(updatedDetails.Name[0]);
                        _contactDetailsCache.RefreshContactCountByChar();
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
                return _contactDetailsDAL.SearchContactDetail(searchString, pageId);
            }
            catch (Exception)
            {
                return null;
            }
        }
        private List<CharCountMapping> GetContactCountByChar()
        {
            return _contactDetailsCache.GetContactCountByChar();
        }
    }
}
