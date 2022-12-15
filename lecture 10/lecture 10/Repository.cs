using lecture_10.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lecture_10
{
    public class Repository : IRepository
    {
        private readonly OOP2022Context _context;

        public Repository(OOP2022Context context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return _context.Set<T>();
        }

        public async Task<T> GetById<T>(int id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task Create<T>(T entity) where T : class
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> Update<T>(T entity) where T : class
        {
            if (entity is null) return 0;
            _context.Set<T>().Update(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete<T>(T entity) where T : class
        {
            if (entity is null) return 0;
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }
    }
}
