using Microsoft.EntityFrameworkCore.Query;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
using OnionProject.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Infrastructure.ConcreteRepositories
{
    public class ContactMessageRepo : IContactMessageRepo
    {
        private readonly AppDbContext _context;

        public ContactMessageRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ContactMessage message)
        {
            await _context.ContactMessages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public Task<bool> Any(Expression<Func<ContactMessage, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task Create(ContactMessage entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(ContactMessage entity)
        {
            throw new NotImplementedException();
        }

        public Task<ContactMessage> GetDefault(Expression<Func<ContactMessage, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<List<ContactMessage>> GetDefaults(Expression<Func<ContactMessage, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetFilteredFirstOrDefault<TResult>(Expression<Func<ContactMessage, TResult>> select, Expression<Func<ContactMessage, bool>> where, Func<IQueryable<ContactMessage>, IOrderedQueryable<ContactMessage>> orderBy = null, Func<IQueryable<ContactMessage>, IIncludableQueryable<ContactMessage, object>> include = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<ContactMessage, TResult>> select, Expression<Func<ContactMessage, bool>> where, Func<IQueryable<ContactMessage>, IOrderedQueryable<ContactMessage>> orderBy = null, Func<IQueryable<ContactMessage>, IIncludableQueryable<ContactMessage, object>> include = null)
        {
            throw new NotImplementedException();
        }

        public Task Update(ContactMessage entity)
        {
            throw new NotImplementedException();
        }

        // Diğer işlemler...
    }

}
