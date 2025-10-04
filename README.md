test jenkins
---

# ASP.NET Core MVC – Project Structure & Basics

## 1. MVC Model

* **Model**: dữ liệu, logic.
* **View**: giao diện (cshtml).
* **Controller**: trung gian, nhận request → xử lý → response.

---

## 2. Data Binding

* **Strongly typed model**: `return View(student);` + `@model Student`
* **ViewData / ViewBag**: truyền key–value
* **TempData**: giữ dữ liệu qua 1 lần redirect

---

## 3. Dependency Injection (DI)

Đăng ký trong `Program.cs`:

```csharp
builder.Services.AddScoped<IStudentService, StudentService>();
```

Controller chỉ inject **interface**:

```csharp
public StudentController(IStudentService service) { ... }
```

---

## 4. Redirect

* `RedirectToAction("Index", "Home")`
* `Redirect("/Home/Index")`
* `RedirectToRoute("default")`
* `LocalRedirect("/Home/About")`

---

## 5. Folder Structure

```
/Entities
    Student.cs
    MyDbContext.cs
    /Migrations        <-- chứa file migration

/Models
    /ErrorViewModels        <-- cho View
    /DTOs              <-- xử lý request (input/output)

/Services
    IStudentService.cs <-- interface nghiệp vụ

/ServicesImpl
    StudentService.cs  <-- implement interface

/Controllers
    StudentController.cs
```

---

## 6. Workflow

1. **Entities** ↔ Database (qua DbContext + Migration)
2. **DTOs** nhận dữ liệu request
3. **Service** (interface) định nghĩa nghiệp vụ
4. **ServiceImpl** implement service, xử lý DB/logic
5. **Controller** inject service → trả **ViewModel** cho View

---

## 7. Scaffold Database (reverse engineer từ DB có sẵn)

Ví dụ database MSSQL trên localhost với user `sa` và password `123`:

```bash
dotnet ef dbcontext scaffold "Server=localhost;Database=MyDb;User Id=sa;Password=123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Entities -c MyDbContext -f
```

* `-o Entities`: xuất entity vào folder Entities
* `-c MyDbContext`: tên DbContext
* `-f`: ghi đè nếu đã có

---

## 8. Migration & Update Database

Thêm migration mới (trong folder `Entities/Migrations`):

```bash
dotnet ef migrations add InitDb -o Entities/Migrations
```

Cập nhật database:

```bash
dotnet ef database update
```



