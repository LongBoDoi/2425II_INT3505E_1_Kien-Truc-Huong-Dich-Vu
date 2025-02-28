import grpc
import order_service_pb2
import order_service_pb2_grpc

from locust import HttpUser, task, between
import random

class GRPCUser(HttpUser):
    wait_time = between(1, 3)  # Thời gian chờ giữa các request của mỗi user

    @task
    def test_order_service(self):
        channel = grpc.insecure_channel("192.168.211.129:50051")  # Change "localhost" to server IP if running on another machine
        stub = order_service_pb2_grpc.OrderServiceStub(channel)

        product_id = f"P{str(random.randint(1, 1000)).zfill(4)}"
        quantity = random.randint(1, 10)

        request = order_service_pb2.OrderRequest(product_id=product_id, quantity=quantity)
        response = stub.CalculateTotal(request)
        print(response.message)