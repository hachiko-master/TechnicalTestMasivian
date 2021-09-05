# TechnicalTestMasivian - Roulette
This is a Technical Test for Masivian Company, It was developed with strongly typed language C # .NET, using WebAPI with NET Core 3, SQL Server Database with direct connection (without ORM's) with connection string in JSON. The project complies with the Clean Code rules.

Esta es la prueba técnica para la empresa Masivian, fue desarrollado con lenguaje fuertemente tipado C # .NET, usando WebAPI con NET Core 3, Base de Datos SQL Server con conexión directa (sin ORM's) con cadena de conexión en JSON. El proyecto cumple con las reglas del Clean Code.

A continuación se listan los requisitos solicitados.
The requested requirements are listed below:

# Requirements, Requerimientos
  * Endpoint de creación de nuevas ruletas que devuelva el id de la nueva ruleta creada
  * Endpoint de apertura de ruleta (el input es un id de ruleta) que permita las
posteriores peticiones de apuestas, este debe devolver simplemente un estado que
confirme que la operación fue exitosa o denegada.
  * Endpoint de apuesta a un número (los números válidos para apostar son del 0 al 36)
o color (negro o rojo) de la ruleta una cantidad determinada de dinero (máximo
10.000 dólares) a una ruleta abierta.
nota: este enpoint recibe además de los parámetros de la apuesta, un id de usuario
en los HEADERS asumiendo que el servicio que haga la petición ya realizo una
autenticación y validación de que el cliente tiene el crédito necesario para realizar la
apuesta.
  * Endpoint de cierre apuestas dado un id de ruleta, este endpoint debe devolver el
resultado de las apuestas hechas desde su apertura hasta el cierre.
El número ganador se debe seleccionar automáticamente por la aplicación al cerrar
la ruleta y para las apuestas de tipo numérico se debe entregar 5 veces el dinero
apostado si atinan al número ganador, para las apuestas de color se debe entrega 1.8
veces el dinero apostado, todos los demás perderán el dinero apostado.
nota: para seleccionar el color ganador se debe tener en cuenta que los números
pares son rojos y los impares son negros.
  * Endpoint de listado de ruletas creadas con sus estados (abierta o cerrada)
  
  # Endpoints
  ```
  POST https://localhost:44325/api/Roulette/create/
  PUT https://localhost:44325/api/Roulette/open/{rouletteId}
  POST https://localhost:44325/api/Roulette/bet/{rouletteId}    <-- With header {userId} and body "{"option":[some value between -2 and 36],"money":[some value]}"
                                                                Example: HEADER: userId=1, BODY: {"option":-1,"money":8000.00}
  PUT https://localhost:44325/api/Roulette/close/{rouletteId}
  GET https://localhost:44325/api/Roulette/all/
  ```
  
  # Configurations
  Configure the ```connectionString``` in [appsettings.json](TechnicalTestMasivian/appsettings.json)
  ```
  ...
  "ConfigGlobalValues": {
    "ConnectionString": "Data Source=DESKTOP-33A93CL;Initial Catalog=TechnicalTestMasivian;User ID=sa;Password=prueba"
  }
  ...
  ```
  The file for create the database is [sqlTechnicalTestMasivian.sql](sqlTechnicalTestMasivian.sql)
  
  Developed by - Julián Andrés Márquez Vélez, 
  Thanks
