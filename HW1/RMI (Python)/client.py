from locust import HttpUser, task, between, events
import Pyro4
import random
import time

server_ip = "192.168.211.129"  # IP máy server
ns = Pyro4.locateNS(host=server_ip)
uri = ns.lookup("orders.service")
order_service = Pyro4.Proxy(uri)

class PyroUser(HttpUser):
    wait_time = between(1, 3)  # Thời gian chờ giữa các request của mỗi user

    @task
    def test_order_service(self):
        product_id = f"P{str(random.randint(1, 1000)).zfill(4)}"
        quantity = random.randint(1, 10)
        
        start_time = time.time()
        try:
            response = order_service.calculateTotal(product_id, quantity)

            total_time = int((time.time() - start_time) * 1000)  # Tính thời gian request theo ms
            self.environment.events.request.fire(
                request_type="PYRO",
                name="calculateTotal",
                response_time=total_time,
                response_length=len(str(response))
            )
            
            print(response)
        except Exception as e:
            total_time = int((time.time() - start_time) * 1000)
            self.environment.events.request.fire(
                request_type="PYRO",
                name="calculateTotal",
                response_time=total_time,
                exception=e
            )
