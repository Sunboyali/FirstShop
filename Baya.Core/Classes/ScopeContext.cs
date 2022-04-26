using Baya.DataLayer.BayaContextDb;
using System;
using System.Collections.Generic;
using System.Text;
using Baya.Core.InterFaces;
using System.Linq;
using Baya.DataLayer.Entities;

namespace Baya.Core.Classes
{
  public class ScopeContext
    {

        private readonly BayaDbContext _context;
        public ScopeContext(BayaDbContext context)
        {
            _context = context;
        }

        public bool HaveSubCategory(Guid id)
        {
            return _context.Categories.Any(c => c.ParentId == id);
        }

        public int CountSubCategory(Guid id)
        {
            return _context.Categories.Where(c => c.ParentId == id).Count();
        }
        public string GetProductImgById(Guid id)
        {
            return _context.Products.SingleOrDefault(p => p.Id == id).ProductImg;
        }


        public string GetStateName(Guid id)
        {
            return _context.Cities.SingleOrDefault(p => p.Id == id).Name;
        }


    }
}
