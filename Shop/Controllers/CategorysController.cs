using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Data.Interfaces;
using Shop.Data.Models;
using Shop.Data.ViewModell;

namespace Shop.Controllers
{
    public class CategorysController : Controller
    {
        private ICategorys IAllCategorys;
        VMItems VMItems = new VMItems();

        public CategorysController(ICategorys IAllCategorys)
        {
            this.IAllCategorys = IAllCategorys;
        }
        [HttpGet]
        public ViewResult List()
        {
            ViewBag.Title = "Страница с категориями";
            var cars = IAllCategorys.AllCategorys;
            return View(cars);
        }
        [HttpGet]
        public ViewResult Add()
        {
            ViewBag.Title = "Добавить категорию";
            return View();
        }
        [HttpPost]
        public RedirectResult Add(string name, string description)
        {

            Categorys newCategorys = new Categorys();
            newCategorys.Name = name;
            newCategorys.Description = description;
            int id = IAllCategorys.Add(newCategorys);
            return Redirect("/Categorys/Update?id=" + id);
        }
        [HttpGet]
        public ViewResult Update()
        {
            ViewBag.Title = "Обновление предмета";
            VMItems.Categorys = IAllCategorys.AllCategorys;
            return View(VMItems);
        }
        [HttpPost]
        public RedirectResult Update(int id, string name, string description, string deleteCategory)
        {
            Categorys newItems = new Categorys();
            newItems.Id = id;
            newItems.Name = name;
            newItems.Description = description;
            IAllCategorys.Update(newItems, deleteCategory);
            return Redirect("/Categorys/List");
        }
    }
}
