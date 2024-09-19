using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Services.AbstractServices
{
    public interface IAuthorService
    {
        Task Create(CreateAuthorDTO model);

        Task Update(UpdateAuthorDTO model);

        Task Delete(int id);

        Task<List<AuthorVm>> GetAuthors();
        Task<AuthorDetailVm> GetDetail(int id);
        Task<UpdateAuthorDTO> GetById(int id);
        Task<bool> IsAuthorExists(string firstName, string lastName);
        Task<AuthorDetailVm> GetFullName(string firstName, string lastName);
    }
}
