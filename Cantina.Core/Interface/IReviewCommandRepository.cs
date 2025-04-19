using Cantina.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.Interface
{
    public interface IReviewCommandRepository
    {
        Task AddAsync(ReviewDto review);
    }
}
