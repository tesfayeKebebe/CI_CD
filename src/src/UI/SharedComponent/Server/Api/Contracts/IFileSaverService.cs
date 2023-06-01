using SharedComponent.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedComponent.Server.Api.Contracts
{
    public interface IFileSaverService
    {
         Task<List<FileClass>?> PostFiles(MultipartFormDataContent files);
        Task<string> Delete(string storedFileName);
        Task<FileClass?> PostResultFiles(MultipartFormDataContent files);
        Task<string> PostFiles(List<FileClass> files);
        Task<string> DeleteResultFile(string storedFileName);


    }
}
