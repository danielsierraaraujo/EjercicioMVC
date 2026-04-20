Sistema de Gestión Académica (Fullstack - Cloud)
Este proyecto es una aplicación web distribuida diseñada para la gestión de notas estudiantiles. La arquitectura se divide en un Backend robusto en .NET y un Frontend reactivo en React, ambos desplegados en la nube.

🚀 Enlaces del Proyecto
Aplicación en vivo (Vercel): https://ejercicio-mvc-front-z138.vercel.app/

Video de Sustentación: Ver explicación detallada

🛠️ Arquitectura Técnica
Backend (API REST)
Tecnología: ASP.NET Core Web API (.NET 10).

Base de Datos: Entity Framework Core (In-Memory Database).

Despliegue: Contenedorizado con Docker y alojado en Render.

Características Clave:

Documentación interactiva con Swagger.

Configuración de CORS para comunicación segura entre dominios.

Configuración de red forzada a IPv4 para compatibilidad en entornos Linux/Cloud.

Repositorio: GitHub - Backend

Frontend (SPA)
Tecnología: React.js con Hooks (useState).

Comunicación: Consumo de servicios mediante Axios.

Despliegue: Alojado en Vercel con integración continua desde GitHub.

Características Clave:

Interfaz de usuario responsiva y moderna.

Manejo de errores en tiempo real y validación de datos del lado del cliente.

Repositorio: GitHub - Frontend

📂 Funcionalidades Principales
Registro de Estudiantes: Permite ingresar el nombre, ID Banner y notas de progreso. Los datos se envían de forma asíncrona a la API.

Consulta de Reportes: Recupera dinámicamente la información de un estudiante mediante su Banner, calculando promedios desde el servidor.

Documentación Swagger: El backend incluye una interfaz Swagger para probar los endpoints GET y POST de manera aislada.

👤 Contacto
Nombre: Daniel Cárdenas

Institución: Universidad de las Américas (UDLA)

Correo: daniel.sierra@udla.edu.ec

💡 Nota para el Despliegue
Debido al uso de instancias gratuitas en Render, la API puede tardar aproximadamente 50 segundos en "despertar" tras el primer intento de comunicación desde el frontend. Por favor, espere un momento al realizar la primera operación.