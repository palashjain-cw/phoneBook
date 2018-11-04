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
        [Route("api/Contacts/")]
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
                ContactDetailsBL conntactDetailBL = new ContactDetailsBL();
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

        [Route("api/Contacts/{name}")]
        [HttpDelete]
        public IHttpActionResult DeleteContactDetails(string name)
        {
            try
            {
                ContactDetailsBL conntactDetailBL = new ContactDetailsBL();
                bool detailAdded = conntactDetailBL.DeleteContactDetail(name);
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

        [Route("api/Contacts/")]
        [HttpGet]
        public IHttpActionResult GetAllContactDetails(int page = 1)
        {
            try
            {
                ContactDetailsBL conntactDetailBL = new ContactDetailsBL();
                IEnumerable<ContactDetail> contactDetails = conntactDetailBL.GetAllContactDetail(page);
                if (contactDetails != null && contactDetails.Count() > 0)
                {
                    Mapper.CreateMap<ContactDetail, ContactDetailDTO>();
                    IEnumerable<ContactDetailDTO> contactDetailDto = Mapper.Map<IEnumerable<ContactDetailDTO>>(contactDetails);
                    return Json(contactDetailDto);
                }
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            
            
        }
        [Route("api/Contacts/")]
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
                ContactDetailsBL conntactDetailBL = new ContactDetailsBL();
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

        [Route("api/Contacts/search/")]
        [HttpGet]
        public IHttpActionResult SearchContactDetails(string str, int pageId = 1)
        {
            try
            {
                ContactDetailsBL conntactDetailBL = new ContactDetailsBL();
                IEnumerable<ContactDetail> searchedResult = conntactDetailBL.SearcheContactDetail(str, pageId);
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
