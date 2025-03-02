from locust import HttpUser, task, between
import random, time
import requests

server_ip = "192.168.211.129"
class RestUser(HttpUser):
    wait_time = between(1, 3)   # Thời gian chờ giữa mỗi request

    @task
    def process_order(self):
        payload = {
            "product_id": f"P{str(random.randint(1, 1000)).zfill(4)}",
            "quantity": random.randint(1, 10)
        }
        headers = {"Content-Type": "application/json"}

        start_time = time.time()
        try:
            response = requests.post(f"http://{server_ip}:5000/CreateOrder", json=payload, headers=headers)

            total_time = int((time.time() - start_time) * 1000)  # Tính thời gian request theo ms
            self.environment.events.request.fire(
                request_type="REST",
                name="create-order",
                response_time=total_time,
                response_length=len(str(response.json()))
            )

            print(f"Response: {response.json()}")
        except Exception as e:
            total_time = int((time.time() - start_time) * 1000)
            self.environment.events.request.fire(
                request_type="REST",
                name="create-order",
                response_time=total_time,
                exception=e
            )