using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Infrastructure.Data;
using Mzad_Palestine_Infrastructure.Repositories.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Mzad_Palestine_Infrastructure.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Transaction> GetByIdAsync(int id)
        {
            return await _context.Transactions
                .Include(t => t.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TransactionId == id);
        }

        public override async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _context.Transactions
                .Include(t => t.User)
                .AsNoTracking()
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync(int userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .Include(t => t.User)
                .AsNoTracking()
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public override async Task DeleteAsync(Transaction transaction)
        {
            _context.Entry(transaction).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public override void Update(Transaction transaction)
        {
            var existingTransaction = _context.Transactions.Local.FirstOrDefault(t => t.TransactionId == transaction.TransactionId);
            if (existingTransaction != null)
            {
                _context.Entry(existingTransaction).State = EntityState.Detached;
            }
            _context.Entry(transaction).State = EntityState.Modified;
        }

        public async Task<Transaction> GetByNameAsync(string name)
        {
            throw new NotImplementedException("Transactions cannot be searched by name.");
        }
    }
}
