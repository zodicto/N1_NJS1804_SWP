using ODTLearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODTLearning.BLL.Repositories
{
    public interface INotificationRepository
    {
        public Task<ApiResponse<object>> GetAllNotification(string id);
    }
}
