# Cantina
## Architecture: Async CQRS
![CQRS Architecture](https://miro.medium.com/v2/resize:fit:551/0*peL7BhC5R4MWoivn.png) 

Since the solution will be running on an old server I decided to implement CQRS design with async communication on commands using [Apache Kafka](https://kafka.apache.org), [Redis](https://redis.io) for read only database and microsoft sql for the storage. The commands will  be written first into the queue and later processed into the database.
<img src="https://miro.medium.com/v2/resize:fit:1400/1*a_MIJzJQX0St3M9QSDi_Mg.png" width="100%" height="400">
The benefits of this design are

- **Decoupling** Adds a buffer between the rest api and the database, making systems more flexible and easier to maintain. 

- **Improved Performance** Allows components to process messages at their own pace, preventing bottlenecks and improving overall system speed. This will allow the database to commit changes without putting too much stress on it. When the traffic is high the message consumer can be turned and all the resources will be on the rest api allowing it to scale.

- **Enhanced Reliability** If a component fails, the queue can store messages until it recovers, ensuring data integrity.

- **Read Peformance** Redis stores data in RAM, allowing for sub-millisecond response times, making it significantly faster than disk-based databases. Since the application will be storing menu items and aggragating the reviews, the amount of ram used will be small.


## Project Design
I used the mediator design pattern and structure the solution by clean architecture.
![Clean architecture](https://blog.cleancoder.com/uncle-bob/images/2012-08-13-the-clean-architecture/CleanArchitecture.jpg)

## Observability
I used Open Telemetry for observability and didnt get the chance to setup the dashboards.

## Use
Swagger Endpoint: https://localhost:8081/swagger/index.html
Health Check: https://localhost:8081/health
Jaeger: http://localhost:16686/search
Prometheus: http://localhost:9090/query
Admin User
  "email": "admin@cantina.com",
  "password": "$400Project"