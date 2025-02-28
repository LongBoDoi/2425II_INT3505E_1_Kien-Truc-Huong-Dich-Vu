from locust import User, task, between, events
import grpc
import order_service_pb2
import order_service_pb2_grpc
import random
import time

server_ip = "192.168.211.129:50051"  # IP server

class GRPCClient:
    def __init__(self):
        self.channel = grpc.insecure_channel(server_ip)
        self.stub = order_service_pb2_grpc.OrderServiceStub(self.channel)

    def calculate_total(self, product_id, quantity):
        request = order_service_pb2.OrderRequest(product_id=product_id, quantity=quantity)
        return self.stub.CalculateTotal(request)

class GRPCUser(User):
    wait_time = between(1, 3)  # Thoi gian cho giua moi request
    client = None

    def on_start(self):
        self.client = GRPCClient()

    @task
    def test_order_service(self):
        # Random san pham, so luong
        product_id = f"P{random.randint(1, 1000):04d}" 
        quantity = random.randint(1, 10)

        start_time = time.time()
        try:
            response = self.client.calculate_total(product_id, quantity)

            total_time = int((time.time() - start_time) * 1000)
            events.request.fire(
                request_type="gRPC",
                name="CalculateTotal",
                response_time=total_time,
                response_length=len(response.message)
            )
            
            print(response)
        except Exception as e:
            total_time = int((time.time() - start_time) * 1000)
            events.request.fire(
                request_type="gRPC",
                name="CalculateTotal",
                response_time=total_time,
                exception=e
            )
