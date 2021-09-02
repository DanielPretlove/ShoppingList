using ShoppingCart.Models;
using ShoppingCart.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class ItemController : Controller
    {
        ECartDBEntities objECartEntities;

        public ItemController()
        {
            objECartEntities = new ECartDBEntities();
        }

        public ActionResult Index()
        {
            ItemViewModel objItemViewModel = new ItemViewModel();
            objItemViewModel.CategorySelectListItem = (from objCat in objECartEntities.Categories
                select new SelectListItem() 
                {
                    Text = objCat.CategoryName,
                    Value = objCat.CategoryID.ToString(),
                    Selected = true
                });
            return View(objItemViewModel);
        }
    }
}