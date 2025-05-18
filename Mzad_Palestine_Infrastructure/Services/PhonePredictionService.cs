using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class PhonePredictionService : IPhonePredictionService
    {
        private readonly ApplicationDbContext _context;

        public PhonePredictionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<double> PredictPrice(Phone phone)
        {
            try
            {
                // حساب السعر بناءً على المواصفات
                double base_price = 1000;
                double price = (
                    phone.BatteryCapacity * 0.1 +    // كل 1000 مللي أمبير تضيف 100 شيكل
                    phone.DisplaySize * 100 +        // كل بوصة تضيف 100 شيكل
                    phone.Storage * 0.5 +            // كل 1 جيجا تخزين تضيف 0.5 شيكل
                    phone.Ram * 50 +                 // كل 1 جيجا رام تضيف 50 شيكل
                    phone.RefreshRate * 2 +          // كل 1 هرتز معدل تحديث تضيف 2 شيكل
                    int.Parse(phone.FrontCamera.Replace("MP", "")) * 10 +  // كل 1 ميجابكسل كاميرا أمامية تضيف 10 شيكل
                    int.Parse(phone.RearCamera.Split('+')[0].Replace("MP", "").Trim()) * 5 +  // كل 1 ميجابكسل كاميرا خلفية تضيف 5 شيكل
                    phone.ChargingSpeed * 3 +        // كل 1 واط سرعة شحن تضيف 3 شيكل
                    base_price                       // السعر الأساسي
                );

                // إضافة علاوة إذا كان الهاتف يدعم توسيع الرام
                if (phone.RamExpandable)
                {
                    price *= 1.1; // زيادة 10% للسعر
                }

                return Math.Round(price, 2);
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء التنبؤ بالسعر: {ex.Message}");
            }
        }

        public async Task<Phone> SavePhoneSpecs(Phone phone)
        {
            try
            {
                phone.CreatedAt = DateTime.Now;
                _context.Phones.Add(phone);
                await _context.SaveChangesAsync();
                return phone;
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء حفظ مواصفات الهاتف: {ex.Message}");
            }
        }
    }
} 