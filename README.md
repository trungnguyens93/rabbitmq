# rabbitmq

### 1. Basic Queue
- Exchange: ""
- Exchange Type: direct
- Queue: "basic-queue"

### 2. Work Queues
- Exchange: ""
- Exchange Type: direct
- Queue: "work-queues"
- Producer:
  - Persistent = true: Đánh dấu message bền vững (save message vào disk, điều này không được RabbitMQ đảm bảo thực hiện ngay, message đôi khi chỉ lưu trong cache)
  - Durable = true: Khởi tạo queue với thuộc tính durable là true, đảm bảo queue sẽ không bị mất khi mà RabbitMQ gặp sự cố hay khởi động lại 
- Consumer:
  - BasicQos với [prefetchCount = 1]: nói với RabbitMQ rằng không gửi nhiều hơn 1 message đến 1 consumer vào một lúc

