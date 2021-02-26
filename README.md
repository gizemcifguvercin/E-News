# E-News
This project is about how we can send news to related news agency.

Use Case

<br/> 1- User/System starts event by posting news data to Report API
<br/> 2- Report API saves data to DB and produce data into queue
<br/> 3- ReportConsumer as webhook, consumes data from queue,
gets agency info from cache or db and send data to related agency integration

<b>Architecture</b>
<hr>

<img src="https://i.ibb.co/3zgZYTv/arch.png"/>

<hr>
<b>Technical Overview</b>

<br/>.Net Core 3.1
<br/>MongoDB
<br/>RabbitMQ with MassTransit Framework
<br/>MediatR
<br/>HealthCheck
<br/>Rest Api
<br/>Consumer
<br/>Memory Cache
<br/>Logging framework Serilog
<br/>Swagger 
<br/>Docker Container 
<br/>Domain Driven Design
<br/><br/>Domain Events
<br/>Command Handlers Patterns
<br/>CQRS
<br/>OOP
<br/>Elastic Search APM 
<br/>Kibana
<br/>SOLID

<hr>
<br/>Mongo DB Collections
<br/>News
<br/>AgencyInfo
<br/><img src="https://i.ibb.co/3dfTgv3/Screen-Shot-2021-01-01-at-18-19-59.png"/>

<br/>Senaryoyu anlatmak ve bilgi vermek amacıyla iki adet eğitim videosu oluşturdum, aşağıdaki linklerden izleyebilirsiniz. 
<br/>1- https://www.youtube.com/watch?v=T9q_oJK_QrE
<br/>2- https://www.youtube.com/watch?v=TfS4EDrewF4

<br/>Blog: https://gcifguvercin.medium.com/webhooks-i%CC%87le-development-zaman%C4%B1n%C4%B1-0a-indirme-1-e8793d8f0a9e
