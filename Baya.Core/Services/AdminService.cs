using System;
using System.Collections.Generic;
using System.Text;
using Baya.Core.InterFaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Baya.DataLayer.BayaContextDb;
using Baya.Core.ViewModels;
using System.Threading.Tasks;
using Baya.DataLayer.Entities;
using Baya.Core.Classes;
using System.IO;
using SixLabors.ImageSharp;

using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Baya.Core.Services
{
    public class AdminService : IAdmin
    {
        private readonly BayaDbContext _context;
        //private readonly IDbContextTransaction _transaction;


        public AdminService(BayaDbContext context)
        {
            _context = context;
            //_transaction = _context.Database.BeginTransaction();
        }
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }


        }

        //public void Commit()
        //{
        //    _transaction.Commit();
        //}

        //public void RoleBack()
        //{
        //    _transaction.Rollback();
        //}


        #region Category

        public async Task<Category> GetCategoryById(Guid id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<List<Category>> GetGategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<string> GetFullCategory(Guid id)
        {
            var GetSecondCategory = await _context.Categories.SingleOrDefaultAsync(c => c.Id == id);
            var First_Sub = await _context.Categories.SingleOrDefaultAsync(c => c.Id == GetSecondCategory.ParentId);
            var Main_cat = await _context.Categories.SingleOrDefaultAsync(c => c.Id == First_Sub.ParentId);

            string GetCatFull = Main_cat.CategoryName + '-' + First_Sub.CategoryName + '-' + GetSecondCategory.CategoryName;

            return await Task.FromResult(GetCatFull);

        }
        public bool ExistCategory(Guid id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool ExistNameCategoryForAdd(string catname)
        {
            return _context.Categories.Any(c => c.CategoryName == catname.Trim());
        }
        public bool ExistNameCategoryForUpdate(string catname, Guid id)
        {
            return _context.Categories.Any(c => c.CategoryName == catname.Trim() && c.Id != id);
        }



        public void AddOrEditMainCategory(MainCategoryViewModel model)
        {
            try
            {
                if (model.Id == Guid.Empty) // Add
                {
                    string filename = model.MainCatImg.FileName;
                    string strExt = Path.GetExtension(filename); //گرفتن پسوند فایل
                    filename = CodeGenerators.FileCode() + strExt;

                    Category category = new Category()
                    {
                        Id = Guid.NewGuid(),
                        CategoryImg = filename,
                        CategoryLevel = 0,
                        ParentId = null,
                        CategoryName = model.CategoryName,
                    };


                    string imgpath = "";
                    int width = 40;
                    int height = 40;
                    imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/category/icon/" + filename);

                    using var image = Image.Load(model.MainCatImg.OpenReadStream());
                    image.Mutate(x => x.Resize(width, height));
                    image.Save(imgpath);



                    _context.Categories.Add(category);
                    _context.SaveChanges();
                }
                else // Update
                {
                    var maincategory = _context.Categories.Find(model.Id);
                    if (model.MainCatImg != null)
                    {
                        File.Delete("wwwroot/site/img/category/icon/" + maincategory.CategoryImg);


                        string filename = model.MainCatImg.FileName;
                        string strExt = Path.GetExtension(filename); //گرفتن پسوند فایل
                        filename = CodeGenerators.FileCode() + strExt;

                        maincategory.CategoryImg = filename;
                        maincategory.CategoryName = model.CategoryName;




                        string imgpath = "";
                        int width = 40;
                        int height = 40;
                        imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/category/icon/" + filename);

                        using var image = Image.Load(model.MainCatImg.OpenReadStream());
                        image.Mutate(x => x.Resize(width, height));
                        image.Save(imgpath);

                        _context.SaveChanges();
                    }
                    else
                    {
                        maincategory.CategoryImg = maincategory.CategoryImg;
                        maincategory.CategoryName = model.CategoryName;
                        _context.SaveChanges();
                    }
                }



            }
            catch
            {

                throw;
            }

        }
        public void AddOrUpdateSubCategory(SubCategoryViewModel model)
        {
            try
            {
                if (model.ParentId == null)//Add
                {
                    Category firstsubcategory = new Category()
                    {
                        Id = Guid.NewGuid(),
                        CategoryName = model.CategoryName,
                        ParentId = model.Id,
                        CategoryLevel = 1,
                        CategoryImg = null,
                    };
                    _context.Categories.Add(firstsubcategory);
                    _context.SaveChanges();
                }
                else
                {
                    var firstsubcategory = _context.Categories.Find(model.Id);
                    firstsubcategory.CategoryName = model.CategoryName;
                    firstsubcategory.ParentId = model.ParentId;
                    _context.SaveChanges();
                }
            }
            catch
            {

                throw;
            }

        }

        public void AddOrUpdateSecondSubCategory(SubCategoryViewModel model)
        {
            try
            {
                if (model.Level == 1)//Add
                {
                    Category firstsubcategory = new Category()
                    {
                        Id = Guid.NewGuid(),
                        CategoryName = model.CategoryName,
                        ParentId = model.Id,
                        CategoryLevel = 2,
                        CategoryImg = null,
                    };
                    _context.Categories.Add(firstsubcategory);
                    _context.SaveChanges();
                }
                else
                {
                    var firstsubcategory = _context.Categories.Find(model.Id);
                    firstsubcategory.CategoryName = model.CategoryName;
                    firstsubcategory.ParentId = model.ParentId;
                    _context.SaveChanges();
                }
            }
            catch
            {

                throw;
            }

        }

        public void DeleteCategory(Guid id)
        {
            Category category = _context.Categories.Find(id);

            if (category.CategoryImg != null)
            {
                File.Delete("wwwroot/site/img/category/icon/" + category.CategoryImg);
            }


            _context.Categories.Remove(category);
            _context.SaveChanges();
        }



        #endregion

        #region Brand
        public async Task<List<Brand>> GetBrands()
        {
            return await _context.Brands.Include(c => c.Category).ToListAsync();
        }

        public async Task<Brand> GetBrandById(Guid id)
        {
            return await _context.Brands.FindAsync(id);

        }


        public void AddBrand(Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();
        }
        public void UpdateBrand(Brand brand)
        {
            _context.Brands.Update(brand);
            _context.SaveChanges();
        }
        public bool ExistBrandId(Guid id)
        {
            return _context.Brands.Any(b => b.Id == id);
        }


        #endregion

        #region User

        public Guid GetRoleId(string name)
        {
            return _context.Roles.SingleOrDefault(r => r.RoleName == name).RoleId;
        }


        public async Task<List<User>> GetActiveUsers(int skip, int countuser)
        {
            Guid useridrole = GetRoleId("Admin");
            return await _context.Users.Include(r => r.Role).Where(u => u.IsActive == true && u.IsBlock == false && u.RoleId != useridrole).OrderBy(c => c.Mobile).Skip(skip).Take(countuser).ToListAsync();
        }
        public async Task<List<User>> GetActiveUsersBySearch(string mobile, int skip, int countuser)
        {
            Guid useridrole = GetRoleId("Admin");
            return await _context.Users.Include(r => r.Role).Where(u => u.Mobile.Contains(mobile.Trim()) && u.IsActive == true && u.IsBlock == false && u.RoleId != useridrole).OrderBy(c => c.Mobile).Skip(skip).Take(countuser).ToListAsync();
        }
        public async Task<List<User>> GetDeActiveUsers(int skip, int countuser)
        {
            return await _context.Users.Include(r => r.Role).Where(u => u.IsActive == false && u.IsBlock == false).OrderBy(c => c.Mobile).Skip(skip).Take(countuser).ToListAsync();
        }
        public async Task<List<User>> GetDeActiveUsersBySearch(string mobile, int skip, int countuser)
        {
            return await _context.Users.Include(r => r.Role).Where(u => u.Mobile.Contains(mobile.Trim()) && u.IsActive == false && u.IsBlock == false).OrderBy(c => c.Mobile).Skip(skip).Take(countuser).ToListAsync();
        }
        public async Task<List<User>> GetBlockedUsers(int skip, int countuser)
        {
            return await _context.Users.Include(r => r.Role).Where(u => u.IsBlock == true).OrderBy(c => c.Mobile).Skip(skip).Take(countuser).ToListAsync();
        }

        public async Task<List<User>> GetBlockedUsersBySearch(string mobile, int skip, int countuser)
        {
            return await _context.Users.Include(r => r.Role).Where(u => u.Mobile.Contains(mobile.Trim()) && u.IsBlock == true).OrderBy(c => c.Mobile).Skip(skip).Take(countuser).ToListAsync();
        }

        public int CountActiveUsers()
        {
            Guid useridrole = GetRoleId("Admin");
            return _context.Users.Where(u => u.IsActive == true && u.IsBlock == false && u.RoleId != useridrole).Count();
        }

        public int CountActiveUsersBySearch(string mobile)
        {
            Guid useridrole = GetRoleId("Admin");
            return _context.Users.Where(u => u.Mobile.Contains(mobile.Trim()) && u.IsActive == true && u.IsBlock == false && u.RoleId != useridrole).Count();
        }

        public int CountDeActiveUsers()
        {
            return _context.Users.Where(u => u.IsActive == false && u.IsBlock == false).Count();
        }


        public int CountDeActiveUsersBySearch(string mobile)
        {
            return _context.Users.Where(u => u.Mobile.Contains(mobile.Trim()) && u.IsActive == false && u.IsBlock == false).Count();
        }
        public int CountBlockedUsers()
        {
            return _context.Users.Where(u => u.IsBlock == true).Count();
        }


        public int CountBlockedUsersBySearch(string mobile)
        {
            return _context.Users.Where(u => u.Mobile.Contains(mobile.Trim()) && u.IsBlock == true).Count();
        }



        public async Task<User> GetUserById(Guid id)
        {
            return await _context.Users.Include(u => u.UserDetail).SingleOrDefaultAsync(u => u.UserId == id);
        }




        public bool CheckMobileNumber(string mobile)
        {
            return _context.Users.Any(u => u.Mobile == mobile);
        }

        public void AddUser(UserViewModel user)
        {
            try
            {
                User newuser = new User()
                {
                    ActiveCode = CodeGenerators.ActiveCode(),
                    Password = HashGenerators.HashWithMD5(HashGenerators.HashWithMD5(user.Password)),
                    RoleId = user.RoleId,
                    Token = null,
                    Mobile = user.Mobile,
                    Wallet = 0,
                    IsActive = true
                };
                _context.Users.Add(newuser);
                UserDetail userDetail = new UserDetail()
                {
                    UserId = newuser.UserId,

                    Date = DateConvertor.ToShamsi(),
                    Time = DateConvertor.GetShamsiTime(),
                    UserImg = "avatar.png"

                };
                _context.UserDetails.Add(userDetail);

                _context.SaveChanges();
            }
            catch
            {

                throw;
            }

        }



        public void BlockUser(Guid id)
        {
            User user = _context.Users.Find(id);
            if (user.IsBlock == true)
            {
                user.IsBlock = false;
            }
            else
            {
                user.IsBlock = true;
            }
            _context.SaveChanges();
        }



        public void UpdateRoleUser(ChangeRoleViewModel model)
        {
            User user = _context.Users.Find(model.Id);

            user.RoleId = model.RoleId;
            _context.SaveChanges();
        }




        #endregion
        #region Shopper
        //public void AddShopper(Guid id)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion

        #region Role
        public async Task<List<Role>> GetRoles()
        {
            return await _context.Roles.Where(r => r.RoleName != "Admin").OrderByDescending(r => r.RoleName).ToListAsync();
        }

        public async Task<Role> GetRoleById(Guid id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public void AddAndUpdateRole(RoleViewModel model)
        {
            if (model.Id == Guid.Empty) //Add
            {
                Role role = new Role()
                {
                    RoleId = Guid.NewGuid(),
                    RoleName = model.RoleName,
                    RoleTitle = model.RoleTitle
                };
                _context.Roles.Add(role);
                _context.SaveChanges();

            }
            else
            {
                var role = _context.Roles.Find(model.Id);
                role.RoleName = model.RoleName;
                role.RoleTitle = model.RoleTitle;
                _context.SaveChanges();
            }
        }

        public void DeleteRole(Guid id)
        {
            Role role = _context.Roles.Find(id);
            _context.Roles.Remove(role);
            _context.SaveChanges();
        }

        public bool ExistRoleName(RoleViewModel model)
        {
            if (model.Id == Guid.Empty)
            {
                return _context.Roles.Any(r => r.RoleName == model.RoleName);
            }
            else
            {
                return _context.Roles.Any(r => r.RoleName == model.RoleName && r.RoleId != model.Id);
            }
        }






        #endregion

        #region Product
        public async Task<List<Product>> GetProducts(int skip, int countproduct)
        {
            return await _context.Products.Where(p => p.IsShowProduct == true && !p.Amazing).Include(c => c.Category).Include(p => p.ProductDetail).OrderByDescending(p => p.ProductDetail.DateProduct).Skip(skip).Take(countproduct).ToListAsync();
        }


        public int CountProducts()
        {
            return _context.Products.Where(p => p.IsShowProduct == true && !p.Amazing).Count();
        }


        public bool ShowProduct(Guid id)
        {
            Product product = _context.Products.Find(id);
            if (product.IsShowProduct == false)
            {
                product.IsShowProduct = true;
                _context.SaveChanges();

                return true;
            }
            else
            {
                product.IsShowProduct = false;
                _context.SaveChanges();
                return false;
            }

        }



        public async Task<Product> GetProductById(Guid id)
        {
            return await _context.Products.Include(p => p.ProductDetail).Include(c => c.Category).SingleOrDefaultAsync(p => p.Id == id);
        }
        public void AddProduct(ProductViewModel model)
        {
            decimal FinalPrice = model.Price;
            if (model.Off > 0 && model.Off < 100)
            {
                decimal p1 = Math.Ceiling(model.Price * model.Off / 100);

                decimal p2 = model.Price - p1;

                FinalPrice = (Math.Ceiling(p2 / 100) * 100);
            }
            if (model.ImagesGallery == null)
            {
                try
                {
                    var colors = "";
                    if (model.Color != null && model.Color.Length >= 2)
                    {
                        colors = String.Join(",", model.Color);
                    }
                    else if (model.Color != null && model.Color.Length == 1)
                    {
                        colors = String.Join("", model.Color);
                    }
                    else
                    {
                        colors = null;
                    }



                    string filename = model.ImageProduct.FileName;
                    string strExt = Path.GetExtension(filename); //گرفتن پسوند فایل
                    filename = CodeGenerators.FileCode() + strExt;


                    Product product = new Product()
                    {
                        Id = Guid.NewGuid(),
                        ProductName = model.ProductName,
                        Amazing = model.Amazing,
                        CountProduct = model.CountProduct,
                        IsShowProduct = true,
                        Off = model.Off,
                        Price = Convert.ToInt32(model.Price),
                        FinalPrice = Convert.ToInt32(FinalPrice),
                        ProductCode = CodeGenerators.FactorCode(),
                        ProductImg = filename,
                        CountsSale = 0,
                        CategoryId = model.CategoryId,
                        Color = colors,


                    };
                    _context.Products.Add(product);
                    ProductDetail productDetail = new ProductDetail()
                    {
                        ProductId = product.Id,
                        Description = model.DesProduct,
                        Feature = model.Feature,
                        DateProduct = DateConvertor.ToShamsi(),
                        TimeProduct = DateConvertor.GetShamsiTime(),
                        MetaTag = model.MetaTag,
                        MetaDescription = model.MetaDescription,
                        VisitCount = 0,

                    };
                    _context.ProductDetails.Add(productDetail);


                    string imgpath = "";
                    int width = 350;
                    int height = 350;
                    imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/product-img/main/" + filename);

                    using var image = Image.Load(model.ImageProduct.OpenReadStream());
                    image.Mutate(x => x.Resize(width, height));
                    image.Save(imgpath);


                    string imgpaththumbnail = "";
                    int widththumnail = 100;
                    int heightthumnail = 100;
                    imgpaththumbnail = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/product-img/thumbnail/" + filename);
                    using var imagethumnail = Image.Load(model.ImageProduct.OpenReadStream());
                    imagethumnail.Mutate(x => x.Resize(widththumnail, heightthumnail));
                    imagethumnail.Save(imgpaththumbnail);

                    _context.SaveChanges();
                }
                catch
                {

                    throw;
                }
            }
            else// Add Product With Gallery Images
            {
                try
                {
                    var colors = "";
                    if (model.Color != null && model.Color.Length >= 2)
                    {
                        colors = String.Join(",", model.Color);
                    }
                    else if (model.Color != null && model.Color.Length == 1)
                    {
                        colors = String.Join("", model.Color);
                    }
                    else
                    {
                        colors = null;
                    }

                    string filename = model.ImageProduct.FileName;
                    string strExt = Path.GetExtension(filename); //گرفتن پسوند فایل
                    filename = CodeGenerators.FileCode() + strExt;
                    Product product = new Product()
                    {
                        Id = Guid.NewGuid(),
                        ProductName = model.ProductName,
                        Amazing = model.Amazing,
                        CountProduct = model.CountProduct,
                        IsShowProduct = true,
                        Off = model.Off,
                        Price = Convert.ToInt32(model.Price),
                        FinalPrice = Convert.ToInt32(FinalPrice),
                        ProductCode = CodeGenerators.FactorCode(),
                        ProductImg = filename,
                        CountsSale = 0,
                        CategoryId = model.CategoryId,
                        Color = colors,

                    };
                    _context.Products.Add(product);
                    ProductDetail productDetail = new ProductDetail()
                    {
                        ProductId = product.Id,
                        Description = model.DesProduct,
                        Feature = model.Feature,
                        DateProduct = DateConvertor.ToShamsi(),
                        TimeProduct = DateConvertor.GetShamsiTime(),
                        MetaTag = model.MetaTag,
                        MetaDescription = model.MetaDescription,
                        VisitCount = 0,

                    };
                    _context.ProductDetails.Add(productDetail);




                    string imgpath = "";
                    int width = 350;
                    int height = 350;
                    imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/product-img/main/" + filename);

                    using var image = Image.Load(model.ImageProduct.OpenReadStream());
                    image.Mutate(x => x.Resize(width, height));
                    image.Save(imgpath);


                    string imgpaththumbnail = "";
                    int widththumnail = 100;
                    int heightthumnail = 100;
                    imgpaththumbnail = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/product-img/thumbnail/" + filename);
                    using var imagethumnail = Image.Load(model.ImageProduct.OpenReadStream());
                    imagethumnail.Mutate(x => x.Resize(widththumnail, heightthumnail));
                    imagethumnail.Save(imgpaththumbnail);



                    string imagesGalleryPath = "";
                    int widthgalleryimage = 350;
                    int heightgalleryimage = 350;
                    foreach (var item in model.ImagesGallery)
                    {
                        string galleryimgname = item.FileName;
                        string galleryimgExt = Path.GetExtension(galleryimgname); //گرفتن پسوند فایل
                        galleryimgname = CodeGenerators.FileCode() + galleryimgExt;
                        imagesGalleryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/product-img/gallery/" + galleryimgname);

                        using var imagegallery = Image.Load(item.OpenReadStream());
                        imagegallery.Mutate(x => x.Resize(widthgalleryimage, heightgalleryimage));
                        imagegallery.Save(imagesGalleryPath);

                        ProductGallery gallery = new ProductGallery()
                        {
                            ProductId = product.Id,
                            ImageName = galleryimgname,
                            Alt = model.ProductName
                        };
                        _context.Add(gallery);

                    }



                    _context.SaveChanges();
                }
                catch
                {

                    throw;
                }
            }



        }



        public void UpdateProduct(ProductViewModel model)
        {
            try
            {
                decimal FinalPrice = model.Price;
                if (model.Off > 0 && model.Off < 100)
                {
                    decimal p1 = Math.Ceiling(model.Price * model.Off / 100);

                    decimal p2 = model.Price - p1;

                    FinalPrice = (Math.Ceiling(p2 / 100) * 100);
                }
                var product = _context.Products.Include(p => p.ProductDetail).SingleOrDefault(p => p.Id == model.Id);
                var colors = "";
                if (model.Color != null && model.Color.Length >= 2)
                {
                    colors = String.Join(",", model.Color);
                }
                else if (model.Color != null && model.Color.Length == 1)
                {
                    colors = String.Join("", model.Color);
                }
                else
                {
                    colors = null;
                }

                if (model.ImageProduct != null)
                {

                    File.Delete("wwwroot/site/img/product-img/main/" + product.ProductImg);

                    File.Delete("wwwroot/site/img/product-img/thumbnail/" + product.ProductImg);

                    string filename = model.ImageProduct.FileName;
                    string strExt = Path.GetExtension(filename); //گرفتن پسوند فایل
                    filename = CodeGenerators.FileCode() + strExt;




                    product.ProductName = model.ProductName;
                    product.CountProduct = model.CountProduct;
                    product.Off = model.Off;
                    product.Price = Convert.ToInt32(model.Price);
                    product.FinalPrice = Convert.ToInt32(FinalPrice);
                    product.CategoryId = model.CategoryId;
                    product.Color = colors;
                    product.ProductImg = filename;
                    product.ProductDetail.Description = model.DesProduct;
                    product.ProductDetail.Feature = model.Feature;
                    product.ProductDetail.MetaTag = model.MetaTag;
                    product.ProductDetail.MetaDescription = model.MetaDescription;
                    product.Amazing = model.Amazing;


                    string imgpath = "";
                    int width = 350;
                    int height = 350;
                    imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/product-img/main/" + filename);

                    using var image = Image.Load(model.ImageProduct.OpenReadStream());
                    image.Mutate(x => x.Resize(width, height));
                    image.Save(imgpath);


                    string imgpaththumbnail = "";
                    int widththumnail = 100;
                    int heightthumnail = 100;
                    imgpaththumbnail = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/product-img/thumbnail/" + filename);
                    using var imagethumnail = Image.Load(model.ImageProduct.OpenReadStream());
                    imagethumnail.Mutate(x => x.Resize(widththumnail, heightthumnail));
                    imagethumnail.Save(imgpaththumbnail);

                    _context.SaveChanges();

                }
                else
                {


                    product.ProductName = model.ProductName;
                    product.CountProduct = model.CountProduct;
                    product.Off = model.Off;
                    product.Price = Convert.ToInt32(model.Price);
                    product.FinalPrice = Convert.ToInt32(FinalPrice);
                    product.Color = colors;
                    product.ProductDetail.Description = model.DesProduct;
                    product.ProductDetail.Feature = model.Feature;
                    product.ProductDetail.MetaTag = model.MetaTag;
                    product.ProductDetail.MetaDescription = model.MetaDescription;
                    product.Amazing = model.Amazing;

                    _context.SaveChanges();
                }


            }
            catch
            {

                throw;
            }
        }











        void IAdmin.RomoveProduct(Guid id)
        {
            try
            {
                var product = _context.Products.Find(id);
                var productdetail = _context.ProductDetails.Find(id);

                File.Delete("wwwroot/site/img/product-img/main/" + product.ProductImg);
                File.Delete("wwwroot/site/img/product-img/thumbnail/" + product.ProductImg);


                bool Product_hasGallery = _context.ProductGalleries.Any(g => g.ProductId == product.Id);

                if (Product_hasGallery == true)
                {

                    var gallery_product = _context.ProductGalleries.Where(g => g.ProductId == product.Id).ToList();

                    foreach (var item in gallery_product)
                    {
                        File.Delete("wwwroot/site/img/product-img/gallery/" + item.ImageName);
                    }
                    _context.AddRange(gallery_product);

                }

                _context.ProductDetails.Remove(productdetail);
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            catch
            {

                throw;
            }


        }




        public int CountProductsBySearch(string code, string name)
        {
            if (code != null)
            {
                return _context.Products.Where(p => p.ProductCode.Contains(code) && p.IsShowProduct == true && !p.Amazing).Count();
            }
            else if (name != null)
            {
                return _context.Products.Where(p => p.ProductName.Contains(name) && p.IsShowProduct == true && !p.Amazing).Count();
            }
            else
            {
                return _context.Products.Where(p => p.ProductCode.Contains(code) && p.IsShowProduct == true && p.ProductName.Contains(name) && !p.Amazing).Count();
            }
        }

        public async Task<List<Product>> SearchProducts(string code, string name, int skip, int countproduct)
        {
            if (code != null)
            {
                return await _context.Products.Include(p => p.ProductDetail).Where(p => p.ProductCode.Contains(code) && p.IsShowProduct == true && !p.Amazing).Take(countproduct).Skip(skip).ToListAsync();
            }
            else if (name != null)
            {
                return await _context.Products.Include(p => p.ProductDetail).Where(p => p.ProductName.Contains(name) && p.IsShowProduct == true && !p.Amazing).Take(countproduct).Skip(skip).ToListAsync();
            }
            else
            {
                return await _context.Products.Include(p => p.ProductDetail).Where(p => p.ProductCode.Contains(code) && p.IsShowProduct == true && p.ProductName.Contains(name) && !p.Amazing).Take(countproduct).Skip(skip).ToListAsync();
            }

        }


        public int CountUnShowProductsBySearch(string code, string name)
        {
            if (code != null)
            {
                return _context.Products.Where(p => p.ProductCode.Contains(code) && p.IsShowProduct == false && !p.Amazing).Count();
            }
            else if (name != null)
            {
                return _context.Products.Where(p => p.ProductName.Contains(name) && p.IsShowProduct == false && !p.Amazing).Count();
            }
            else
            {
                return _context.Products.Where(p => p.ProductCode.Contains(code) && p.IsShowProduct == false && p.ProductName.Contains(name) && !p.Amazing).Count();
            }
        }

        public async Task<List<Product>> SearchUnShowProducts(string code, string name, int skip, int countproduct)
        {
            if (code != null)
            {
                return await _context.Products.Include(p => p.ProductDetail).Where(p => p.ProductCode.Contains(code) && p.IsShowProduct == false && !p.Amazing).Take(countproduct).Skip(skip).ToListAsync();
            }
            else if (name != null)
            {
                return await _context.Products.Include(p => p.ProductDetail).Where(p => p.ProductName.Contains(name) && p.IsShowProduct == false && !p.Amazing).Take(countproduct).Skip(skip).ToListAsync();
            }
            else
            {
                return await _context.Products.Include(p => p.ProductDetail).Where(p => p.ProductCode.Contains(code) && p.IsShowProduct == false && p.ProductName.Contains(name) && !p.Amazing).Take(countproduct).Skip(skip).ToListAsync();
            }

        }


        #endregion
        #region Gallery
        public async Task<List<ProductGallery>> GetLsitProductGallery(Guid id)
        {
            return await _context.ProductGalleries.Where(g => g.ProductId == id).ToListAsync();
        }

        public void UpdateOneImageGallery(UpdateImageViewModel model)
        {
            var Image_Gallery = _context.ProductGalleries.Find(model.Id);

            if (Image_Gallery != null)
            {
                try
                {
                    string imagename = model.SingleImageProduct.FileName;
                    string strExt = Path.GetExtension(imagename); //گرفتن پسوند فایل
                    imagename = CodeGenerators.FileCode() + strExt;




                    File.Delete("wwwroot/site/img/product-img/gallery/" + Image_Gallery.ImageName);

                    Image_Gallery.ImageName = imagename;

                    _context.ProductGalleries.Update(Image_Gallery);

                    string imgpath = "";
                    int width = 350;
                    int height = 350;
                    imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/product-img/gallery/" + imagename);

                    using var image = Image.Load(model.SingleImageProduct.OpenReadStream());
                    image.Mutate(x => x.Resize(width, height));
                    image.Save(imgpath);

                    _context.SaveChanges();

                }
                catch
                {

                    throw;
                }

            }

        }

        public async Task<ProductGallery> GetImageGalleryById(Guid id)
        {
            return await _context.ProductGalleries.FindAsync(id);
        }

        public void UpdateMainImageProduct(UpdateImageViewModel model)
        {
            var Image_Product = _context.Products.Find(model.Id);

            if (Image_Product != null)
            {
                try
                {
                    string imagename = model.SingleImageProduct.FileName;
                    string strExt = Path.GetExtension(imagename); //گرفتن پسوند فایل
                    imagename = CodeGenerators.FileCode() + strExt;




                    File.Delete("wwwroot/site/img/product-img/main/" + Image_Product.ProductImg);
                    File.Delete("wwwroot/site/img/product-img/thumbnail/" + Image_Product.ProductImg);

                    Image_Product.ProductImg = imagename;

                    _context.Products.Update(Image_Product);

                    string imgpath = "";
                    int width = 350;
                    int height = 350;
                    imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/product-img/main/" + imagename);

                    using var image = Image.Load(model.SingleImageProduct.OpenReadStream());
                    image.Mutate(x => x.Resize(width, height));
                    image.Save(imgpath);



                    string imgpaththumbnail = "";
                    int widththumnail = 100;
                    int heightthumnail = 100;
                    imgpaththumbnail = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/product-img/thumbnail/" + imagename);
                    using var imagethumnail = Image.Load(model.SingleImageProduct.OpenReadStream());
                    imagethumnail.Mutate(x => x.Resize(widththumnail, heightthumnail));
                    imagethumnail.Save(imgpaththumbnail);


                    _context.SaveChanges();

                }
                catch
                {

                    throw;
                }

            }
        }

        public void AddGalleryListForProduct(ProductGalleryViewModel model)
        {
            try
            {
                string ProductName = _context.Products.SingleOrDefault(p => p.Id == model.Id).ProductName;

                string imagesGalleryPath = "";
                int widthgalleryimage = 350;
                int heightgalleryimage = 350;
                foreach (var item in model.GalleryImages)
                {
                    string galleryimgname = item.FileName;
                    string galleryimgExt = Path.GetExtension(galleryimgname); //گرفتن پسوند فایل
                    galleryimgname = CodeGenerators.FileCode() + galleryimgExt;
                    imagesGalleryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/product-img/gallery/" + galleryimgname);

                    using var imagegallery = Image.Load(item.OpenReadStream());
                    imagegallery.Mutate(x => x.Resize(widthgalleryimage, heightgalleryimage));
                    imagegallery.Save(imagesGalleryPath);

                    ProductGallery gallery = new ProductGallery()
                    {
                        ProductId = model.Id,
                        ImageName = galleryimgname,
                        Alt = ProductName
                    };
                    _context.Add(gallery);

                }
                _context.SaveChanges();
            }
            catch
            {

                throw;
            }

        }

        #endregion
        #region Amazing Product
        public void Amazing(Guid id)
        {
            Product product = _context.Products.Find(id);
            if (product.Amazing == false)
            {
                product.Amazing = true;
                _context.SaveChanges();
            }
            else
            {
                product.Amazing = false;
                _context.SaveChanges();
            }
        }
        public async Task<List<Product>> GetAmazingProducts(int skip, int countproduct)
        {
            return await _context.Products.Where(p => p.IsShowProduct == true && p.Amazing == true).Include(c => c.Category).Include(p => p.ProductDetail).OrderByDescending(p => p.ProductDetail.DateProduct).Skip(skip).Take(countproduct).ToListAsync();
        }

        public int CountAmazingProducts()
        {
            return _context.Products.Where(p => p.IsShowProduct == true && p.Amazing == true).Count();
        }



        public int CountAmazingProductsBySearch(string code, string name)
        {
            if (code != null)
            {
                return _context.Products.Where(p => p.ProductCode.Contains(code) && p.Amazing == true).Count();
            }
            else if (name != null)
            {
                return _context.Products.Where(p => p.ProductName.Contains(name) && p.Amazing == true).Count();
            }
            else
            {
                return _context.Products.Where(p => p.ProductCode.Contains(code) && p.ProductName.Contains(name) && p.Amazing == true).Count();
            }
        }

        public async Task<List<Product>> SearchAmazingProducts(string code, string name, int skip, int countproduct)
        {
            if (code != null)
            {
                return await _context.Products.Include(p => p.ProductDetail).Where(p => p.ProductCode.Contains(code) && p.Amazing == true).Take(countproduct).Skip(skip).ToListAsync();
            }
            else if (name != null)
            {
                return await _context.Products.Include(p => p.ProductDetail).Where(p => p.ProductName.Contains(name) && p.Amazing == true).Take(countproduct).Skip(skip).ToListAsync();
            }
            else
            {
                return await _context.Products.Include(p => p.ProductDetail).Where(p => p.ProductCode.Contains(code) && p.ProductName.Contains(name) && p.Amazing == true).Take(countproduct).Skip(skip).ToListAsync();
            }
        }



        #endregion
        #region UnShow Product
        public async Task<List<Product>> GetUnShowProducts(int skip, int countproduct)
        {
            return await _context.Products.Where(p => p.IsShowProduct == false).Include(c => c.Category).Include(p => p.ProductDetail).OrderByDescending(p => p.ProductDetail.DateProduct).Skip(skip).Take(countproduct).ToListAsync();
        }
        public int CountUnShowProducts()
        {
            return _context.Products.Where(p => p.IsShowProduct == false).Count();
        }
        #endregion
        #region Settings
        public async Task<Setting> GetSetting()
        {
            return await _context.Settings.FirstOrDefaultAsync();
        }

        public bool UpdateSiteSetting(SiteSettingViewModel viewModel)
        {
            Setting sitesetting = _context.Settings.FirstOrDefault();
            if (sitesetting != null)
            {
                sitesetting.Name = viewModel.Name;
                sitesetting.Desc = viewModel.Desc;
                sitesetting.Tel = viewModel.Tel;
                sitesetting.Fax = viewModel.Fax;
                sitesetting.About = viewModel.About;
                sitesetting.Terms = viewModel.Terms;
                _context.SaveChanges();
                return true;
            }
            return false;

        }


        #endregion

        #region Polls
        public async Task<List<Poll>> GetPolls(int skip, int count)
        {
            return await _context.Polls.Skip(skip).Take(count).OrderByDescending(p => p.StartDate).ToListAsync();
        }


        public async Task<Poll> GetPollById(Guid id)
        {
            return await _context.Polls.FindAsync(id);
        }
        public async Task<PollOption> GetPollOptionById(Guid id)
        {
            return await _context.PollOptions.FindAsync(id);
        }


        public async Task<List<PollOption>> GetQuestionsListByPollId(Guid id)
        {
            return await _context.PollOptions.Where(p => p.PollId == id).ToListAsync();
        }

        void IAdmin.AddAndUpdatePoll(PollViewModel viewModel)
        {
            try
            {
                if (viewModel.Id == Guid.Empty)//Add
                {
                    Poll poll = new Poll()
                    {
                        Question = viewModel.Question,
                        StartDate = viewModel.StartDate,
                        EndDate = viewModel.EndDate,
                        Active = true,

                    };
                    _context.Polls.Add(poll);

                    PollOption option = new PollOption();



                    foreach (var answer in viewModel.AnswerList)
                    {
                        option.Id = new Guid();
                        option.PollId = poll.Id;
                        option.Answer = answer;
                        option.VoteCount = 0;
                        _context.PollOptions.Add(option);
                        _context.SaveChanges();
                    }
                    _context.SaveChanges();
                }
                else //Update
                {
                    var poll = _context.Polls.Find(viewModel.Id);
                    poll.Question = viewModel.Question;
                    poll.StartDate = viewModel.StartDate;
                    poll.EndDate = viewModel.EndDate;

                    _context.SaveChanges();

                }
            }
            catch
            {

                throw;
            }
        }

        void IAdmin.UpdatePollOption(Guid Id, string polloptionname)
        {
            PollOption polloption = _context.PollOptions.Find(Id);
            polloption.Answer = polloptionname;
            _context.SaveChanges();
        }

        public int CountPolls()
        {
            return _context.Polls.Count();
        }

        public void ActivatePoll(Guid id)
        {
            Poll poll = _context.Polls.Find(id);
            if (poll.Active == false)
            {
                poll.Active = true;
            }
            else
            {
                poll.Active = false;
            }

            _context.Polls.Update(poll);
            _context.SaveChanges();
        }
        public void DeletePoll(Guid id)
        {
            try
            {
                var poll = _context.Polls.Find(id);
                var polloption = _context.PollOptions.Where(p => p.PollId == id).ToList();
                foreach (var item in polloption)
                {
                    _context.PollOptions.Remove(item);
                    _context.SaveChanges();
                }

                _context.Polls.Remove(poll);
                _context.SaveChanges();

            }
            catch
            {
                throw;

            }


        }




        public int SumVotes(Guid id)
        {
            return _context.PollOptions.Where(p => p.PollId == id).Sum(p => p.VoteCount);
        }





        #endregion
        #region Slider manager
        public async Task<List<Slider>> GetSliders()
        {
            return await _context.Sliders.Include(s => s.SliderImages).OrderByDescending(s => s.Name).ToListAsync();
        }

        public async Task<Slider> GetSliderById(Guid id)
        {
            return await _context.Sliders.FindAsync(id);

        }

        public async Task<List<SliderImage>> GetSliderImages(Guid id)
        {
            return await _context.SliderImages.Include(s => s.Slider).Where(s => s.SliderId == id).OrderBy(s => s.DisplaySlide).ToListAsync();
        }

        public async Task<List<int>> GetDisplaySlides(Guid slideid)
        {
            return await _context.SliderImages.Where(s=> s.SliderId == slideid).Select(s=> s.DisplaySlide).ToListAsync();
        }


        void IAdmin.AddAndUpdateSlideImage(SlideImageViewModel model)
        {
            try
            {
                if (model.Id == Guid.Empty)//Add
                {
                    string NameSlider = GetSliderName(model.SlideId);
                    string filename = model.ImageSlide.FileName;
                    string strExt = Path.GetExtension(filename); //گرفتن پسوند فایل
                    filename = CodeGenerators.FileCode() + strExt;


                    string imgpath = "";
                    if (NameSlider == "MainSlider")
                    {
                        int width = 850;
                        int height = 425;
                        imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/sliders/main-slider/main-upload/" + filename);
                        using var image = Image.Load(model.ImageSlide.OpenReadStream());
                        image.Mutate(x => x.Resize(width, height));
                        image.Save(imgpath);
                    }
                    else
                    {
                        int width = 440;
                        int height = 220;
                        imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/sliders/sidebar-slider/sidebar-upload/" + filename);
                        using var image = Image.Load(model.ImageSlide.OpenReadStream());
                        image.Mutate(x => x.Resize(width, height));
                        image.Save(imgpath);
                    }

                    int max = 0;
                    bool ExistSlider = _context.SliderImages.Any(s => s.SliderId == model.SlideId);
                    if (ExistSlider == true)
                    {
                        max = _context.SliderImages.Where(s => s.SliderId == model.SlideId).Select(s => s.DisplaySlide).Max();
                    }
                   

                    SliderImage imageslide = new SliderImage()
                    {
                        ImageName = filename,
                        AltImage = model.AltImage,
                        SliderId = model.SlideId,
                        TitleImage = model.TitleImage,
                        DisplaySlide = max + 1,
                        Link = model.Link
                    };
                    _context.SliderImages.Add(imageslide);
                    _context.SaveChanges();
                }
                else //Update
                {
                    SliderImage slider = _context.SliderImages.Include(s => s.Slider).SingleOrDefault(s => s.Id == model.Id);


                    if (model.ImageSlide != null)
                    {
                        string filename = model.ImageSlide.FileName;

                        string strExt = Path.GetExtension(filename); //گرفتن پسوند فایل
                        filename = CodeGenerators.FileCode() + strExt;


                        string NameSlider = slider.Slider.Name;
                        string imgpath = "";
                        if (NameSlider == "MainSlider")
                        {
                            int width = 850;
                            int height = 425;
                            imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/sliders/main-slider/main-upload/" + filename);
                            using var image = Image.Load(model.ImageSlide.OpenReadStream());
                            image.Mutate(x => x.Resize(width, height));
                            image.Save(imgpath);
                            File.Delete("wwwroot/site/img/sliders/main-slider/main-upload/" + slider.ImageName);
                        }
                        else
                        {
                            int width = 440;
                            int height = 220;
                            imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/sliders/sidebar-slider/sidebar-upload/" + filename);
                            using var image = Image.Load(model.ImageSlide.OpenReadStream());
                            image.Mutate(x => x.Resize(width, height));
                            image.Save(imgpath);

                            File.Delete("wwwroot/site/img/sliders/sidebar-slider/sidebar-upload/" + slider.ImageName);
                        }
                        slider.ImageName = filename;
                        slider.AltImage = model.AltImage;

                        slider.TitleImage = model.TitleImage;
                        slider.DisplaySlide = model.DisplaySlide;
                        slider.Link = model.Link;

                        _context.SliderImages.Update(slider);
                        _context.SaveChanges();
                    }

                    else
                    {
                        slider.AltImage = model.AltImage;
                        slider.TitleImage = model.TitleImage;
                        slider.DisplaySlide = model.DisplaySlide;
                        slider.Link = model.Link;
                        _context.SliderImages.Update(slider);
                        _context.SaveChanges();
                    }

                }
            }
            catch
            {

                throw;
            }
        }









        public string GetSliderName(Guid id)
        {
            return _context.Sliders.SingleOrDefault(s => s.Id == id).Name;
        }



        public async Task<SliderImage> GetSliderImageById(Guid id)
        {
            return await _context.SliderImages.Include(s => s.Slider).SingleOrDefaultAsync(s => s.Id == id);
        }

        public bool IsNumberSlide(Guid id, int numslide)
        {
            return _context.SliderImages.Any(s => s.SliderId == id && s.DisplaySlide == numslide);
        }

        public void ActivateSlider(Guid id)
        {
            var sliderimage = _context.Sliders.Find(id);

            if (sliderimage.IsActive == true)
            {
                sliderimage.IsActive = false;
            }
            else
            {
                sliderimage.IsActive = true;
            }
            _context.SaveChanges();


        }

        public void UpdateNumberSlide(Guid id, int number)
        {
            SliderImage sliderImage = _context.SliderImages.Find(id);

            var numslide = _context.SliderImages.SingleOrDefault(s => s.DisplaySlide == number);
            numslide.DisplaySlide = sliderImage.DisplaySlide;

            sliderImage.DisplaySlide = number;

            _context.SaveChanges();           

        }

        public void DeleteSlideImage(Guid id)
        {
            try
            {
                SliderImage sliderimage = _context.SliderImages.Include(s => s.Slider).SingleOrDefault(s => s.Id == id);
                string NameSlider = sliderimage.Slider.Name;

                if (NameSlider == "MainSlider")
                {
                    File.Delete("wwwroot/site/img/sliders/main-slider/main-upload/" + sliderimage.ImageName);
                }
                else
                {
                    File.Delete("wwwroot/site/img/sliders/sidebar-slider/sidebar-upload/" + sliderimage.ImageName);
                }

                _context.SliderImages.Remove(sliderimage);
                _context.SaveChanges();


            }
            catch
            {

                throw;
            }



        }
        #endregion

        #region Banner
        public async Task<List<Banner>> GetBanners()
        {
            return await _context.Banners.ToListAsync();
        }

        public async Task<Banner> GetBannerById(Guid id)
        {
            return await _context.Banners.FindAsync(id);
        }

        void IAdmin.UpdateBanner(BannerImageViewModel viewModel)
        {          
           
            try
            {
                Banner banner = _context.Banners.Find(viewModel.Id);
                if (viewModel.Image != null)
                {
                    string filename = viewModel.Image.FileName;

                    string strExt = Path.GetExtension(filename); //گرفتن پسوند فایل
                    filename = CodeGenerators.FileCode() + strExt;                 

                    string NameSlider = banner.Name;
                    string imgpath = "";
                    if (NameSlider == "MainBanner")
                    {

                        if (banner.ImageName == "main-banner.jpg")
                        {
                            int width = 850;
                            int height = 425;
                            imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/banner/main/" + filename);
                            using var image = Image.Load(viewModel.Image.OpenReadStream());
                            image.Mutate(x => x.Resize(width, height));
                            image.Save(imgpath);
                        }
                        else
                        {
                            int width = 850;
                            int height = 425;
                            imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/banner/main/" + filename);
                            using var image = Image.Load(viewModel.Image.OpenReadStream());
                            image.Mutate(x => x.Resize(width, height));
                            image.Save(imgpath);
                            File.Delete("wwwroot/site/img/banner/main/" + banner.ImageName);
                        }


                    }
                    else
                    {

                        if (banner.ImageName == "side-banner-1.jpg" || banner.ImageName == "side-banner-2.jpg")
                        {
                            int width = 440;
                            int height = 220;
                            imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/banner/side-banner/" + filename);
                            using var image = Image.Load(viewModel.Image.OpenReadStream());
                            image.Mutate(x => x.Resize(width, height));
                            image.Save(imgpath);
                        }
                        else
                        {
                            int width = 440;
                            int height = 220;
                            imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/banner/side-banner/" + filename);
                            using var image = Image.Load(viewModel.Image.OpenReadStream());
                            image.Mutate(x => x.Resize(width, height));
                            image.Save(imgpath);
                            File.Delete("wwwroot/site/img/banner/side-banner/" + banner.ImageName);
                        }

                    }

                    banner.ImageName = filename;
                    banner.AltImage = viewModel.AltImage;
                    banner.TitleImage = viewModel.TitleImage;
                    banner.Link = viewModel.Link;
                    _context.Banners.Update(banner);
                    _context.SaveChanges();
                }
                else
                {
                    banner.AltImage = viewModel.AltImage;
                    banner.TitleImage = viewModel.TitleImage;
                    banner.Link = viewModel.Link;
                    _context.Banners.Update(banner);
                    _context.SaveChanges();

                }
            }
            catch
            {

                throw;
            }
        }


        #endregion

        #region Factors

        public async Task<List<Factor>> GetSuccessFactors(string search_mobile, int skip, int countfactor)
        {

            if (search_mobile != null)
            {
                return await _context.Factors.Where(f => f.BankName != null && f.Order.User.Mobile.Contains(search_mobile.Trim())).Include(o => o.Order).ThenInclude(u => u.User).OrderByDescending(f => f.Date).Skip(skip).Take(countfactor).ToListAsync();
            }

            return await _context.Factors.Where(f => f.BankName != null).Include(o => o.Order).ThenInclude(u => u.User).OrderByDescending(f => f.Date).Skip(skip).Take(countfactor).ToListAsync();
        }
        public int CountFactor()
        {
            return _context.Factors.Where(f => f.BankName != null).Count();
        }

        public async Task<List<OrderDetail>> GetDetailFactor(Guid id)
        {
            return await _context.OrderDetails.Include(o => o.Order).Include(p => p.Product).Where(o => o.OrderId == id).ToListAsync();
        }

        public int CountFactorWithMobile(string mobile)
        {
            return _context.Factors.Where(f => f.BankName != null && f.Order.User.Mobile.Contains(mobile.Trim())).Include(o => o.Order).ThenInclude(u => u.User).Count();
        }



        public int CountFactorByTwoDate(string fromdate, string todate)
        {
            return _context.Factors.Where(f => f.Date.CompareTo(fromdate) >= 0 && f.Date.CompareTo(todate) <= 0).Count();

        }

        public int CountFactorByFromDate(string fromdate)
        {
            return _context.Factors.Where(f => f.Date.CompareTo(fromdate) >= 0).Count();
        }

        public int CountFactorByToDate(string todate)
        {
            return _context.Factors.Where(f => f.Date.CompareTo(todate) <= 0).Count();
        }

        public async Task<List<Factor>> GetFactorsFromDate(string fromdate, int skip, int countfactor)
        {
            return await _context.Factors.Where(f => f.BankName != null && f.Date.CompareTo(fromdate) >= 0).Include(o => o.Order).ThenInclude(u => u.User).Skip(skip).Take(countfactor).ToListAsync();
        }

        public async Task<List<Factor>> GetFactorsToDate(string todate, int skip, int countfactor)
        {
            return await _context.Factors.Where(f => f.BankName != null && f.Date.CompareTo(todate) <= 0).Include(o => o.Order).ThenInclude(u => u.User).Skip(skip).Take(countfactor).ToListAsync();
        }



        public async Task<List<Factor>> GetFactorsFromAndToDate(string fromdate, string todate, int skip, int countfactor)
        {
            return await _context.Factors.Where(f => f.BankName != null && f.Date.CompareTo(fromdate) >= 0 && f.Date.CompareTo(todate) <= 0)
                .Include(o => o.Order).ThenInclude(u => u.User).Skip(skip).Take(countfactor).ToListAsync();
        }

        #endregion

        #region Color
        public async Task<List<DataLayer.Entities.Color>> GetColors()
        {
            return await _context.Colors.OrderByDescending(c => c.ColorName).ToListAsync();
        }

        public async Task<string> GetFullColorNamesForProductDetail(string colorcode)
        {
            List<string> ListColorCode = colorcode.Split(',').ToList();
            string ColorNames = "";

            for (int i = 0; i < ListColorCode.Count(); i++)
            {
                if (i == 0)
                {
                    ColorNames += _context.Colors.FirstOrDefault(c => c.ColorCode == ListColorCode[i]).ColorName;
                }
                else
                {
                    ColorNames += "-" + _context.Colors.FirstOrDefault(c => c.ColorCode == ListColorCode[i]).ColorName;
                }

            }


            //foreach (string item in ListColorCode)
            //{
            //    ColorNames += _context.Colors.FirstOrDefault(c => c.ColorCode == item).ColorName + "-";
            //}

            return await Task.FromResult(ColorNames);
        }

        public async Task<DataLayer.Entities.Color> GetColorById(Guid id)
        {
            return await _context.Colors.FindAsync(id);
        }

        void IAdmin.AddOrUpdateColor(ColorViewModel model)
        {
            try
            {
                if (model.Id == Guid.Empty)//Add
                {
                    DataLayer.Entities.Color color = new DataLayer.Entities.Color()
                    {
                        ColorId = Guid.NewGuid(),
                        ColorCode = model.ColorCode,
                        ColorName = model.ColorName,

                    };
                    _context.Colors.Add(color);
                    _context.SaveChanges();
                }
                else
                {
                    var color = _context.Colors.Find(model.Id);
                    color.ColorCode = model.ColorCode;
                    color.ColorName = model.ColorName;
                    _context.Colors.Update(color);
                    _context.SaveChanges();
                }
            }
            catch
            {

                throw;
            }
        }

        public bool ExistColorName(ColorViewModel model)
        {
            if (model.Id == Guid.Empty)
            {
                return _context.Colors.Any(c => c.ColorName == model.ColorName);
            }
            else
            {
                return _context.Colors.Any(c => c.ColorName == model.ColorName && c.ColorId != model.Id);
            }
        }

        public bool ExistColorCode(ColorViewModel model)
        {
            if (model.Id == Guid.Empty)
            {
                return _context.Colors.Any(c => c.ColorCode == model.ColorCode);
            }
            else
            {
                return _context.Colors.Any(c => c.ColorCode == model.ColorCode && c.ColorId != model.Id);
            }
        }

        public void RemoveColor(Guid id)
        {
            var color = _context.Colors.Find(id);
            _context.Colors.Remove(color);
            _context.SaveChanges();
        }





        #endregion
        #region Theme
        public async Task<Theme> GetTheme()
        {
            return await _context.Themes.SingleOrDefaultAsync();
        }

        public void UpdateLogoImage(LogoViewModel model)
        {
            try
            {
                Theme theme = _context.Themes.Single(t => t.Id == model.Id);


                File.Delete("wwwroot/site/img/logo/" + theme.Logo);

                string filename = model.LogoImage.FileName;

                string strExt = Path.GetExtension(filename); //گرفتن پسوند فایل
                filename = CodeGenerators.FileCode() + strExt;
                string imgpath = "";
                int width = 140;
                int height = 40;
                imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/site/img/logo/" + filename);
                using var image = Image.Load(model.LogoImage.OpenReadStream());
                image.Mutate(x => x.Resize(width, height));
                image.Save(imgpath);
                theme.Logo = filename;
                _context.SaveChanges();
            }
            catch 
            {

                throw;
            }
           

        }


        #endregion
        #region City
        public async Task<List<City>> GetStates()
        {
            return await _context.Cities.Where(c => c.StateId == null).OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<City> GetStateById(Guid id)
        {
            return await _context.Cities.FindAsync(id);
        }

        void IAdmin.AddAndUpdateState(CityViewModel model)
        {
            if (model.Id == Guid.Empty)// Add
            {
                City city = new City()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    StateId = null,
                };
                _context.Cities.Add(city);
                _context.SaveChanges();

            }
            else // Update
            {
                var city = _context.Cities.Find(model.Id);

                city.Name = model.Name;
                _context.Cities.Update(city);
                _context.SaveChanges();
            }
        }

        public bool ExistStateName(CityViewModel model)
        {
            if (model.Id == Guid.Empty)
            {
                return _context.Cities.Where(c=> c.StateId == null).Any(c => c.Name == model.Name);
            }
            else
            {
                return _context.Cities.Where(c => c.StateId == null).Any(c => c.Id != model.Id && c.Name == model.Name);
            }
        }

        public void AddExcelState(CityViewModel model)
        {
            City city = new City()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                StateId = null,
            };
            _context.Cities.Add(city);
            _context.SaveChanges();
        }

        public bool ExistStateNameExcel(string name)
        {
            return _context.Cities.Where(c => c.StateId == null).Any(c => c.Name == name);
        }

        public async Task<List<City>> GetCities(Guid id)
        {
            return await _context.Cities.Where(c => c.StateId == id).OrderBy(c=> c.Name).ToListAsync();
        }

        public void AddAndUpdateCity(CityViewModel model)
        {
            if (model.Id == Guid.Empty)// Add
            {
                City city = new City()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    StateId = model.StateId,
                };
                _context.Cities.Add(city);
                _context.SaveChanges();

            }
            else // Update
            {
                var city = _context.Cities.Find(model.Id);

                city.Name = model.Name;
                city.StateId = model.StateId;
                _context.Cities.Update(city);
                _context.SaveChanges();
            }
        }
        public bool ExistCityName(CityViewModel model)
        {
            if (model.Id == Guid.Empty)
            {
                return _context.Cities.Where(c => c.StateId == model.StateId).Any(c => c.Name == model.Name);
            }
            else
            {
                return _context.Cities.Where(c => c.StateId == model.StateId).Any(c => c.Id != model.Id && c.Name == model.Name);
            }
        }

        void IAdmin.RemoveState(Guid id)
        {
            var state = _context.Cities.Find(id);
            bool havescities = _context.Cities.Any(c => c.StateId == id);

            if (havescities == true)
            {
                var citeslist = _context.Cities.Where(c => c.StateId == id).ToList();
                _context.RemoveRange(citeslist);
            }

           
            _context.Remove(state);
            _context.SaveChanges();


        }

        public bool ExistCityNameExcel(Guid Id, string name)
        {
            return _context.Cities.Where(c => c.StateId == Id).Any(c => c.Name == name);
        }

        public void AddExcelCity(CityViewModel model)
        {
            City city = new City()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                StateId = model.StateId,
            };
            _context.Cities.Add(city);
            _context.SaveChanges();
        }

        Guid? IAdmin.RemoveCity(Guid id)
        {
            City city = _context.Cities.SingleOrDefault(c=> c.Id == id);
            _context.Remove(city);
            _context.SaveChanges();

           

            return city.StateId;
        }

        #endregion
    }
}
