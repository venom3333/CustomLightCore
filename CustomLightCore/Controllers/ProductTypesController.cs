using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CustomLightCore.Models;
using CustomLightCore.ViewModels.ProductTypes;
using Microsoft.AspNetCore.Authorization;

namespace CustomLightCore.Controllers
{
    public class ProductTypesController : BaseController
    {
        // GET
        [Authorize]
        public async Task<IActionResult> List()
        {
            var productTypes = await db.ProductTypes.ToListAsync();

            return View(productTypes);
        }

        // GET: ProductTypes/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            await CreateViewBag();
            return View();
        }

        // POST: ProductTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(
            [Bind("Name,SpecificationTitles")] ProductTypeCreateViewModel createdProductType)
        {
            //TODO: коллекция SpecificationTitles приходит, доделать все!
            if (ModelState.IsValid)
            {
                ProductType category = createdProductType.GetModelByViewModel();

                db.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction("List");
            }

            await CreateViewBag();
            return View(createdProductType);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateSpecificationTitle(ProductTypeCreateViewModel productType)
        {
            SpecificationTitle specificationTitle = new SpecificationTitle();

            if (productType.SpecificationTitles == null)
            {
                productType.SpecificationTitles = new List<SpecificationTitle>();
            }
            
            productType.SpecificationTitles.Add(specificationTitle);

            return PartialView("_SpecificationTitles", productType);
        }
    }

    /*
    [HttpPost]
    public ActionResult GenerateContacts(CustomerModel customer)
    {
        // Check whether this request is comming with javascript, if so, we know that we are going to add contact details.
        if (Request.IsAjaxRequest())
        {
            ContactModel contact = new ContactModel();
            contact.ContactName = customer.ContactMode.ContactName;
            contact.ContactNo = customer.ContactMode.ContactNo;

            if (customer.Contacts == null)
            {
                customer.Contacts = new List<ContactModel>();
            }

            customer.Contacts.Add(contact);

            return PartialView("_Contact", customer);
        }            
    }
    */
}