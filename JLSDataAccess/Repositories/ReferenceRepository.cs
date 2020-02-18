using JLSDataModel.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using JLSDataModel.ViewModels;
using JLSDataAccess.Interfaces;
using Microsoft.Extensions.Configuration;

namespace JLSDataAccess.Repositories
{
    public class ReferenceRepository : IReferenceRepository
    {
        private readonly JlsDbContext db;
        private readonly string _defaultLang;

        public ReferenceRepository(JlsDbContext context, IConfiguration config)
        {
            db = context;
            _defaultLang = config.GetValue<string>("Lang:DefaultLang");
        }
        public Task<List<ReferenceItem>> GetReferenceItemsByCategoryIds(string categoryIds, string lang)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ReferenceItemViewModel>> GetReferenceItemsByCategoryLabels(string shortLabels, string lang)
        {
            List<string> referenceLabelList = new List<string>(shortLabels.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries));

            var result = (from ri in db.ReferenceItem
                          join rc in db.ReferenceCategory on ri.ReferenceCategoryId equals rc.Id
                          from rl in db.ReferenceLabel.Where(p => p.ReferenceItemId == ri.Id && p.Lang == lang).DefaultIfEmpty()
                          where referenceLabelList.Contains(rc.ShortLabel)
                          select new ReferenceItemViewModel
                          {
                              Id = ri.Id,
                              Code = ri.Code,
                              Value = ri.Value,
                              Order = ri.Order,
                              ParentId = ri.ParentId,
                              Label = rl.Label,
                              Lang = rl.Lang,
                              Category = rc.ShortLabel,
                              Validity = ri.Validity
                          });
            return await result.ToListAsync<ReferenceItemViewModel>();
        }



        public Task<List<ReferenceItem>> GetReferenceItemsByCode(string referencecode, string lang)
        {
            throw new NotImplementedException();
        }

        public Task<List<ReferenceItem>> GetReferenceItemsById(long referenceId, string lang)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ReferenceItemViewModel>> GetAllReferenceItem()
        {
            var result = await (from ri in db.ReferenceItem
                          join rc in db.ReferenceCategory on ri.ReferenceCategoryId equals rc.Id
                          select new ReferenceItemViewModel
                          {
                              Id = ri.Id,
                              Code = ri.Code,
                              Value = ri.Value,
                              Order = ri.Order,
                              ParentId = ri.ParentId,
                              Category = rc.ShortLabel,
                              ReferenceCategoryId = rc.Id,
                              Validity = ri.Validity,
                              Labels = (from rl in db.ReferenceLabel
                                             where rl.ReferenceItemId == ri.Id
                                             select rl).ToList(),
                            }).ToListAsync<ReferenceItemViewModel>();

            return result;
        }

        public async Task<List<ReferenceCategory>> GetAllReferenceCategory()
        {
            var result = await (from rc in db.ReferenceCategory
                                select rc).ToListAsync();

            return result;
        }

        public async Task<List<ReferenceCategory>> GetAllValidityReferenceCategory()
        {
            var result = await (from rc in db.ReferenceCategory
                                where rc.Validity == true
                                select rc).ToListAsync();

            return result;
        }

        public async Task<int> updateItem(ReferenceItem item, List<ReferenceLabel> labels)
        {
            if(item.Id == 0)
            {
                db.ReferenceItem.Add(item);
                await db.SaveChangesAsync();

                labels = CheckLabels(labels, item.Id);
                foreach (ReferenceLabel label in labels)
                {
                    db.ReferenceLabel.Add(label);
                }
            }
            else
            {
                db.ReferenceItem.Update(item);

                labels = CheckLabels(labels, item.Id);
                foreach (ReferenceLabel label in labels)
                {
                    db.ReferenceLabel.Update(label);
                }

            }

            await db.SaveChangesAsync();

            return 1;
        }

        public async Task<int> updateCategory(ReferenceCategory category)
        {
            if(category.Id == 0)
            {
                db.ReferenceCategory.Add(category);
            }
            else
            {
                db.ReferenceCategory.Update(category);
            }

            await db.SaveChangesAsync();

            return 1;
        }

        public List<ReferenceLabel> CheckLabels(List<ReferenceLabel> labels, long referenceItemId)
        {

            string defaultLabel = labels.Find(label => label.Lang.Equals(_defaultLang)).Label;
            foreach (ReferenceLabel label in labels)
            {
                if (label.Label == "")
                {
                    label.Label = defaultLabel;
                }
                label.ReferenceItemId = referenceItemId;
            }
            return labels;
        }

    }
}
