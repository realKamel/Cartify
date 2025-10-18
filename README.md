# 🛒 Cartify - Full Stack E-Commerce Web App
___
An ambitious, full-stack, e-commerce solution built on .NET 9 and featuring Angular 20 with SSR.
This project runs on a Decoupled Services Model, requiring separate processes for the front-end and back-end.
___
## 1. About the Project (Purpose)

This project serves as a personal deep-dive into building a production-ready application using modern .NET practices, specifically focusing on building a robust, secure, and scalable online retail experience.
The architecture follows a Decoupled Services Model, running the high-performance Server-Side Rendered (SSR) front-end (Node.js) separately from the scalable ASP.NET Core API (C#).
- **Goal**: To create a scalable and feature-rich platform demonstrating modern API management and decoupling techniques.

- **Focus Areas**: 
  - Implementing a Decoupled Services Architecture requiring Cross-Origin Resource Sharing (CORS) setup on the API.
  - Utilizing Server-Side Rendering (SSR) for SEO and initial page load speed.
  - Demonstrating proficiency in Database (SQL/NoSQL) interaction using Entity Framework Core.
  - Implementing secure Identity and Authentication (e.g., JWT, Identity Server).
___
### 2. Key Features (Functional Scope)

Core User Modules
1. 🔐 User Authentication: Registration, Login, Password Reset (using ASP.NET Core Identity).
2. 🛍️ Product Catalog: Browse, search, filter, and view detailed product pages.
3. 🛒 Shopping Cart: Add, update, and remove items dynamically.
4. ❤️ Wishlist : Add, remove, add to Cart
5. 💳 Checkout Process: Multi-step process for shipping, payment selection, and order confirmation.
6. 📦 Order History: Users can view the status and details of past orders.

Administration Modules
1. ⚙️ Product Management: CRUD (Create, Read, Update, Delete) products and manage inventory levels.
2. 📈 Basic Reporting: View total sales and popular products.(Planned)
___
### 3. Technology Stack
#### Backend
| Component      | Technology            | Description                                               |
|----------------|-----------------------|-----------------------------------------------------------|
| Framework      | .NET 9                | Core platform for all services.                           |
| API            | ASP.NET Core Web API  | RESTful endpoints for the front-end.                      |
| Database       | PostgreSQL            | Primary data storage.                                     |
| ORM            | Entity Framework Core | Database abstraction and migration management.            |
| Authentication | ASP.NET Core Identity | User management and security.                             |
| Caching        | Redis                 | Distributed caching for product catalog and session data. |

#### Frontend
| Component  | Technology   | Description                                           |
|------------|--------------|-------------------------------------------------------|
| Framework  | Angular 20   | Interactive single-page application (SPA).            |
| Rendering  | SSR          | Server-Side Rendering via a dedicated Node.js server. |
| Styling    | Tailwind CSS | Utility-first or component-based styling.             |

#### Tooling & DevOps

- Version Control: Git & GitHub

- Testing: xUnit for unit/integration testing.

- CI/CD: GitHub Actions

### 4. Getting Started (Setup)

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

You need the following software installed:

- .NET 9 SDK

- Node.js 22+
- VS Code
- Visual Studio 2022 or Rider
- pgAdmin for PostgreSQL

### Installation

1. Clone the repository:
```bash
git clone https://github.com/realKamel/Cartify.git
git clone https://github.com/realKamel/Cartify.Client.git
````
2. Restore Backend Dependencies:
```bash
# Navigate to the solution folder containing the .sln file
cd ./Carify
dotnet restore
### 2.Restore Backend Dependencies (API):
dotnet ef database update --project [Your.Data.Project]
```
### 3.Start the .NET Reverse Proxy/API
```bash
dotnet run
```

### 4. Run the Frontend (Angular):

```bash
cd ../Cartify.Client
npm install
ng serve -o
```

**Angular application should now be accessible at http://localhost:4200.**

## 5. Project Status & Roadmap

This project is currently in the **Initial Development** phase.

Current Version: v0.1-alpha

### Future Roadmap
| Milestone           | Target                                                            | Status        |
|---------------------|-------------------------------------------------------------------|---------------|
| v0.2                | Implement basic Search functionality and filtering.               | ⏳ In Progress |
| v0.3                | Integrate a real Payment Gateway (e.g., Stripe/PayPal Sandbox)    | 📅 Planned    | 
| Future Architecture | implement YARP Reverse Proxy for unified entry point (Optional).  | 📅 Planned    |
| v1.0                | Full CI/CD pipeline setup and deploy to Azure/AWS.                | 💡 Idea       |
| Stretch Goal        | Implement product Reviews and Ratings.(Currently is Dummy rating) | 💡 Idea       |


## 7. License

Distributed under the MIT License. See LICENSE for more information.

Contact

Abdelrahman Ali - abdelrahman.kamel.dev@gmail.com - https://www.linkedin.com/in/real-kamel/

- Project API Link: https://github.com/realKamel/Cartify.git
- Project Frontend Link: https://github.com/realKamel/Cartify.Client.git