# ADR-001: Usar SQL Server como motor de base de datos en HelpDesk RD

## Estado

Aceptado

## Contexto

Para HelpDesk RD estamos usando .NET con Entity Framework Core para manejar las entidades del sistema, como Ticket, Cliente, Agente y Categoría. Como la solución es un solo proyecto (eso ya se decidió aparte), no hay una capa separada de acceso a datos que nos obligara a pensar mucho en qué motor usar, simplemente seguimos con lo que ya conocíamos.

La verdad es que en el equipo nadie había trabajado antes con otro motor que no fuera SQL Server. En casi todas las materias de la carrera hemos usado SQL Server, así que ya teníamos SSMS instalado y sabíamos cómo conectarlo con un proyecto en .NET. No nos sentamos a comparar opciones antes de empezar, simplemente usamos lo que nos daba menos dolor de cabeza en ese momento, que era seguir con lo que ya sabíamos usar, sobre todo porque el tiempo para entregar el proyecto no es mucho.

## Decisión

Decidimos usar SQL Server como base de datos de HelpDesk RD. Se conecta desde la API en .NET usando Entity Framework Core con el paquete de SQL Server (Microsoft.EntityFrameworkCore.SqlServer). La cadena de conexión está en el appsettings.json, y las tablas se crean con las migraciones de EF Core que están en la carpeta Migrations.

## Alternativas consideradas

Como dije antes, en su momento no nos sentamos a comparar, así que aquí pongo lo que debimos haber revisado.

**PostgreSQL**
Es una base de datos también gratuita y de código abierto, muy usada fuera del mundo Microsoft. Tiene fama de ser muy completa y soporta cosas como JSON dentro de las columnas.
No la usamos porque ninguno del equipo había trabajado con ella antes, y nos tocaría aprender a usar pgAdmin y configurar el driver de Entity Framework para PostgreSQL (Npgsql), cosa que no necesitamos hacer con SQL Server porque ya viene "amigable" con .NET.

**MySQL**
Es otra opción gratis, muy popular en páginas web normales.
Tampoco la usamos porque el conector para Entity Framework no es de Microsoft directamente, es de un paquete de terceros (Pomelo), y eso nos daba algo de desconfianza por si dejaba de funcionar bien con la versión de .NET que estamos usando. Con SQL Server no tenemos ese riesgo porque el soporte es oficial.

## Consecuencias

**Lo bueno:**
- No tuvimos que aprender nada nuevo, ya sabíamos usar SQL Server de otras materias.
- La conexión con Entity Framework Core es directa, sin paquetes raros de terceros.
- Hay mucha documentación y ejemplos en español e inglés por ser tan usado.

**Lo malo / lo que queda pendiente:**
- La versión gratis de SQL Server (Express) tiene un límite de 10GB de base de datos, así que si el proyecto se usara de verdad en producción con muchos tickets, eso podría quedarse corto.
- Si algún día quisiéramos sacar esto de lo académico y llevarlo a producción real, SQL Server cuesta dinero para licencias, a diferencia de PostgreSQL o MySQL que son gratis siempre.
- Todo lo que tengamos hecho específico de SQL Server (si usamos algo de T-SQL puntual) habría que reescribirlo si en el futuro cambiamos de motor.
- Nunca comparamos en serio con otra opción antes de decidir, así que tampoco podemos decir con certeza que SQL Server era la mejor opción, fue más la opción más cómoda para el equipo en ese momento.
