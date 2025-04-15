using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Enums;
using Mzad_Palestine_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice> GetByIdAsync(int id)
        {
            return await _context.Invoices.FindAsync(id);
        }

        public async Task<IEnumerable<Invoice>> GetAllAsync()
        {
            return await Task.FromResult(_context.Invoices.ToList());
        }

        public async Task<Invoice> AddAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice> UpdateAsync(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var invoice = await GetByIdAsync(id);
            if (invoice == null)
                return false;

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Invoice>> GetByUserIdAsync(int userId)
        {
            return await Task.FromResult(_context.Invoices
                .Where(i => i.UserId == userId)
                .ToList());
        }

        public async Task<IEnumerable<Invoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(_context.Invoices
                .Where(i => i.IssuedAt >= startDate && i.IssuedAt <= endDate)
                .ToList());
        }

        public async Task<decimal> GetTotalAmountByUserIdAsync(int userId)
        {
            return await Task.FromResult(_context.Invoices
                .Where(i => i.UserId == userId)
                .Sum(i => i.Amount));
        }

        public async Task<bool> MarkAsPaidAsync(int id)
        {
            var invoice = await GetByIdAsync(id);
            if (invoice == null)
                return false;

            invoice.Status = InvoiceStatus.Paid;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelInvoiceAsync(int id)
        {
            var invoice = await GetByIdAsync(id);
            if (invoice == null)
                return false;

            invoice.Status = InvoiceStatus.Canceled;
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 