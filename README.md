# Cantina
![Star Wars](https://i.giphy.com/l3fZEXqPPadtqMRyM.webp)
## Architecture: Async CQRS
![CQRS Architecture](https://tjabanestorage.blob.core.windows.net/uploads/Cantina.drawio.png?sp=r&st=2025-05-05T07:40:45Z&se=2025-08-01T15:40:45Z&spr=https&sv=2024-11-04&sr=b&sig=Y8Y%2FFInYXMGFJb9WTC%2F6Ts8X3kAtmJGlrAXQnmYckwE%3D) 

Since the solution will be running on an old server I decided to implement CQRS design with async communication on commands using [Apache Kafka](https://kafka.apache.org), [Redis](https://redis.io) for NoSql read only database and microsoft sql server for the storage. The commands will  be written first into the queue and later processed into the database.
The benefits of this design are

- **Decoupling** Adds a buffer between the rest api and the database, making systems more flexible and easier to maintain. 

- **Improved Performance** Allows components to process messages at their own pace, preventing bottlenecks and improving overall system speed. This will allow the database to commit changes without putting too much stress on it. When the traffic is high the message consumer can be turned off and all the resources will be on the rest api allowing it to scale. Once the traffic slows down the message consumer can be turned on to start processing the messages. Since we the application will be using kafka it can support high thoughput and store messages for as long as needed.

- **Enhanced Reliability** If a component fails, the queue can store messages until it recovers, ensuring data integrity.

- **Read Peformance** Redis stores data in memory, allowing for sub-millisecond response times, making it significantly faster than disk-based databases. . Redis allows us to reduce the load on a primary database while speeding up database reads. Reducing the load on the database and been efficient.

### Images
The menu images will be uploaded to a 3rd party cloud service like azure, aws or google.What will be stored in the database will be the image url. This approach will reducing the load on the server from reading images from storage or database into memory avoiding further stress on the old hardware. One of the benefits of this is using cloud CDN to cache images closer to our clients and hence improve the performance of the application.


## Observability
![Observability stack](https://tjabanestorage.blob.core.windows.net/uploads/Cantina%20Metrics.png?sp=r&st=2025-05-05T07:45:20Z&se=2025-08-01T15:45:20Z&spr=https&sv=2024-11-04&sr=b&sig=T0eK%2FRf4DOg7eIWLdAsSj5abqi1MuBeym%2BiJNMXHFR8%3D)
I used Open Telemetry standard for collecting metrics, traces and logs. Then send them to [Prometheus](https://prometheus.io) for storage and [grafana](https://grafana.com) to build dashboards. 
The metrics collected are:

- Request Metrics
- Error Metrics
- Request Size and Response Size
- Connection Metrics
- Request Rate
- Rate limiting
- Server health
- Reviews Count


These are useful for 
- **Monitoring Performance**: Understanding how long requests take and identifying bottlenecks.
- **Error Tracking**: Monitoring the rate of errors to detect issues in the application.
- **Traffic Analysis**: Observing the volume of incoming requests and active connections.
- **Resource usage**: Observe the amount of RAM and CPU been used.
- **Review Count**: Custom metric that tracked the amount of reviews received.

We can build dashboard like 
![server dashboard](https://www.mytechramblings.com/img/otel-metrics-runtime-perf-counters-and-process-dashboard.png)

![webapi dashboards](https://www.mytechramblings.com/img/otel-metrics-aspnet-core-metrics-dashboard.png)
## Use
To start the application run 
```
docker-compose up -d 
```
Then use the health check endpoint to see if everything is ok: https://localhost:8081/health <br/>
## Endpoints

- Swagger Endpoint: https://localhost:8081/swagger/index.html
- Health Check: https://localhost:8081/health
- Jaeger: http://localhost:16686/search
- Prometheus: http://localhost:9090/query
- Grafana: http://localhost:3000/

Admin User
  - "email": admin@cantina.com
  - "password": $400Project

Grafana user: 
  - "email": admin
  - "password": admin