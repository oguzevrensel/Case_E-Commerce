Bu proje, bir e-ticaret sisteminde **sipariş oluşturma**, **işleme**, **loglama**, **cacheleme** ve **bildirim gönderme** gibi temel işlevleri içeren, modüler ve ölçeklenebilir bir mimariyle geliştirilmiş bir backend sistemidir.

## 📦 Proje Katmanları

- **MiniEcommerceCase (API):** .NET 8 Web API uygulaması. Sipariş alma, kullanıcı doğrulama ve veri iletim işlemlerini içerir.
- **MiniEcommerceCase.Application:** DTO'lar, servis arayüzleri, AutoMapper konfigürasyonları.
- **MiniEcommerceCase.Domain:** Domain entity’leri, enum’lar ve base sınıflar.
- **MiniEcommerceCase.Infrastructure:** Servis implementasyonları, veritabanı bağlantısı (EF Core), RabbitMQ publisher, Redis cache, Serilog entegrasyonu.
- **MiniEcommerceCase.WorkerService:** RabbitMQ kuyruğunu dinleyen, sipariş işleyen ve Redis’e log yazan ayrı bir Console uygulamasıdır.

---
