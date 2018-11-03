using AutoMapper;
using BL;
using DTOs;
using Entities;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PhoneBook.Controllers
{
    public class PhoneBookController : ApiController
    {
        [Route("api/Contact/")]
        [HttpPost]
        public IHttpActionResult AddContactDetails([FromBody] ContactDetailDTO contactDto)
        {
            try
            {
                if (contactDto == null)
                {
                    ModelState.AddModelError("Contact Detail", "contact Detail cannot be null");
                    return BadRequest(ModelState);
                }

                ContactDetailValidator ContactDetailvalidator = new ContactDetailValidator();
                ValidationResult result = ContactDetailvalidator.Validate(contactDto);
                if (!result.IsValid)
                {
                    foreach (var failure in result.Errors)
                    {
                        ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                    }
                    return BadRequest(ModelState);
                }

                Mapper.CreateMap<ContactDetailDTO, ContactDetail>();
                ContactDetail contactDetail = Mapper.Map<ContactDetail>(contactDto);
                ContactDetails conntactDetailBL = new ContactDetails();
                bool detailAdded = conntactDetailBL.AddContactDetail(contactDetail);
                if (detailAdded)
                    return Ok("Contact Detail Added Successfully");
                else
                    return Ok("Contact Detail cannot be added");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("api/Contact/{mobileNumber}")]
        [HttpDelete]
        public IHttpActionResult DeleteContactDetails(string mobileNumber)
        {
            try
            {
                ContactDetails conntactDetailBL = new ContactDetails();
                bool detailAdded = conntactDetailBL.DeleteContactDetail(mobileNumber);
                if (detailAdded)
                    return Ok("Contact Detail Deleted Successfully");
                else
                    return Ok("Contact Detail cannot be Deleted");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("api/Contact/")]
        [HttpGet]
        public IHttpActionResult GetAllContactDetails(int pageId = 1)
        {
            try
            {
                ContactDetails conntactDetailBL = new ContactDetails();
                IEnumerable<ContactDetail> contactDetails = conntactDetailBL.GetAllContactDetail(pageId);
                Mapper.CreateMap<ContactDetail, ContactDetailDTO>();
                IEnumerable<ContactDetailDTO> contactDetailDto = Mapper.Map<IEnumerable<ContactDetailDTO>>(contactDetails);
                if (contactDetailDto.Count() > 0)
                {
                    return Json(contactDetailDto);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            
            
        }
        [Route("api/Contact/")]
        [HttpPut]
        public IHttpActionResult UpdateContactDetails(string name, [FromBody] ContactDetailDTO updatedDetails)
        {
            try
            {
                if (updatedDetails == null)
                {
                    ModelState.AddModelError("Contact Detail", "contact Detail cannot be null");
                    return BadRequest(ModelState);
                }
                if (String.IsNullOrWhiteSpace(name))
                {
                    ModelState.AddModelError("Name", "Name to be updated cannot be null");
                }

                ContactDetailValidator ContactDetailvalidator = new ContactDetailValidator();
                ValidationResult result = ContactDetailvalidator.Validate(updatedDetails);
                if (!result.IsValid || !ModelState.IsValid)
                {
                    foreach (var failure in result.Errors)
                    {
                        ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                    }
                    return BadRequest(ModelState);
                }

                Mapper.CreateMap<ContactDetailDTO, ContactDetail>();
                ContactDetail contactDetail = Mapper.Map<ContactDetail>(updatedDetails);
                ContactDetails conntactDetailBL = new ContactDetails();
                bool detailAdded = conntactDetailBL.UpdateContactDetail(name, contactDetail);
                if (detailAdded)
                    return Ok("Contact Detail Updated Successfully");
                else
                    return Ok("Contact Detail cannot be Updated");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("api/Contact/search/")]
        [HttpGet]
        public IHttpActionResult SearchContactDetails(string searchString, int pageId = 1)
        {
            try
            {
                ContactDetails conntactDetailBL = new ContactDetails();
                IEnumerable<ContactDetail> searchedResult = conntactDetailBL.SearcheContactDetail(searchString, pageId);
                Mapper.CreateMap<ContactDetail, ContactDetailDTO>();
                IEnumerable<ContactDetailDTO> searchedContactDto = Mapper.Map<IEnumerable<ContactDetailDTO>>(searchedResult);
                if (searchedContactDto != null && searchedContactDto.Count() > 0)
                {
                    return Json(searchedContactDto);
                }
                else
                {
                    return Ok("No Match found");
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
