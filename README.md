# BloodBankManagementSystem
# 📌 Overview

BloodBankManagementSystem is a full-stack web application designed to streamline blood donation, storage, and distribution processes. It ensures efficient management of donors, recipients, and blood inventory using ASP.NET Core Web API, Blazor WebAssembly, and Entity Framework Core.

# 🚀 Features

🩸 Donor & Recipient Registration – Manage donor and recipient profiles.

🏥 Hospital & Blood Bank Management – Track blood availability in real-time.

🔎 Blood Compatibility Matching – Ensures accurate blood transfusions.

📊 Donation & Request Tracking – Logs all donation and transfusion activities.

📢 Notifications & Alerts – Alerts for low inventory and urgent blood requests.

🔒 Secure Authentication – User authentication via JWT.

☁ Cloud Integration – Hosted on Azure with Docker for scalability.

📌 Multi-role Access Control – Different access levels for admins, hospitals, and donors.

📜 Automated Reports & Analytics – Generate reports on donation trends and inventory status.

🏦 Integration with Payment Gateway – Allows hospitals to manage service fees.

🌐 Multi-language Support – Support for multiple languages for broader accessibility.

🛠️ Tech Stack

Frontend: Blazor WebAssembly, Bootstrap

Backend: ASP.NET Core Web API, C#, Dapper, Entity Framework Core

Database: Microsoft SQL Server (MSSQL)

DevOps: Docker, Azure, CI/CD Pipelines

Authentication: JWT, OAuth2

Payments: Stripe API

# 📊 System Overview (Graph Representation)

BloodBankManagementSystem Data Flow:

+--------------------+
|  Donor/Recipient  |
+--------------------+
        |
        v
+----------------------+
|  Blood Inventory    |
+----------------------+
        |
        v
+------------------------+
|  Hospital Requests    |
+------------------------+
        |
        v
+---------------------------+
|  Matching & Allocation   |
+---------------------------+
        |
        v
+---------------------------+
|  Reports & Analytics     |
+---------------------------+

📈 Blood Inventory Sample Graph

Blood Inventory Levels (Sample Data)
-------------------------------------
Blood Type  | A+ | A- | B+ | B- | O+ | O- | AB+ | AB- |
-------------------------------------
Units (ml)  | 500 | 300 | 450 | 200 | 600 | 150 | 350 | 100 |
-------------------------------------

(This can be visualized using charts in the Blazor UI)

📌 Installation Guide

Clone the Repository:

git clone https://github.com/im-rakesh0827/BloodBankManagementSystem.git
cd BloodBankManagementSystem

Set up the Database:

Configure MSSQL Server

Run EF Core Migrations:

dotnet ef database update

Run the Application:

docker-compose up
dotnet run

Access in Browser:

Frontend: http://localhost:5000

API: http://localhost:5001/swagger

📜 License

This project is licensed under the MIT License.

🤝 Contributing

Contributions are welcome! Feel free to submit pull requests.

# 📬 Contact

Developer: Rakesh

GitHub: im-rakesh0827

Email: rakesh@example.com