using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shop.Data.ViewModell;
using Shop.Data.DataBase;
using Shop.Data.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Shop.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IHostingEnvironment hostingEnviroment;
        private IItems IAllItems;
        private ICategorys IAllCategorys;
        VMItems VMItems = new VMItems();

        public ItemsController(IItems IAllItems, ICategorys IAllCategorys, IHostingEnvironment environment)
        {
            this.IAllItems = IAllItems;
            this.IAllCategorys = IAllCategorys;
            this.hostingEnviroment = environment;
        }
        public ViewResult List(int id = 0, int priceSort = 0, string search = "")
        {
            ViewBag.Title = "Страница с предметами";
            VMItems.Items = IAllItems.AllItems;
            VMItems.Categorys = IAllCategorys.AllCategorys;
            VMItems.SelectCategory = id;
            VMItems.PriceSort = priceSort;
            VMItems.search = search;
            return View(VMItems);
        }
        [HttpGet]
        public ViewResult Add()
        {
            IEnumerable<Categorys> Categorys = IAllCategorys.AllCategorys;
            return View(Categorys);
        }
        [HttpPost]
        public RedirectResult Add(string name, string description, IFormFile files, float price, int idCategory)
        {
            if (files != null)
            {
                var uploads = Path.Combine(hostingEnviroment.WebRootPath, "img");
                var filePath = Path.Combine(uploads, files.FileName);
                FileStream stream = new FileStream(filePath, FileMode.Create);
                files.CopyTo(stream);
                stream.Close();
            }
            Items newItems = new Items();
            newItems.Name = name;
            newItems.Description = description;

            newItems.Img = ("/img/" + files.FileName);
            newItems.Price = Convert.ToInt32(price);
            newItems.Category = new Categorys() { Id = idCategory };
            int id = IAllItems.Add(newItems);
            return Redirect("/Items/Update?id=" + id);
        }
        [HttpGet]
        public ViewResult Update()
        {
            ViewBag.Title = "Обновление предмета";
            VMItems.Items = IAllItems.AllItems;
            VMItems.Categorys = IAllCategorys.AllCategorys;
            return View(VMItems);
        }
        [HttpPost]
        public RedirectResult Update(int id, string name, string description, IFormFile files, float price, int idCategory, string deleteItem)
        {
            Items newItems = new Items();
            if (files != null)
            {
                var uploads = Path.Combine(hostingEnviroment.WebRootPath, "img");
                var filePath = Path.Combine(uploads, files.FileName);
                files.CopyTo(new FileStream(filePath, FileMode.Create));
                newItems.Img = files.FileName;
            }
            if (files == null)
            {
                newItems.Img = "NaN";
            }
            newItems.Id = id;
            newItems.Name = name;
            newItems.Description = description;
            newItems.Price = Convert.ToInt32(price);
            newItems.Category = new Categorys() { Id = idCategory };
            IAllItems.Update(newItems, deleteItem);
            return Redirect("/Items/List");
        }
        public ActionResult Basket(int idItem = -1)
        {
            if (idItem != -1)
            {
                Startup.BasketItem.Add(new ItemsBasket(1, IAllItems.AllItems.Where(x => x.Id == idItem).First()));
            }
            return Json(Startup.BasketItem);
        }
        public ActionResult BasketCount(int idItem = -1, int count = -1)
        {
            if(idItem != -1)
            {
                if(count == 0)
                {
                    Startup.BasketItem.Remove(Startup.BasketItem.Find(x => x.Id == idItem));
                }
                else
                {
                    Startup.BasketItem.Find(x => x.Id == idItem).Count = count;
                }
            }
            return Json(Startup.BasketItem);
        }
        [HttpGet]
        public ViewResult ItemBasket()
        {
            ViewBag.Title = "Корзина предметов";
            return View();
        }
    }
}
