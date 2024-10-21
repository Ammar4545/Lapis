using Lapis_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lapis_DataAcess.Repository.IRepository
{
    public interface IApplicationTypeRepository: IRepository<ApplicationType>
    {
        void Update(ApplicationType applicationType);
    }
}
