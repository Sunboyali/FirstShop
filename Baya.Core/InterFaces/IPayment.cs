using Baya.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Baya.Core.InterFaces
{
   public interface IPayment : IDisposable
    {
        void AddFactor(Guid userid, string orderNumber);
        bool UpdateFactor(Guid userid, string orderNumber);
        Guid GetFactorById(string orderNumber);

        int GetTotalPriceFactor(Guid factorId);
        void UpdatePayment(Guid id, string desc, string bank, string trace, string refId);

        Task<Factor> GetFactor(Guid id);
    }
}
