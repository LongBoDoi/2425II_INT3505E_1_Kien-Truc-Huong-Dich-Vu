from locust import User, task, between
from zeep import Client
import random, time

server_ip = "192.168.211.129"

client = Client(f"http://{server_ip}:5000/order-service?wsdl")

class SoapUser(User):
    wait_time = between(1, 3)

    @task
    def test_calculate_total(self):
        product_id = f"P{str(random.randint(1, 1000)).zfill(4)}"
        quantity = random.randint(1, 10)

        start_time = time.time()
        try:
            response = client.service.processOrder(product_id, quantity)

            total_time = int((time.time() - start_time) * 1000)  # Tính thời gian request theo ms
            self.environment.events.request.fire(
                request_type="SOAP",
                name="processOrder",
                response_time=total_time,
                response_length=len(str(response))
            )

            print(f"Response: {response}")
        except Exception as e:
            total_time = int((time.time() - start_time) * 1000)
            self.environment.events.request_failure.fire(
                request_type="SOAP",
                name="processOrder",
                response_time=total_time,
                exception=e
            )
