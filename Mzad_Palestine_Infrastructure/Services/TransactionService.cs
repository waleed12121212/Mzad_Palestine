using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            try
            {
                await _unitOfWork.Transactions.AddAsync(transaction);
                await _unitOfWork.CompleteAsync();
                return transaction;
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء إضافة المعاملة: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
                if (transaction == null)
                    return false;

                await _unitOfWork.Transactions.DeleteAsync(transaction);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء حذف المعاملة: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            try
            {
                return await _unitOfWork.Transactions.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء جلب جميع المعاملات: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var transactions = await _unitOfWork.Transactions.FindAsync(t => 
                    t.TransactionDate >= startDate && 
                    t.TransactionDate <= endDate);
                return transactions;
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء جلب المعاملات حسب التاريخ: {ex.Message}");
            }
        }

        public async Task<Transaction> GetByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.Transactions.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء جلب المعاملة: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId)
        {
            try
            {
                return await _unitOfWork.Transactions.GetUserTransactionsAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء جلب معاملات المستخدم: {ex.Message}");
            }
        }

        public async Task<decimal> GetTotalAmountByUserIdAsync(int userId)
        {
            try
            {
                var transactions = await GetByUserIdAsync(userId);
                return transactions.Sum(t => t.Amount);
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء حساب المبلغ الإجمالي: {ex.Message}");
            }
        }

        public async Task<bool> ProcessPaymentAsync(int transactionId)
        {
            try
            {
                var transaction = await _unitOfWork.Transactions.GetByIdAsync(transactionId);
                if (transaction == null)
                    return false;

                if (transaction.Status != "Pending")
                    return false;

                transaction.Status = "Completed";
                _unitOfWork.Transactions.Update(transaction);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء معالجة الدفع: {ex.Message}");
            }
        }

        public async Task<bool> RefundTransactionAsync(int transactionId)
        {
            try
            {
                var transaction = await _unitOfWork.Transactions.GetByIdAsync(transactionId);
                if (transaction == null)
                    return false;

                if (transaction.Status != "Completed")
                    return false;

                transaction.Status = "Refunded";
                _unitOfWork.Transactions.Update(transaction);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء استرداد المعاملة: {ex.Message}");
            }
        }

        public async Task<Transaction> UpdateAsync(Transaction transaction)
        {
            try
            {
                var existingTransaction = await _unitOfWork.Transactions.GetByIdAsync(transaction.TransactionId);
                if (existingTransaction == null)
                    throw new Exception("المعاملة غير موجودة");

                // Update the existing entity's properties
                existingTransaction.Amount = transaction.Amount;
                existingTransaction.TransactionType = transaction.TransactionType;
                existingTransaction.Status = transaction.Status;
                existingTransaction.Description = transaction.Description;

                _unitOfWork.Transactions.Update(existingTransaction);
                await _unitOfWork.CompleteAsync();
                return existingTransaction;
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء تحديث المعاملة: {ex.Message}");
            }
        }
    }
} 