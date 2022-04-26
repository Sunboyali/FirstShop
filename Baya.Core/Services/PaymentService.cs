using Baya.Core.InterFaces;
using System;
using System.Collections.Generic;
using System.Text;
using Baya.DataLayer.Entities;
using Baya.DataLayer.BayaContextDb;
using Baya.Core.ViewModels;
using Baya.Core.Classes;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Baya.Core.Services
{
    public class PaymentService : IPayment
    {
        private readonly BayaDbContext _context;

        public PaymentService(BayaDbContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void AddFactor(Guid userid, string orderNumber)
        {


            Order order = _context.Orders.SingleOrDefault(o => o.UserId == userid && !o.IsFinally);
            Factor factor = new Factor()
            {
                BankName = null,
                Date = null,
                Desc = null,                
                OrderNumber = orderNumber,
                TotalPrice = Convert.ToInt32(order.Sum),
                RefNumber = null,
                Time = null,
                TraceNumber = null,
                UserId = userid,
                CountProduct = order.CountOrders,
                OrderId = order.Id,
            };


            _context.Factors.Add(factor);
            _context.SaveChanges();
        }



        public async Task<Factor> GetFactor(Guid id)
        {
            return await _context.Factors.FindAsync(id);
        }

        public Guid GetFactorById(string orderNumber)
        {
            return _context.Factors.SingleOrDefault(f => f.OrderNumber == orderNumber).OrderId;
        }

        public bool UpdateFactor(Guid userid, string orderNumber)
        {
            Order order = _context.Orders.SingleOrDefault(o => o.UserId == userid && !o.IsFinally);
            

            Factor factor = _context.Factors.SingleOrDefault(f => f.UserId == userid && f.BankName == null);

            if (factor != null)
            {
                factor.OrderNumber = orderNumber;

                factor.TotalPrice = Convert.ToInt32(order.Sum);

               

                _context.SaveChanges();



               

                return true;





            }
            return false;



        }



        public int GetTotalPriceFactor(Guid factorId)
        {
            return _context.Factors.SingleOrDefault(f => f.OrderId == factorId).TotalPrice;
        }

        public void UpdatePayment(Guid id, string desc, string bank, string trace, string refId)
        {



            Factor factor = _context.Factors.Find(id);

            Order order = _context.Orders.SingleOrDefault(o => o.UserId == factor.UserId && !o.IsFinally);



            factor.Date = DateConvertor.ToShamsi();
            factor.Time = DateConvertor.GetShamsiTime();
            factor.Desc = desc;
            factor.TraceNumber = trace;
            factor.BankName = bank;
            factor.RefNumber = refId;
            order.IsFinally = true;

            _context.SaveChanges();


            var orderdetails = _context.OrderDetails.Include(p => p.Product).Where(o => o.OrderId == order.Id).ToList();

            foreach (var item in orderdetails)
            {
                item.Product.CountProduct -= item.Count;
                item.Product.CountsSale += item.Count;
                _context.SaveChanges();
            }


        }


    }
}
