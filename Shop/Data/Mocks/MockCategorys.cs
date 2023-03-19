using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Data.Interfaces;
using Shop.Data.Models;

namespace Shop.Data.Mocks
{
    public class MockCategorys : ICategorys
    {
        public IEnumerable<Categorys> AllCategorys
        {
            get
            {
                return new List<Categorys>()
                {
                    new Categorys()
                    {
                        Id = 1,
                        Name = "Микроволновые печи",
                        Description = "Микроволновая печь - это ааааа"
                    },
                    new Categorys()
                    {
                        Id = 2,
                        Name = "Мультиварки",
                        Description = "Описание12334214"
                    }
                };
            }
        }

        public int Add(Categorys categorys)
        {
            return 0;
        }
        public void Update(Categorys categorys, string deleteCategory)
        {

        }
    }
}
