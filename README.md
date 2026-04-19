# MealPulse


## Description

MealPulse is a personal full-stack web application focused on calorie tracking and nutrition management. The system allows users to log their daily food intake, track calories, and set goals for gaining, losing, or maintaining weight.

The application uses a Microsoft SQL Server database populated with scraped food data, including nutritional values and barcodes. Users can log food manually, scan barcodes for quick input, or use an integrated AI chatbot to log meals through natural language (e.g. "I ate a banana").

The project was developed individually as part of my software engineering learning process, with a strong focus on clean architecture, user experience, and practical feature implementation.



## Features

- User authentication and authorization  
- Daily food diary (breakfast, lunch, dinner, snacks)  
- Calorie tracking and goal calculation  
- Barcode scanning for quick food logging  
- Large food database with nutritional values (scraped data)  
- AI chatbot for natural language food logging  
- Automatic calorie calculation based on user goals  
- Responsive UI with modal-based interactions  



## Tech Stack

- Backend: ASP.NET Core (MVC)  
- Frontend: Razor Views + Bootstrap  
- Database: Microsoft SQL Server  
- AI Integration: OpenAI API  
- Barcode Scanning: QuaggaJS  
- Architecture: Layered (Controllers, Services, Repositories)  



## Usage

- Register and log in  
- Set your weight goal (gain, lose, maintain)  
- Log food manually or via barcode scanning  
- Use the chatbot to log meals using natural language  
- Track daily calorie intake and progress  



## Project Structure

MealPulse/  
├── Controllers/  
├── Services/  
├── Repositories/  
├── Models/  
├── Views/  
├── wwwroot/  
└── Program.cs  



## Role & Contribution

This was an individual project where I was responsible for:

- Designing and implementing the full system architecture  
- Developing backend logic and database structure  
- Implementing barcode scanning functionality  
- Integrating OpenAI API for chatbot-based food logging  
- Creating a responsive and user-friendly interface  
- Deploying the application on Fontys servers  




## Authors

Developed individually as a personal project

--

## License

This project was created for educational purposes and is not intended for commercial use.

--
