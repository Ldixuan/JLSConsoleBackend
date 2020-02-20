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
    }
}
