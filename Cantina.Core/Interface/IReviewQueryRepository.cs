using Cantina.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.Interface
{
    public interface IReviewQueryRepository
    {
        public Task<List<ReviewView>> GetAllAsync();
    }
}
