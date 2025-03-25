## 📁 Full Folder Structure

```
/CleanArchitectureMicroservicesStarter

│── docker-compose.yml                        --> Container Orchestration
│── README.md                                 --> Documentation
│── CleanArchitectureMicroservicesStarter.sln --> Solution File
```

## 📌 Services Involved
Order Service

Receives an order request.

Starts the Saga and publishes OrderCreated.

Inventory Service

Listens for OrderCreated.

Checks stock availability.

If stock is available → Publishes StockReserved.

If stock is not available → Publishes StockFailed.

Payment Service

Listens for StockReserved.

Processes payment.

If successful → Publishes PaymentProcessed.

If failed → Publishes PaymentFailed.

Order Service (Saga Orchestrator)

Listens for StockReserved and PaymentProcessed.

If both succeed → Confirms order.

If any step fails → Publishes a Compensating Event (OrderCancelled).

## 📌 Flow Diagram
```
1️⃣ User Requests Order  --->  OrderService (Saga Started)  
2️⃣ OrderService Publishes → OrderCreated  
3️⃣ InventoryService Listens → Checks Stock  
    ✔ Stock Available → Publishes StockReserved  
    ❌ Stock Not Available → Publishes StockFailed  

4️⃣ PaymentService Listens to StockReserved → Processes Payment  
    ✔ Payment Success → Publishes PaymentProcessed  
    ❌ Payment Failure → Publishes PaymentFailed  

5️⃣ OrderService Listens to StockReserved & PaymentProcessed  
    ✔ If Both Succeed → Order Confirmed ✅  
    ❌ If Any Step Fails → Publishes OrderCancelled (Rollback)  
```