using ContactsApi.Data;
using ContactsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApi.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class ContactController : Controller
    {
        private readonly ContactApiDbContext contactApiDbContext;

        public ContactController(ContactApiDbContext contactApiDbContext)
        {
            this.contactApiDbContext = contactApiDbContext;
        }


        [HttpGet]
        public IActionResult GetContacts()
        {
            contactApiDbContext.Contacts.ToList();
            return Ok();
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetContact([FromRoute] Guid id)
        {
            var contact = contactApiDbContext.Contacts.Find(id);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = addContactRequest.Address,
                Email = addContactRequest.Email,
                FullName = addContactRequest.FullName,
                Phone = addContactRequest.Phone
            };
            await contactApiDbContext.Contacts.AddAsync(contact);
            await contactApiDbContext.SaveChangesAsync();

            return Ok(contact);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = contactApiDbContext.Contacts.Find(id);
            if (contact != null) 
            {
                contact.Email = updateContactRequest.Email;
                contact.FullName = updateContactRequest.FullName;
                contact.Phone = updateContactRequest.Phone;
                contact.Address = updateContactRequest.Address;

                contactApiDbContext.SaveChanges();

                return Ok(contact);
            }
            return NotFound();

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteContact([FromRoute] Guid id)
        {
            var contact = contactApiDbContext.Contacts.Find(id);
            if (contact != null)
            {
                contactApiDbContext.Remove(contact);
                contactApiDbContext.SaveChanges();
                return Ok(contact);
            }

            return NotFound();
        }

    }
}
