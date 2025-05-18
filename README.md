# مزاد فلسطين - Mzad Palestine

<p align="center">
  <img src="https://github.com/waleed12121212/Mzad_Palestine/raw/main/logo.png" alt="Mzad Palestine Logo" width="200"/>
</p>

## نظرة عامة | Overview
مزاد فلسطين هو منصة مزادات إلكترونية متكاملة تتيح للمستخدمين إنشاء وإدارة المزادات عبر الإنترنت. يوفر النظام تجربة مستخدم سلسة وآمنة لعمليات البيع والشراء من خلال نظام المزايدة.

Mzad Palestine is a comprehensive online auction platform that allows users to create and manage auctions online. The system provides a smooth and secure user experience for buying and selling through the bidding system.

## المميزات الرئيسية | Key Features

### نظام المزايدة | Auction System
- **المزايدة اليدوية والتلقائية**: دعم المزايدات اليدوية والتلقائية مع إشعارات فورية
- **Manual & Automatic Bidding**: Support for both manual and automatic bidding with instant notifications
- **السعر الاحتياطي**: إمكانية تحديد سعر احتياطي للمزاد
- **Reserve Price**: Ability to set reserve prices for auctions
- **زيادة المزايدة**: تحديد قيمة الزيادة في المزايدة
- **Bid Increment**: Setting bid increment values

### إدارة المستخدمين | User Management
- **التسجيل والتوثيق**: نظام تسجيل وتوثيق متكامل
- **Registration & Authentication**: Comprehensive registration and authentication system
- **الأدوار المختلفة**: دعم أدوار متعددة (مستخدم عادي، بائع، مشتري، مدير، مشرف)
- **Multiple Roles**: Support for various roles (Regular User, Seller, Buyer, Admin, Moderator)

### الدفع والأمان | Payment & Security
- **طرق دفع متعددة**: دعم وسائل دفع متنوعة
- **Multiple Payment Methods**: Support for various payment options
- **نظام الضمان**: ضمان المعاملات المالية وحماية المستخدمين
- **Escrow System**: Financial transaction guarantees and user protection

### الميزات الإضافية | Additional Features
- **نظام التقييم والمراجعة**: تقييم البائعين والمشترين
- **Rating & Review System**: Seller and buyer ratings
- **نظام الإشعارات**: إشعارات فورية للتحديثات والعروض
- **Notification System**: Real-time notifications for updates and offers
- **نظام الدعم**: نظام تذاكر دعم متكامل
- **Support System**: Integrated support ticket system
- **نظام التقارير**: إدارة الشكاوى والمخالفات
- **Reporting System**: Complaint and violation management
- **قائمة المراقبة**: متابعة المزادات المفضلة
- **Watchlist**: Follow favorite auctions
- **نظام النزاعات**: حل النزاعات بين المستخدمين
- **Dispute System**: Resolve disputes between users
- **نظام التصنيفات**: تنظيم المنتجات حسب الفئات
- **Categorization System**: Organize products by categories
- **نظام التوقعات**: توقع أسعار السيارات والهواتف باستخدام الذكاء الاصطناعي
- **Prediction System**: Car and mobile phone price prediction using AI

## التقنيات المستخدمة | Technologies Used
- **Backend**: ASP.NET Core 8.0 Web API
- **ORM**: Entity Framework Core
- **Database**: SQL Server
- **Authentication**: ASP.NET Core Identity
- **API Documentation**: Swagger/OpenAPI
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Machine Learning**: ML.NET (للتنبؤ بأسعار السيارات والهواتف | for car and mobile phone price prediction)

## هيكل المشروع | Project Structure
```
Mzad_Palestine/
├── Mzad_Palestine_API/              # واجهة برمجة التطبيقات | Web API
│   ├── Controllers/                 # وحدات التحكم | Controllers
│   ├── Middleware/                  # الوسائط البرمجية | Middleware
│   └── Properties/                  # إعدادات التشغيل | Launch settings
├── Mzad_Palestine_Core/             # طبقة النواة | Core layer
│   ├── DTO's/                       # كائنات نقل البيانات | Data Transfer Objects
│   ├── Enums/                       # التعدادات | Enumerations
│   ├── Interfaces/                  # الواجهات | Interfaces
│   ├── Models/                      # النماذج | Models
│   │   └── ML/                      # نماذج التعلم الآلي | Machine Learning Models
│   └── Validation/                  # التحقق من الصحة | Validation
└── Mzad_Palestine_Infrastructure/   # طبقة البنية التحتية | Infrastructure layer
    ├── Data/                        # سياق قاعدة البيانات | Database Context
    ├── Repositories/                # المستودعات | Repositories
    └── Services/                    # الخدمات | Services
```

## نماذج البيانات | Data Models
النظام يحتوي على عدة نماذج بيانات رئيسية تشمل:
The system contains several key data models including:

- **User**: المستخدمون وبيانات الحسابات | Users and account data
- **Listing**: القوائم والمنتجات المعروضة | Listings and products
- **Auction**: المزادات وتفاصيلها | Auctions and their details
- **Bid**: العروض المقدمة | Bids submitted
- **AutoBid**: نظام المزايدة التلقائية | Automatic bidding system
- **Payment**: المدفوعات والمعاملات المالية | Payments and financial transactions
- **Invoice**: الفواتير | Invoices
- **Category**: تصنيفات المنتجات | Product categories
- **Report**: التقارير والشكاوى | Reports and complaints
- **Dispute**: النزاعات | Disputes
- **Review**: التقييمات والمراجعات | Ratings and reviews
- **Message**: نظام المراسلة | Messaging system
- **Notification**: الإشعارات | Notifications
- **CustomerSupportTicket**: تذاكر الدعم | Support tickets
- **Watchlist**: قائمة المراقبة | Watchlist
- **Subscription**: الاشتراكات | Subscriptions
- **Transaction**: سجل المعاملات | Transaction records

## المتطلبات | Requirements
- .NET 8.0 SDK
- SQL Server 2019+
- Visual Studio 2022+

## التثبيت والتشغيل | Installation & Running
1. استنساخ المشروع | Clone the project
```bash
git clone https://github.com/waleed12121212/Mzad_Palestine.git
cd Mzad_Palestine
```

2. إعداد قاعدة البيانات | Database setup
- تعديل سلسلة الاتصال في `appsettings.json` | Edit connection string in `appsettings.json`
- تنفيذ الترحيلات | Run migrations:
```bash
dotnet ef database update
```

3. تشغيل المشروع | Run the project
```bash
dotnet run --project Mzad_Palestine_API
```

4. فتح واجهة Swagger | Open Swagger UI
```
https://localhost:5001/swagger
```

## ميزات الذكاء الاصطناعي | AI Features
يتضمن النظام نماذج تنبؤ لتقدير أسعار:
The system includes prediction models for estimating prices of:

- **السيارات**: تقدير قيمة السيارة بناءً على خصائصها | Cars: estimate car value based on its characteristics
- **الهواتف المحمولة**: تقدير سعر الهاتف بناءً على مواصفاته | Mobile phones: estimate phone price based on specifications

## الوثائق | Documentation
- واجهة API: `/swagger` | API interface: `/swagger`
- مخطط قاعدة البيانات: `docs/database-schema.pdf` | Database schema: `docs/database-schema.pdf`

## المساهمة | Contributing
نرحب بمساهماتكم! يرجى اتباع الخطوات التالية:
We welcome your contributions! Please follow these steps:

1. عمل Fork للمشروع | Fork the project
2. إنشاء فرع جديد للميزة (`git checkout -b feature/amazing-feature`)
3. تنفيذ التغييرات مع الالتزام بمعايير الكود | Implement changes while adhering to code standards
4. إضافة الاختبارات المناسبة | Add appropriate tests
5. تقديم Pull Request | Submit a Pull Request

## الترخيص | License
هذا المشروع مرخص تحت رخصة MIT. انظر ملف `LICENSE` للمزيد من التفاصيل.
This project is licensed under the MIT License. See the `LICENSE` file for more details.

## الاتصال والدعم | Contact & Support
للمساعدة والاستفسارات، يرجى:
For help and inquiries, please:

- فتح issue في GitHub | Open an issue on GitHub
- التواصل عبر البريد الإلكتروني: support@mzad-palestine.com | Contact via email: support@mzad-palestine.com
- زيارة موقعنا: https://mzad-palestine.com | Visit our website: https://mzad-palestine.com

## شكر خاص | Special Thanks
شكر خاص لجميع المساهمين والداعمين لهذا المشروع.
Special thanks to all contributors and supporters of this project.

# نموذج التنبؤ بأسعار الهواتف المحمولة

هذا المشروع يستخدم خوارزمية KNN للتنبؤ بأسعار الهواتف المحمولة بناءً على مواصفاتها.

## المتطلبات

- Python 3.6 أو أحدث
- المكتبات المذكورة في ملف requirements.txt

## التثبيت

1. قم بتثبيت المتطلبات:
```bash
pip install -r requirements.txt
```

2. تأكد من وجود ملف البيانات train.csv في نفس المجلد

## الاستخدام

1. قم بتشغيل النموذج:
```bash
python mobile_price_prediction.py
```

2. سيتم عرض:
   - دقة النموذج
   - تقرير التصنيف
   - مصفوفة الارتباك

3. سيتم حفظ النموذج المدرب في ملف mobile_price_model.pkl

## هيكل البيانات

يجب أن يحتوي ملف البيانات train.csv على الأعمدة التالية:
- battery_power: قوة البطارية
- blue: دعم البلوتوث
- clock_speed: سرعة المعالج
- dual_sim: دعم الشريحتين
- fc: كاميرا أمامية
- four_g: دعم 4G
- int_memory: الذاكرة الداخلية
- m_dep: سمك الهاتف
- mobile_wt: وزن الهاتف
- n_cores: عدد النوى
- pc: الكاميرا الرئيسية
- px_height: دقة الشاشة (الارتفاع)
- px_width: دقة الشاشة (العرض)
- ram: الذاكرة العشوائية
- sc_h: ارتفاع الشاشة
- sc_w: عرض الشاشة
- talk_time: وقت التحدث
- three_g: دعم 3G
- touch_screen: شاشة لمس
- wifi: دعم الواي فاي
- price_range: نطاق السعر (الهدف)