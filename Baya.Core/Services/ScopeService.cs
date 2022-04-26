using Baya.Core.InterFaces;
using System;
using System.Collections.Generic;
using System.Text;
using Baya.DataLayer.BayaContextDb;
using Baya.Core.Classes;
using Baya.DataLayer.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Baya.Core.ViewModels;

namespace Baya.Core.Services
{
    public class ScopeService : IScope
    {
        private readonly BayaDbContext _context;

        public ScopeService(BayaDbContext context)
        {
            _context = context;
        }

        public  int CountCartUser(Guid Iduser)
        {
            Order order = _context.Orders.SingleOrDefault(o => o.UserId == Iduser && !o.IsFinally);
            if (order != null)
            {
                return _context.OrderDetails.Where(o => o.OrderId == order.Id).Count();
            }
            return 0;

            
        }
        public void AddCard(Guid idProduct, Guid idUser)
        {
            Order order = _context.Orders.FirstOrDefault(o => o.UserId == idUser && !o.IsFinally);
            Product product = _context.Products.Find(idProduct);

            decimal price = product.Price;
            int off = product.Off;
            decimal p1 = Math.Ceiling(price * off / 100);

            decimal p2 = price - p1;

            decimal FinalPrice = (Math.Ceiling(p2 / 100) * 100);

            if (order == null) //کاربر فاکتور باز ندارد
            {
                order = new Order()
                {
                    Id = new Guid(),
                    UserId = idUser,
                    CreateDate = DateConvertor.ToShamsi(),
                    CreateTime = DateConvertor.GetShamsiTime(),
                    IsFinally = false,
                    Sum = 0,

                };
                _context.Orders.Add(order);

                OrderDetail orderDetail = new OrderDetail()
                {
                    Id = new Guid(),
                    Count = 1,
                    OrderId = order.Id,
                    ProductId = idProduct,
                    Price = Convert.ToInt32(FinalPrice) 

                };
                _context.OrderDetails.Add(orderDetail);

                _context.SaveChanges();
                UpdateSumPrice(order.Id);
            }
            else
            {
                var detailes = _context.OrderDetails.SingleOrDefault(o => o.OrderId == order.Id && o.ProductId == idProduct);
                if (detailes == null)
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        Id = new Guid(),
                        Count = 1,
                        OrderId = order.Id,
                        ProductId = idProduct,
                        Price = Convert.ToInt32(FinalPrice)

                    };
                    _context.OrderDetails.Add(orderDetail);

                    _context.SaveChanges();
                    UpdateSumPrice(order.Id);
                }
                else
                {
                    detailes.Count += 1;

                    _context.SaveChanges();
                    UpdateSumPrice(order.Id);
                }
            }


        }

        public void UpdateSumPrice(Guid orderid)
        {
            var order = _context.Orders.Find(orderid);
            order.Sum = _context.OrderDetails.Where(o => o.OrderId == order.Id).Select(d => d.Count * d.Price).Sum();
            order.CountOrders = _context.OrderDetails.Where(o => o.OrderId == orderid).Select(d => d.Count).Sum();
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
        

        public void DeleteBasketProduct(long totalprice, Guid id)
        {
            var orderdetail = _context.OrderDetails.Find(id);

            _context.OrderDetails.Remove(orderdetail);
            var order = _context.Orders.SingleOrDefault(o => o.Id == orderdetail.OrderId);
            int total = order.CountOrders;

            order.CountOrders = total - orderdetail.Count;

            order.Sum = totalprice;
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public async Task<OrderDetail> GetOrderDetailById(Guid id)
        {
            return await _context.OrderDetails.Include(o => o.Order).SingleOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<ShowCardViewModel>> ShowBasketUser(Guid idUser)
        {

            Order order = await _context.Orders.FirstOrDefaultAsync(o => o.UserId == idUser && !o.IsFinally);

            List<ShowCardViewModel> list = new List<ShowCardViewModel>();
            if (order != null)
            {
                var detail = await _context.OrderDetails.Include(p => p.Product)
                    .Where(o => o.OrderId == order.Id).ToListAsync();

                foreach (var item in detail)
                {
                    list.Add(new ShowCardViewModel()
                    {
                        OrderDetailId = item.Id,
                        CountProduct = item.Count,
                        ProductId = item.Product.Id,
                        ImageName = item.Product.ProductImg,
                        ProductName = item.Product.ProductName,
                        ProductCode = item.Product.ProductCode,
                        Price = item.Price,
                        SumPrice = item.Price * item.Count
                    }
                        );
                }

            }
            return list;
        }
        //public async Task<UserDetail> GetInformationUserForPayment(Guid id)
        //{
        //    return await _context.UserDetails.SingleOrDefaultAsync(u => u.UserId == id);
        //}
        public Task<List<City>> GetCities()
        {
            return _context.Cities.OrderBy(c=> c.Name).ToListAsync();
        }
        public Guid GetStateIdByName(string name)
        {
            return _context.Cities.SingleOrDefault(c => c.StateId == null && c.Name == name).Id;
        }
        public Guid GetCityIdByName(string name)
        {
            return _context.Cities.SingleOrDefault(c => c.StateId != null && c.Name == name).Id;
        }

        public async Task<Array> GetUserInfo(Guid id)
        {       

            var user = await _context.Users.Include(u => u.UserDetail).SingleAsync(u=> u.UserId == id);
            var userinfo = new List<string> { user.Mobile, user.UserDetail.Name, user.UserDetail.Family };           
                      
            return userinfo.ToArray();
        }
         void IScope.AddAddressUser(Guid id, AddAddressInfoViewModel model)
        {
            UserAddress userAddress = new UserAddress()
            {
                Id = Guid.NewGuid(),
                UserId = id,
                City = model.City,
                HomeAddress = model.Address,
                Mobile = model.Mobile,
                Ostan = model.State,
                Pelak = model.Pelak,
                PostCode = model.PostCode,
                TransfereeName= model.Name,
                TransfereeFamily = model.Family,
                Vahed = model.Vahed,           

            };
            _context.UserAddresses.Add(userAddress);
            _context.SaveChanges();
        }
        //public long SumPriceUserCart(Guid Iduser)
        //{
        //    return _context.Orders.SingleOrDefault(o => o.UserId == Iduser && !o.IsFinally).Sum;
        //}

        #region Favorites
        public int CountFavoritesUser(Guid Iduser)
        {
            return _context.Favorites.Where(f => f.UserId == Iduser).Count();
        }

        public async Task<List<UserFavoriteViewModel>> GetFavoritesUser(Guid Iduser)
        {
            List<UserFavoriteViewModel> list = new List<UserFavoriteViewModel>();
            var favs =  await _context.Favorites.Include(p => p.Product).Where(f => f.UserId == Iduser).ToListAsync();
            foreach (var item in favs)
            {
                list.Add(new UserFavoriteViewModel()
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductCode = item.Product.ProductCode,
                    ProductImage = item.Product.ProductImg,
                    ProductName = item.Product.ProductName,
                    ProductPrice = item.Product.Price,
                }
               );
            }
            return list;        


        }
       
        public bool AddFavorite(Guid idProduct, Guid idUser)
        {
            bool IsExist = _context.Favorites.Any(f => f.ProductId == idProduct && f.UserId == idUser);

            if (IsExist == false)
            {
                Favorite favorite = new Favorite()
                {
                    ProductId = idProduct,
                    UserId = idUser,
                };
                _context.Favorites.Add(favorite);
                _context.SaveChanges();

                return true;
            }
            return false;


        }


        public void DeleteFavorite(Guid id)
        {
            var fav = _context.Favorites.Find(id);
            _context.Favorites.Remove(fav);
            _context.SaveChanges();
        }

        public bool IsSelectedFavorite(Guid productid, Guid userid)
        {
            return _context.Favorites.Any(f => f.ProductId == productid && f.UserId == userid);
        }

        #endregion

        #region Category
        public async Task<List<Category>> ShowCategory()
        {
            return await _context.Categories.ToListAsync();
        }
        public bool HaveAnySubCategory(Guid id)
        {
            return _context.Categories.Any(c => c.ParentId == id);
        }
        #endregion
        #region Sliders
        public async Task<List<SliderImage>> GetSlidersImage()
        {
            return await _context.SliderImages.Include(s => s.Slider).ToListAsync();
        }
        #endregion



        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }

        }


        #region Theme
        public Theme GetSettingTheme()
        {
            return _context.Themes.Single();
        }
        #endregion
        public async Task<List<Banner>> GetBannersImage()
        {
            return await _context.Banners.ToListAsync();
        }

        public bool UserHaveAddress(Guid id)
        {
            return _context.UserAddresses.Any(a => a.UserId == id);
        }

       
    }
}
