using ContactManager.Dtos;
using ContactManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactDBcontext _contactManager;
        public ContactController(ContactDBcontext contactManager)
        {
            _contactManager = contactManager;
        }
        [HttpGet]
        [Route("Contacts")]
        public ActionResult<List<Contact>> GetContactList()
        {
            var q = _contactManager.People.ToList();
            List<Contact> lst = new List<Contact>();
            foreach (var item in q)
            {
                lst.Add(new Contact {Email=item.Email,Family=item.Family,Id=item.Id,Name=item.Name,Tell=item.Tell });
            }
            return lst;
        }
        [HttpGet]
        [Route("Contacts/{searchTxt}")]
        public ActionResult<List<Contact>> GetContactSearch(string searchTxt)
        {
            var q = from c in _contactManager.People
                            where 
                            EF.Functions.Like(c.Name, "%"+ searchTxt + "%") 
                            ||
                            EF.Functions.Like(c.Family, "%" + searchTxt + "%")
                            ||
                             EF.Functions.Like(c.Email, "%" + searchTxt + "%")
                              ||
                             EF.Functions.Like(c.Tell, "%" + searchTxt + "%")
                    select c;
            List<Contact> lst = new List<Contact>();
            foreach (var item in q)
            {
                lst.Add(new Contact { Email = item.Email, Family = item.Family, Id = item.Id, Name = item.Name, Tell = item.Tell });
            }
            return lst;
        }
        [HttpGet]
        [Route("{id}")]
        public ActionResult<Contact> GetContact(int id)
        {
            var item = _contactManager.People.FirstOrDefault(x=>x.Id==id);
            Contact res = new Contact() { Email = item.Email, Family = item.Family, Id = item.Id, Name = item.Name, Tell = item.Tell };
           
            return res;
        }
        [HttpPost]
        [Route("Create")]
        public ActionResult<int> CreateContact(Contact contact)
        {
            Person person = new Person() { Email = contact.Email, Family = contact.Family, Name = contact.Name, Tell = contact.Tell };
            _contactManager.People.Add(person);
            _contactManager.SaveChanges();
            return person.Id;
           
        }
        [HttpPost]
        [Route("Edit")]
        public ActionResult<int> EditContact(Contact contact)
        {
            Person p = _contactManager.People.FirstOrDefault(x => x.Id == contact.Id);
            p.Email = contact.Email;
            p.Family = contact.Family;
            p.Name = contact.Name;
            p.Tell = contact.Tell;
            _contactManager.SaveChanges();
            return p.Id;

        }
        [HttpGet]
        [Route("Delete/{id}")]
        public ActionResult<bool> DeleteContact(int id)
        {
            try
            {
                Person p = _contactManager.People.FirstOrDefault(x => x.Id == id);
                if (p != null)
                {
                    _contactManager.People.Remove(p);
                    _contactManager.SaveChanges();
                    return true;
                }
                throw new Exception("مخاطبی یافت نشد");
            }
            catch (Exception)
            {

                throw;
            }
           
        }

    }
}
