using JLSDataAccess.Interfaces;
using JLSDataModel.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace JLSDataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly JlsDbContext context;
        private readonly string _defaultLang;

        public OrderRepository(JlsDbContext context, IConfiguration config)
        {
            this.context = context;
            _defaultLang = config.GetValue<string>("Lang:DefaultLang");
        }

        public async Task<List<OrdersListViewModel>> GetAllOrdersWithInterval(string lang, int intervalCount, int size, string orderActive, string orderDirection)
        {
            var request = (from order in context.OrderInfo
                           join user in context.Users on order.UserId equals user.Id
                           select new OrdersListViewModel
                           {
                               Id = order.Id,
                               OrderReferenceCode = order.OrderReferenceCode,
                               EntrepriseName = user.EntrepriseName,
                               UserName = user.UserName,
                               TotalPrice = order.TotalPrice,
                               Date = order.CreatedOn,
                               StatusReferenceItemLabel = (from ri in context.ReferenceItem
                                                          where ri.Id == order.StatusReferenceItemId
                                                          from rl in context.ReferenceLabel
                                                          .Where(rl => rl.ReferenceItemId == ri.Id && rl.Lang == lang).DefaultIfEmpty()
                                                          select rl.Label).FirstOrDefault()
                           });

            if (orderActive == "null" || orderActive == "undefined" || orderDirection == "null")
            {
                return await request.Skip(intervalCount * size).Take(size).ToListAsync();
            }

            Expression<Func<OrdersListViewModel, object>> funcOrder;

            switch (orderActive)
            {
                case "id":
                    funcOrder = order => order.Id;
                    break;
                case "reference":
                    funcOrder = order => order.OrderReferenceCode;
                    break;
                case "name":
                    funcOrder = order => order.UserName;
                    break;
                case "entrepriseName":
                    funcOrder = order => order.EntrepriseName;
                    break;
                case "total":
                    funcOrder = order => order.TotalPrice;
                    break;
                case "status":
                    funcOrder = order => order.StatusReferenceItemLabel;
                    break;
                case "date":
                    funcOrder = order => order.Date;
                    break;
                default:
                    funcOrder = order => order.Id;
                    break;
            }

            if (orderDirection == "asc")
            {
                request = request.OrderBy(funcOrder);
            }
            else
            {
                request = request.OrderByDescending(funcOrder);
            }

            var result = await request.Skip(intervalCount * size).Take(size).ToListAsync();


            return result;
        }

        public async Task<OrderViewModel> GetOrderById(long id,string lang)
        {
            var result = await (from order in context.OrderInfo
                          where order.Id == id
                          join sa in context.Adress on order.ShippingAdressId equals sa.Id
                          join fa in context.Adress on order.FacturationAdressId equals fa.Id
                          join user in context.Users on order.UserId equals user.Id
                          join ris in context.ReferenceItem on order.StatusReferenceItemId equals ris.Id
                          from rls in context.ReferenceLabel.Where(rls => rls.ReferenceItemId == ris.Id
                          && rls.Lang == lang).Take(1).DefaultIfEmpty()
                          select new OrderViewModel
                          {
                              OrderReferenceCode = order.OrderReferenceCode,
                              PaymentInfo = order.PaymentInfo,
                              TaxRate = order.TaxRate,
                              TotalPrice = order.TotalPrice,
                              AdminRemark = order.AdminRemark,
                              ClientRemark = order.ClientRemark,
                              StatusLabel = rls.Label,
                              StatusReferenceItem = ris,
                              User = new UserViewModel 
                              { 
                                    Id = user.Id,
                                    Email = user.Email,
                                    EntrepriseName = user.EntrepriseName,
                                    Name = user.UserName,
                                    Telephone = user.PhoneNumber
                              },
                              FacturationAdress = fa,
                              ShippingAdress = sa,
                              Products = (from po in context.OrderProduct
                                          where po.OrderId == order.Id
                                          join rip in context.ReferenceItem on po.ReferenceId equals rip.Id
                                          join pi in context.Product on rip.Id equals pi.ReferenceItemId
                                          from img in context.ProductPhotoPath.Where(img => img.ProductId == pi.Id)
                                          .Take(1).DefaultIfEmpty()
                                          from rlp in context.ReferenceLabel.Where(rlp => rlp.ReferenceItemId == rip.Id
                                          && rlp.Lang == lang).Take(1).DefaultIfEmpty()
                                          select new OrderProductViewModel
                                          { 
                                            Id = pi.Id,
                                            Image = img.Path,
                                            Name = rlp.Label,
                                            Price = pi.Price,
                                            Quantity = po.Quantity,
                                            ReferenceCode = rip.Code
                                          }).ToList()
                          }).FirstOrDefaultAsync();

            return result;
        }
    } 
}
