<p align="center">

</p>

# 💪 Gym Management System

A modern, comprehensive platform for managing gym operations, members, trainers, sessions, and memberships. Built with ASP.NET Core MVC (.NET 9.0), Entity Framework Core, and SQL Server, this system streamlines gym administration, member management, booking operations, and analytics for fitness centers and gym facilities.

---

## 🚀 Features

- 👤 **Member Management**: Registration, profile management, health records tracking, photo uploads

- 🏋️ **Trainer Management**: Trainer profiles, specialties (General Fitness, Yoga, Boxing, CrossFit), session assignments

- 📅 **Session Management**: Create and manage training sessions with categories, capacity limits, date/time scheduling

- 📝 **Booking System**: Members can book sessions, view availability, manage their bookings

- 💳 **Membership Plans**: Multiple membership tiers (Basic, Standard, Premium, Annual) with flexible pricing and duration

- 📊 **Analytics Dashboard**: Track gym statistics, member enrollment, session attendance, and performance metrics

- 🏥 **Health Records**: Store and manage member health information and medical records

- 🔐 **Authentication & Authorization**: Secure login system with ASP.NET Core Identity, role-based access control

- 📁 **File Management**: Upload and manage member photos, attachments, and documents

- 🎯 **Category Management**: Organize sessions by categories (Yoga, Boxing, CrossFit, General Fitness)

- 🔍 **Search & Filtering**: Find members, trainers, sessions, and bookings efficiently

- 📈 **Real-time Statistics**: View enrollment stats, active memberships, session capacity, and more

---

## 🛠️ Tech Stack

- **Framework**: [ASP.NET Core MVC](https://dotnet.microsoft.com/apps/aspnet/mvc) (.NET 9.0)

- **Database**: SQL Server with [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)

- **Authentication**: [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)

- **Mapping**: [AutoMapper](https://automapper.org/)

- **Architecture**: Three-Layer Architecture (DAL, BLL, PL)

- **Frontend**: Razor Views, CSS, JavaScript

- **ORM**: Entity Framework Core 9.0

---

## 📂 Project Structure

```
Gym-Management-System-Production/
├── GymManagementDAL/          # Data Access Layer
│   ├── Data/
│   │   ├── Context/           # DbContext configuration
│   │   ├── Configurations/    # Entity configurations
│   │   ├── Migrations/        # Database migrations
│   │   └── DataSeed/          # Database seeding
│   ├── Entities/              # Domain models
│   │   └── ENUMS/             # Enumerations
│   └── Repositories/          # Repository pattern implementation
│       ├── Classes/
│       └── Interfaces/
│
├── GymManagementBLL/          # Business Logic Layer
│   ├── Services/              # Business services
│   │   ├── Classes/           # Service implementations
│   │   └── Interfaces/        # Service contracts
│   └── ViewModels/            # Data transfer objects
│
├── GymManagementPL/           # Presentation Layer
│   ├── Controllers/           # MVC controllers
│   ├── Views/                 # Razor views
│   ├── Models/                # View models
│   ├── wwwroot/               # Static files (CSS, JS, images)
│   └── Program.cs             # Application entry point
│
└── GymManagementSystemSolution.sln  # Solution file
```

---

## 🖥️ How to Run Locally

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) with C# extension

### Setup Instructions

1. **Clone the repository**

   ```bash
   git clone <your-repo-url>
   cd Gym-Management-System-Production
   ```

2. **Configure the database connection**

   - Open `GymManagementPL/appsettings.json` or `appsettings.Development.json`
   - Update the connection string:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=GymManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

3. **Restore NuGet packages**

   ```bash
   dotnet restore
   ```

4. **Run database migrations**

   The application automatically runs migrations on startup. Alternatively, you can run:

   ```bash
   cd GymManagementPL
   dotnet ef database update --project ../GymManagementDAL
   ```

5. **Run the application**

   ```bash
   cd GymManagementPL
   dotnet run
   ```

   Or use Visual Studio:
   - Set `GymManagementPL` as the startup project
   - Press `F5` to run

6. **Access the application**

   - Visit [https://localhost:5001](https://localhost:5001) or [http://localhost:5000](http://localhost:5000)
   - The default route redirects to `/Account/Login`

### 🔑 Demo Accounts

For testing purposes, you can use the following pre-configured accounts:

#### Super Admin
- **Email**: `hassanmohamedali0113@gmail.com`
- **Password**: `P@ssw0rd`

#### Admin
- **Email**: `ahmedrahmo@gmail.com`
- **Password**: `P@ssw0rd`

---

## 🗄️ Database Schema

The system includes the following main entities:

- **Members**: Gym members with profiles and health records
- **Trainers**: Fitness trainers with specialties
- **Sessions**: Training sessions with categories and capacity
- **Bookings**: Member session bookings
- **Memberships**: Member subscription plans
- **Plans**: Membership plan templates
- **Categories**: Session categories
- **HealthRecords**: Member health information

---

## 🖼️ Screenshots

### 🔐 Login & Registration

Secure login and registration forms for gym staff and members.

 <img src="GymManagementPL/wwwroot\images\Screenshots\Login\1.png" alt="Gym Management Logo" width="600"/>

### 🏠 Home 

Analytics and overview of gym operations.

<img src="GymManagementPL/wwwroot\images\Screenshots\Dashboard\1.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Dashboard\2.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Dashboard\3.png" alt="Gym Management Logo" width="600"/>


### 👤 Member Management

Manage member profiles, health records, and memberships.

<img src="GymManagementPL/wwwroot\images\Screenshots\Member\1.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Member\2.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Member\3.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Member\4.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Member\5.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Member\6.png" alt="Gym Management Logo" width="600"/>


### 🏋️ Trainer Management

View and manage trainer profiles and specialties.

<img src="GymManagementPL/wwwroot\images\Screenshots\Trainer\1.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Trainer\2.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Trainer\3.png" alt="Gym Management Logo" width="600"/>


### 📅 Session Management

Create and manage training sessions with categories and capacity.

<img src="GymManagementPL/wwwroot\images\Screenshots\Session\1.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Session\2.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Session\3.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Session\4.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Session\5.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Session\6.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Session\7.png" alt="Gym Management Logo" width="600"/>




### 📝 Booking Management

Members can book sessions and view their booking history.

<img src="GymManagementPL/wwwroot\images\Screenshots\Booking\1.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Booking\2.png" alt="Gym Management Logo" width="600"/>

### 💳 Membership Plans

Configure and manage membership plans with pricing and duration.

<img src="GymManagementPL/wwwroot\images\Screenshots\Membership\1.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Membership\2.png" alt="Gym Management Logo" width="600"/>
<img src="GymManagementPL/wwwroot\images\Screenshots\Membership\3.png" alt="Gym Management Logo" width="600"/>

---

## 📌 Key Features Details

### Member Management
- Create, update, and delete member profiles
- Upload member photos
- Track health records
- Manage multiple memberships per member
- View booking history

### Trainer Management
- Manage trainer profiles with specialties
- Assign trainers to sessions
- Track trainer session assignments
- View trainer performance

### Session Management
- Create sessions with categories
- Set capacity limits
- Schedule start and end dates
- Assign trainers to sessions
- Track member enrollments

### Booking System
- Members can book available sessions
- Check session availability
- View booking history
- Manage active bookings

### Analytics
- Total members count
- Active memberships
- Session statistics
- Enrollment metrics
- Performance tracking

---

## 🔧 Configuration

### Environment Variables

The application uses `appsettings.json` for configuration. Key settings include:

- **ConnectionStrings**: Database connection string
- **Logging**: Logging configuration
- **Identity**: Authentication and authorization settings

### Database Seeding

The application includes automatic data seeding:
- Default roles (Admin, Member, Trainer)
- Sample membership plans
- Initial admin user (configure in `IdentityDbContextSeeding.cs`)

---

## 📝 Notes

- The application uses **Code-First** approach with Entity Framework Core migrations
- Database migrations run automatically on application startup
- File uploads are stored in `wwwroot/images/Members/`
- The system uses **Repository Pattern** and **Unit of Work** for data access
- **AutoMapper** is used for object-to-object mapping between entities and view models
- Authentication is handled by **ASP.NET Core Identity**

---

## 🧪 Testing

To run tests (if available):

```bash
dotnet test
```

---

## 📫 Contact

- **GitHub**: [Hassanoli](https://github.com/Hassanoli)
- **Email**: hassanmohamedali0113@gmail.com

---

## 💬 Contributing & Support

We welcome contributions, suggestions, and bug reports! Feel free to fork the repo, open issues, or submit pull requests. If you need help, please reach out via GitHub or email.

**Happy coding and building! 🚀**

---

## 📄 License

This project is open source and available under the [MIT License](LICENSE).

