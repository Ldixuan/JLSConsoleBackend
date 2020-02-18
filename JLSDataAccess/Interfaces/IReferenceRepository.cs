using JLSDataModel.Models;
using JLSDataModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JLSDataAccess.Interfaces
{
    public interface IReferenceRepository
    {

        Task<List<ReferenceItemViewModel>> GetReferenceItemsByCategoryLabels(string shortLabel, string lang);

        Task<List<ReferenceItem>> GetReferenceItemsByCategoryIds(string categoryIds, string lang);

        Task<List<ReferenceItem>> GetReferenceItemsById(long referenceId, string lang);

        Task<List<ReferenceItem>> GetReferenceItemsByCode(string referencecode, string lang);

        Task<List<ReferenceCategory>> GetAllReferenceCategory();

        Task<List<ReferenceItemViewModel>> GetAllReferenceItem();

        Task<int> updateItem(ReferenceItem item, List<ReferenceLabel> labels);

        List<ReferenceLabel> CheckLabels(List<ReferenceLabel> labels, long referenceItemId);

        Task<int> updateCategory(ReferenceCategory category);

    }
}
